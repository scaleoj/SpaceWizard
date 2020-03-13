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
    // Start is called before the first frame update
    void Awake()
    {
        _light = gameObject.GetComponent<Light>();
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
                currentTime = 0f;
            }
        }
        else
        {
            if (currentTime > minDeadTime)
            {
                _light.enabled = true;
                currentTime = 0f;
            }
        }
    }
}
