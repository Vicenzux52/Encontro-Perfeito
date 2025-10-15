using System;
using UnityEngine;

public class CalculateVelocity : MonoBehaviour
{
    float passedTime = 0;
    float initialZ = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;
        if (passedTime >= 10)
        {
            passedTime = 0f;
            Debug.Log("10s: " + (transform.position.z - initialZ));
            initialZ = transform.position.z;
        }
    }
}
