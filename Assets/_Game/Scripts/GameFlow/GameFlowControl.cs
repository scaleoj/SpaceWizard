using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowControl : MonoBehaviour
{
    [SerializeField] private QueueManager queue;
    [Tooltip("Array holds the Player and Enemy Units")]
    [SerializeField] private GameObject[] units;

    void Awake()
    {
        for (int i = 0; i < units.Length; i++)
        {
            queue.KillUnit(units[i]);            
        }
        queue.ActivePosition = 0;
        
        addUnitsToQueue(units); //Remove this when wanting to be able to manually place units
    }

    public void addUnitsToQueue(GameObject[] unitArr)
    {
        for (int i = 0; i < unitArr.Length; i++)
        {
            queue.SpawnUnit(unitArr[i]);            
        }
    }
}
