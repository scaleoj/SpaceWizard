using System;
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

    private CharacterStat[] savedStats;


    private void Awake()
    {
        hudStateChanged.RegisterListener(this);
        gameobjectChanged.RegisterListener(this);
        /*for (int i = 0; i < units.Length; i++)
        {
            queue.KillUnit(units[i]);            
        }*/
        queue.ActivePosition = 0;
        
        addUnitsToQueue(units); //Remove this when wanting to be able to manually place in unity
        savedStats = new CharacterStat[units.Length];
        for (int i = 0; i < units.Length; i++)
        {
            savedStats[i] = new CharacterStat();
            CharacterStat stats = units[i].GetComponent<Character>().CharStats;
            Debug.Log(savedStats[i]);
            savedStats[i].Initiative = stats.Initiative;
            savedStats[i].CurrentAp = stats.CurrentAp;
            savedStats[i].CurrentArmor = stats.CurrentArmor;
            savedStats[i].CurrentHealth = stats.CurrentHealth;
            savedStats[i].CurrentMp = stats.CurrentMp;
            savedStats[i].CurrentMs = stats.CurrentMs;

            savedStats[i].MaxArmor = stats.MaxArmor;
            savedStats[i].MaxHealth = stats.MaxHealth;
            savedStats[i].MaxMp = stats.MaxMp;
            savedStats[i].MaxMs = stats.MaxMs;
        }

        HUDstate.SelectedAction = State.currentAction.IDLE;
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < units.Length; i++)
        {
            CharacterStat stats = units[i].GetComponent<Character>().CharStats;
            stats.Initiative = savedStats[i].Initiative;
            stats.CurrentAp = savedStats[i].CurrentAp;
            stats.CurrentArmor = savedStats[i].CurrentArmor;
            stats.CurrentHealth = savedStats[i].CurrentHealth;
            stats.CurrentMp = savedStats[i].CurrentMp;
            stats.CurrentMs = savedStats[i].CurrentMs;

            stats.MaxArmor = savedStats[i].MaxArmor;
            stats.MaxHealth = savedStats[i].MaxHealth;
            stats.MaxMp = savedStats[i].MaxMp;
            stats.MaxMs = savedStats[i].MaxMs;
        }
    }

    public void addUnitsToQueue(GameObject[] unitArr)
    {
        foreach (GameObject t in unitArr)
        {
            queue.SpawnUnit(t);
            
        }
    }

    public void OnEventRaised(int item)
    {
        //Debug.Log(item);
        ResetTiles(tileAttributes);
        Character currentChar = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>();

        if (currentChar.CharStats.CurrentAp == 0)
        {
            return;
        }
        
        switch (item)
        {
           case 0: break; //IDLE
           case 1:
                //-----MOVE HIGHLIGHT-----
               
                
                tileAttributes = grid.GetTilesInRange(currentChar.OccupiedTile,
                    currentChar.CharStats.MoveRange);

                //Debug.Log("Length: " + tileAttributes.Length);
                for (int i = 0; i < tileAttributes.Length; i++)
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
    
    //Moving logic
    public void OnEventRaised(GameObject item)
    {
        if (tileAttributes == null)
        {
            return;
        }
        
        Character currentChar = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>();
        //Debug.Log("ItemLayer: " + item.layer + ",TileState: " + item.GetComponent<TileContainer>().State);
        bool tileInRange = false;
        for (int i = 0; i < tileAttributes.Length; i++)
        {
            if (tileAttributes[i] == null)
            {
                continue;
            }
            
            if (item == tileAttributes[i].node)
            {
                tileInRange = true;
            }
        }
        
        if (item.layer == 9 && tileInRange)
        {
            item.GetComponent<TileContainer>().OccupiedGameObject = queue.Queue[queue.ActivePosition].Key;
            HUDstate.SelectedAction = State.currentAction.IDLE;
            int distance = grid.GetRange(currentChar.OccupiedTile, item);
            //Debug.Log("Distance moved:" +  distance);
            currentChar.CharStats.MoveReduceAp(distance);
            currentChar.OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = null;
            currentChar.OccupiedTile = item;
            currentChar.OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = currentChar.gameObject;
            ResetTiles(tileAttributes);
            tileInRange = false;
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
        
        foreach (TileAttribute t in tiles)
        {
            if (t != null)
            {
                t.node.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;                
            }
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = null;
        }
    }
}
