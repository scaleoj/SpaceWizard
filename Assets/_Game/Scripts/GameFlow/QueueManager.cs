using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/GameFlow/QueueManager")]
public class QueueManager : ScriptableObject
{
    
    private Dictionary<GameObject, int> initSheet; //Sheet for the initiative
    private List<KeyValuePair<GameObject, int>> queue;
    private int activePosition = 0;
    
    
    // Start is called before the first frame update
    private void OnEnable()
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
            link.CharStats.Active = false;
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
            checkEnd();
        }
        else
        {
            Debug.LogError("Killed Character is not in Queue");
        }
    }

    public void Next()
    {
        if (activePosition < queue.Count - 1)
        {
            var link = queue[activePosition].Key.GetComponent<Character>();
            link.CharStats.Active = false;
            ++activePosition;
            link = queue[activePosition].Key.GetComponent<Character>();
            link.CharStats.Active = true;
        }
        else
        {
            var link = queue[activePosition].Key.GetComponent<Character>();
            link.CharStats.Active = false;
            activePosition = 0;
            link = queue[activePosition].Key.GetComponent<Character>();
            link.CharStats.Active = true;
        }
    }

    private bool checkEnd()
    {
        var check = -1;
        foreach (var element in queue)
        {
            var link = queue[activePosition].Key.GetComponent<Character>();
            if (check == -1)
            {
                check = link.CharStats.Team;
            }
            else
            {
                if (check != link.CharStats.Team)
                {
                    return false;
                }
            }
            
        }

        return true;
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
    
    
    //---GET,SET---
    public List<KeyValuePair<GameObject, int>> Queue => queue;
    public Dictionary<GameObject, int> InitSheet => initSheet;

    public int ActivePosition => activePosition;
}
