using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float lerpSpeedMultiplier;
    [Range(0f,1f)][SerializeField] private float frequency;
    [SerializeField] private float minDeadTime;
    [SerializeField] private BoolVariable isMoving;
    [Tooltip("Delay in seconds after it switched isMoving to true.")][SerializeField] private float startDelay;
    private bool waitForDelay = false;
    private float currTime;
    
    // Update is called once per frame
    void Update()
    {
        if (waitForDelay)
        {
            currTime += Time.deltaTime;
            if (currTime >= startDelay)
            {
                waitForDelay = false;
                currTime = 0f;
            }
            else
            {
                return;
            }
        }
        
        if (isMoving.Value)
        {
            currTime += Time.deltaTime * lerpSpeedMultiplier;
            transform.position = Vector3.Lerp(startPosition, endPosition, currTime);
            if (currTime >= 1f)
            {
                transform.position = startPosition;
                isMoving.Value = false;
                currTime = 0f;
            }
        }
        else
        {
            currTime += Time.deltaTime;
            if (currTime >= minDeadTime) 
            {
                float randNum = UnityEngine.Random.Range(0f,1f);
        
                if (randNum <= frequency)
                {
                    isMoving.Value = true;
                    waitForDelay = true;
                    currTime = 0f;
                } 
            }
        }
    }
}
