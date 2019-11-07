using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    
    private Dictionary<GameObject, int> initSheet; //Sheet for the initiative
    private List<KeyValuePair<GameObject, int>> queue;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        initSheet = new Dictionary<GameObject, int>();
        List<KeyValuePair<GameObject, int>> queue = new List<KeyValuePair<GameObject, int>>();
    }

    public void SpawnUnit(GameObject c)
    {
        var link = c.GetComponent<Character>();
        
        if (!initSheet.ContainsKey(c))
        {
            initSheet.Add(c, link.CharStats.Initiative);
            UpdateList();
        }
        else
        {
            Debug.Log("Character konnte nicht hinzugefügt werden");
        }
    }

    public void KillUnit(GameObject c)
    {
        if (initSheet.ContainsKey(c))
        {
            initSheet.Remove(c);
            UpdateList();
        }
        else
        {
            Debug.LogError("Killed Character is not in Queue");
        }
    }

    private void UpdateList()
    {
        queue = initSheet.ToList();
    }

    private void Sort()
    {
        UpdateList();
        queue.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value)
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
