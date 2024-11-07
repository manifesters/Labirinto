using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 0.5f;    // Speed of the cloud's movement
    public float range = 2f;      // Range of movement left and right

    private Vector3 startPosition;  // Initial position of the cloud

    void Start()
    {
        // Store the starting position of the cloud
        startPosition = transform.position;
    }

    void Update()
    {
        
        float offsetX = Mathf.Sin(Time.time * speed) * range;
        transform.position = startPosition + new Vector3(offsetX, 0, 0);
    }
}
    


