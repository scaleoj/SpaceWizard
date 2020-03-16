using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float minDeadTime;
    [Range(0f,1f)] [SerializeField] private float flickerProbability;

    private float currentTime;
    private float randNum;
    private Light _light;

    private Material mat;
    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            _light = gameObject.GetComponentInChildren<Light>();
            mat = new Material(GetComponent<MeshRenderer>().material);
            GetComponent<MeshRenderer>().material = mat;
        }
        catch (Exception e)
        {
            Debug.LogError("Couldnt find any Light Component in the Children of <" + gameObject + ">. Full Error :" + e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        randNum = UnityEngine.Random.Range(0f,1f);
        if (randNum <= flickerProbability)
        {
            if (currentTime > minDeadTime)
            {
                _light.enabled = false;
                mat.DisableKeyword("_EMISSION");
                currentTime = 0f;
            }
        }
        else
        {
            if (currentTime > minDeadTime)
            {
                _light.enabled = true;
                mat.EnableKeyword("_EMISSION");
                currentTime = 0f;
            }
        }
    }
}
