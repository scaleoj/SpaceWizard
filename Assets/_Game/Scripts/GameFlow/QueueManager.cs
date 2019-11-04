using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    
    private ArrayList unitQueue;
    
    
    // Start is called before the first frame update
    void Start()
    {
        unitQueue = new ArrayList();
    }

    void Spawn(Character c)
    {
        unitQueue.Add(c);
    }

    void Kill(Character c)
    {
        if (unitQueue.Contains(c))
        {
            unitQueue.Remove(c);
        }
        else
        {
            Debug.LogError("Killed Character is not in Queue");
        }
    }

    void Sort()
    {
        //Sort Implementierung
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
