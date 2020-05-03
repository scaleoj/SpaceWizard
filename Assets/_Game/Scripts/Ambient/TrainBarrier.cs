using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class TrainBarrier : MonoBehaviour, IAtomListener<bool>
{
    [SerializeField] private BoolEvent trainIsMovingChanged;
    [SerializeField] private float lerpMultiplier;
    private float currTime;
    private bool barrierIsUp = true;

    private void Awake()
    {
        trainIsMovingChanged.RegisterListener(this);
    }

    public void OnEventRaised(bool item)
    {
        StartCoroutine(moveBarrier(item));
    }

    private IEnumerator moveBarrier(bool trainIsMoving)
    {
        currTime = 0f;
        if (trainIsMoving)
        {
            yield return new WaitForSeconds(4f);
            float currOffset = -90f;
            while (currOffset <= 0f)
            {
                currTime += Time.deltaTime * lerpMultiplier;
                currOffset += currTime;
                transform.Rotate(new Vector3(-currTime,0f,0f),Space.World);
                yield return null;
            }
        }
        else
        {
            float currOffset = 0f;
            while (currOffset <= 90f)
            {
                currTime += Time.deltaTime * lerpMultiplier;
                currOffset += currTime;
                transform.Rotate(new Vector3(currTime,0f,0f),Space.World);
                yield return null;
            }
        }
    }
}
