using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private float lerpSpeedMultiplier;
    [Range(0f,1f)][SerializeField] private float frequency;
    [SerializeField] private float minDeadTime;
    [SerializeField] private BoolVariable trainIsMoving;
    private bool isMoving;
    private float currTime;
    
    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            currTime += Time.deltaTime * lerpSpeedMultiplier;
            transform.position = Vector3.Lerp(startPosition, endPosition, currTime);
            if (currTime >= 1f)
            {
                transform.position = startPosition;
                isMoving = false;
                currTime = 0f;
            }
        }
        else
        {
            currTime += Time.deltaTime;
            if (currTime >= minDeadTime) 
            {
                float randNum = UnityEngine.Random.Range(0f,1f);
        
                if (randNum <= frequency && !trainIsMoving.Value)
                {
                    isMoving = true;
                    currTime = 0f;
                } 
            }
        }
    }
}
