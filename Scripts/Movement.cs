using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{

    Rigidbody rb;
    AudioSource audioComponent;
    [SerializeField] float Thrust = 1.0f;
    [SerializeField] float RotationThrust = 1.0f;
    [SerializeField] AudioClip gameEngine;
    [SerializeField] ParticleSystem MainThrustParticles;
    
    [SerializeField] ParticleSystem RightThrustParticles;
    [SerializeField] ParticleSystem LeftThrustParticles;

    
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioComponent = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessRotation();
        ProcessThrust();
        ProcessCheatKey();
    }

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrustParticlesAndSound();
        }

    }
    void ProcessRotation()
    {
        
        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            LeftThrustParticles.Stop();
        }
        else if(Input.GetKeyUp(KeyCode.D))
        {
            RightThrustParticles.Stop();
        }
    }

    void ProcessCheatKey()
    {
        if(Input.GetKey(KeyCode.L))
        {
            ChangeLevelCheat();
        }
    }
    void StartThrusting()
    {
        if (!audioComponent.isPlaying)
        {
            audioComponent.PlayOneShot(gameEngine);

        }
        if (!MainThrustParticles.isPlaying)
        {
            MainThrustParticles.Play();
        }
        rb.AddRelativeForce(Vector3.up * Thrust * Time.deltaTime);
    }
    void StopThrustParticlesAndSound()
    {
        audioComponent.Stop();
        MainThrustParticles.Stop();
    }

   void RotateLeft()
    {
        RightThrustParticles.Stop();
        if (!LeftThrustParticles.isPlaying)
        {
            LeftThrustParticles.Play();
        }
        ApplyRotation(RotationThrust);
    }

    void RotateRight()
    {
        LeftThrustParticles.Stop();
        if (!RightThrustParticles.isPlaying)
        {
            RightThrustParticles.Play();
        }
        ApplyRotation(-RotationThrust);
    }

    void ChangeLevelCheat()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        int MaxBuildIndex = SceneManager.sceneCountInBuildSettings;

        if (buildIndex != MaxBuildIndex - 1)
        {
            SceneManager.LoadScene(buildIndex + 1);
        }
        else
        {
            buildIndex = 0;
            SceneManager.LoadScene(buildIndex);
        }
    }


    void ApplyRotation(float RotationThrust)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward *RotationThrust* Time.deltaTime);
        rb.freezeRotation = false;
    }
}
