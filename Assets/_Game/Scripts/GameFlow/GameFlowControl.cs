using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using _Game.Scripts.GameFlow.Grid;

public class GameFlowControl : MonoBehaviour, IAtomListener<int>
{
    [SerializeField] private QueueManager queue;

    [Tooltip("Array holds the Player and Enemy Units")] 
    [SerializeField] private TileHub grid;
    [SerializeField] private GameObject[] units;
    [SerializeField] private IntEvent hudStateChanged;
    [SerializeField] private GameObject testTile;
    

    void Awake()
    {
        hudStateChanged.RegisterListener(this);
        /*for (int i = 0; i < units.Length; i++)
        {
            queue.KillUnit(units[i]);            
        }*/
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

    public void OnEventRaised(int item)
    {
        Debug.Log(item);
        switch (item)
        {
           case 0: break; //IDLE
           case 1:
                //-----MOVE-----
                Character currentChar = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>();
                TileAttribute[] tileAttributes;
               
                tileAttributes = grid.GetTilesInRange(currentChar.OccupiedTile,
                    currentChar.CharStats.MoveRange);

                for (int i = 0; i < tileAttributes.Length; i++)
                {
                    tileAttributes[i].node.GetComponent<TileContainer>().State = TileContainer.tileState.IN_MOVE_RANGE;
                }             
     
               break;
           case 2: break; //ATK1
           case 3: break; //ATK2
           case 4: break; //WAIT
           default: break;
        }
    }
}
