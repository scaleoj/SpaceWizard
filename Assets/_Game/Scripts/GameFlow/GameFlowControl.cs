using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;

public class GameFlowControl : MonoBehaviour, IAtomListener<int>, IAtomListener<GameObject>
{
    [SerializeField] private QueueManager queue;

    [Tooltip("Array holds the Player and Enemy Units")] 
    [SerializeField] private TileHub grid;
    [SerializeField] private GameObject[] units;
    [SerializeField] private IntEvent hudStateChanged;
    [SerializeField] private State HUDstate;
    [SerializeField] private GameObjectEvent gameobjectChanged;
    //[SerializeField] private GameObjectVariable currentSelectedGameObject;
    //[SerializeField] private GameObject testTile;
    private TileAttribute[] tileAttributes;


    private void Awake()
    {
        hudStateChanged.RegisterListener(this);
        gameobjectChanged.RegisterListener(this);
        /*for (int i = 0; i < units.Length; i++)
        {
            queue.KillUnit(units[i]);            
        }*/
        queue.ActivePosition = 0;
        
        addUnitsToQueue(units); //Remove this when wanting to be able to manually place unity

        HUDstate.SelectedAction = State.currentAction.IDLE;
    }

    public void addUnitsToQueue(GameObject[] unitArr)
    {
        foreach (var t in unitArr)
        {
            queue.SpawnUnit(t);
        }
    }

    public void OnEventRaised(int item)
    {
        //Debug.Log(item);
        ResetTiles(tileAttributes);
        switch (item)
        {
           case 0: break; //IDLE
           case 1:
                //-----MOVE-----
                var currentChar = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>();
                
                tileAttributes = grid.GetTilesInRange(currentChar.OccupiedTile,
                    currentChar.CharStats.MoveRange);

                //Debug.Log("Length: " + tileAttributes.Length);
                for (var i = 0; i < tileAttributes.Length; i++)
                {
                    if (tileAttributes[i].node.GetComponent<TileContainer>().OccupiedGameObject == null)
                    {
                        tileAttributes[i].node.GetComponent<TileContainer>().State = TileContainer.tileState.IN_MOVE_RANGE;
                    }
                    else
                    {
                        tileAttributes[i] = null; //!Disclaimer: Needs to be checked in further methods
                    }
                }             
     
               break;
           case 2: break; //ATK1
           case 3: break; //ATK2
           case 4: break; //WAIT
           default: break;
        }
    }
    
    public void OnEventRaised(GameObject item)
    {
        var currentChar = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>();
        if (item.layer == 9 && item.GetComponent<TileContainer>().State == TileContainer.tileState.IN_MOVE_RANGE)
        {
            item.GetComponent<TileContainer>().OccupiedGameObject = queue.Queue[queue.ActivePosition].Key;
            HUDstate.SelectedAction = State.currentAction.IDLE;
            var distance = grid.GetRange(currentChar.OccupiedTile, item);
            //Debug.Log("Distance moved:" +  distance);
            currentChar.CharStats.moveReduceAp(distance);
            currentChar.OccupiedTile = item;
            ResetTiles(tileAttributes);
        }
        else
        {
            Debug.Log("Cant Move");
        }
    }

    public static void ResetTiles(TileAttribute[] tiles)
    {
        if (tiles == null)
        {
            return;
        }
        
        foreach (var t in tiles)
        {
            if (t != null)
            {
                t.node.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;                
            }
        }

        tiles = null;
    }
}
