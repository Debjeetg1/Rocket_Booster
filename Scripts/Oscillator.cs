using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 StartingPosition;
    [SerializeField]Vector3 MovementPosition;
    [SerializeField][Range(0,1)] float movementFactor;
    [SerializeField] float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        float sinWave = Mathf.Sin(Time.time);
        
        movementFactor = (sinWave + 1)/2 * speed;
        
        Debug.Log(movementFactor);
        Vector3 Offset = MovementPosition * movementFactor;
        transform.position = StartingPosition + Offset;
        Debug.Log(StartingPosition + Offset);

    }
}
