using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{
    AudioSource audioComponent;
    [SerializeField] float CrashDelay = 0;
    [SerializeField] float LevelChangeDelay = 0;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound; 
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem MainThrustParticles;
    [SerializeField] ParticleSystem LeftThrustParticles;
    [SerializeField] ParticleSystem RightThrustParticles;
    Rigidbody rb;

    bool isTransitioning = false;
    bool isColliding = true;
   

    void Start() {
        audioComponent = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if(Input.GetKey(KeyCode.C))
        {
            DisableCollision();
        }
    }
    void DisableCollision()
    {
        isColliding = !isColliding;
    }
    void OnCollisionEnter(Collision other)
    {
        if(isTransitioning || !isColliding){return;}
        string gameObjectTag = other.gameObject.tag;    
        CheckCollisionInfo(gameObjectTag);
    }

    void CheckCollisionInfo(string gameObjectTag)
    {
        switch (gameObjectTag) {
            case "Finish":
                if(transform.rotation.x  < 2f  && transform.rotation.y < 2f && transform.rotation.z >= -0.05f && transform.rotation.z <= 0.05f)
                {
                    StartChangeLevelSequence();
                }
                else{
                    StartCrashSequence();
                }
                break;
            case "Friendly":
                Debug.Log("The object you touched is friendly");
                break;
            default :
                Debug.Log("You died by touching the obstacle");
                StartCrashSequence();
                break;
       }
    }

    void StartChangeLevelSequence()
    {
        isTransitioning = true;
        isColliding = true;
        NextLevelTransitionInfo();
        GetComponent<Movement>().enabled = false;
        StopAllParticles();
        Invoke("NextLevel", LevelChangeDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        CrashSequenceInfo();
        GetComponent<Movement>().enabled = false;
        StopAllParticles();
        Invoke("RestartScene", CrashDelay);
    }
    void NextLevelTransitionInfo()
    {
        winParticles.Play();
        audioComponent.Stop();
        audioComponent.PlayOneShot(winSound);
    }
    void CrashSequenceInfo()
    {
        crashParticles.Play();
        audioComponent.Stop();
        audioComponent.PlayOneShot(deathSound);
    }

    void StopAllParticles()
    {
        LeftThrustParticles.Stop();
        RightThrustParticles.Stop();
        MainThrustParticles.Stop();
    }
    void NextLevel()
    {
        int NextSceneIndex;
        Debug.Log(SceneManager.sceneCountInBuildSettings);
        if(SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            NextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        }
        else{
            NextSceneIndex = 0;
        }
        Debug.Log(SceneManager.GetSceneByBuildIndex(NextSceneIndex));
        SceneManager.LoadScene(NextSceneIndex);
    }

    void RestartScene()
    {
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(CurrentSceneIndex);

    }




}
