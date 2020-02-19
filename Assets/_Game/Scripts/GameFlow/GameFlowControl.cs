using UnityAtoms;
using UnityEngine;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;
//using UnityEditor.Experimental.GraphView;

public class GameFlowControl : MonoBehaviour, IAtomListener<int>, IAtomListener<GameObject>
{
    [SerializeField] private QueueManager queue;

    [Tooltip("Array holds the Player and Enemy Units")] 
    [SerializeField] private TileHub grid;
    [SerializeField] private GameObject[] units;
    [SerializeField] private IntEvent hudStateChanged;
    [SerializeField] private State HUDstate;
    [SerializeField] private GameObjectEvent gameobjectChanged;
    [SerializeField] private IntVariable selectedAbility;
    [SerializeField] private GameObjectEvent nextGOinQueue;

    [SerializeField] private BoolVariable gameUpdates;
    //[SerializeField] private GameObjectVariable currentSelectedGameObject;
    //[SerializeField] private GameObject testTile;
    private TileAttribute[] tileAttributes;

    private CharContainer[] savedStats;


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
        savedStats = new CharContainer[units.Length];
        
        
        /*for (int i = 0; i < units.Length; i++)
        {
           // savedStats[i] = ScriptableObject.CreateInstance<CharacterStat>();
            CharacterStat stats = units[i].GetComponent<Character>().CharStats;
            //savedStats[i] = Instantiate(units[i].GetComponent<Character>().CharStats);

            savedStats[i].maxArmor = stats.MaxArmor;
             savedStats[i].maxHealth = stats.MaxHealth;
             savedStats[i].maxMp = stats.MaxMp;
             savedStats[i].maxMs = stats.MaxMs;
             
             savedStats[i].initiative = stats.Initiative;
             savedStats[i].currentAP = stats.CurrentAp;
             savedStats[i].currentArmor = stats.CurrentArmor;
             savedStats[i].currentHealth = stats.CurrentHealth;
             savedStats[i].currentMp = stats.CurrentMp;
             savedStats[i].currentMs = stats.CurrentMs;
             savedStats[i].apgain = stats.Apgain;
        }
        */
        
        
        HUDstate.SelectedAction = State.currentAction.IDLE;

        queue.Queue[queue.ActivePosition].Key.GetComponent<Character>().OccupiedTile.GetComponent<TileContainer>().State
            = TileContainer.tileState.SELECTED;
        
        nextGOinQueue.Raise(queue.Queue[queue.ActivePosition].Key);
        
        gameUpdates.Value = true;
    }

    private void OnDestroy()
    {
        gameUpdates.Value = false;
        
        /*for (int i = 0; i < units.Length; i++)
        {
            CharacterStat stats = units[i].GetComponent<Character>().CharStats;

            stats.Apgain = savedStats[i].apgain;
            stats.MaxArmor = savedStats[i].maxArmor;
            stats.MaxHealth = savedStats[i].maxHealth;
            stats.MaxMp = savedStats[i].maxMp;
            stats.MaxMs = savedStats[i].maxMs;
            
            stats.Initiative = savedStats[i].initiative;
            stats.CurrentAp = savedStats[i].currentAP;
            stats.CurrentArmor = savedStats[i].currentArmor;
            stats.CurrentHealth = savedStats[i].currentHealth;
            stats.CurrentMp = savedStats[i].currentMp;
            stats.CurrentMs = savedStats[i].currentMs;
            
        }*/
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
           case 2: //ATK1
               if (selectedAbility.Value == -1) return;
               Debug.Log(selectedAbility.Value);
               
               int remainingAP1 = currentChar.CharStats.CurrentAp -
                                 currentChar.CharStats.PrimaryWeapon.Abilities[selectedAbility.Value].ApCost;

               if (remainingAP1 < 0)
               {
                   return;
               }
               
               tileAttributes =
                   grid.GetTilesInRange(currentChar.OccupiedTile, currentChar.CharStats.PrimaryWeapon.Range);

               for (int i = 0; i < tileAttributes.Length; i++)
               {
                   tileAttributes[i].node.GetComponent<TileContainer>().State = TileContainer.tileState.TARGET;
               }
               
               break; 
           case 3: //ATK2
               if (selectedAbility.Value == -1) return;
               
               int remainingAP2 = currentChar.CharStats.CurrentAp -
                                 currentChar.CharStats.SecondaryWeapon.Abilities[selectedAbility.Value].ApCost;
               
               if (remainingAP2 < 0)
               {
                   return;
               }
               
               
               tileAttributes =
                   grid.GetTilesInRange(currentChar.OccupiedTile, currentChar.CharStats.SecondaryWeapon.Range);

               for (int i = 0; i < tileAttributes.Length; i++)
               {
                   tileAttributes[i].node.GetComponent<TileContainer>().State = TileContainer.tileState.TARGET;
               }
               
               break; 
           case 4: break; //WAIT
           default: break;
        }
    }
    
    // Logic for reacting to Targetselection after UI input, basicly moving and attakcing
    public void OnEventRaised(GameObject item)
    {
        if (tileAttributes != null)
        {
            if (HUDstate.SelectedAction == State.currentAction.MOVE)
            {
                moveChar(item);
            }
            else if (HUDstate.SelectedAction == State.currentAction.ATTACK1)
            {
                attackTargetOne(item);
            }
            else if (HUDstate.SelectedAction == State.currentAction.ATTACK2)
            {
                attackTargetTwo(item);
            }
            else
            {
                //Debug.Log("Nothing to do...");
            }
        }
    }

    public void attackTargetOne(GameObject item)
    {
        Character currentChar = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>();
        
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
        int remainingAP = currentChar.CharStats.CurrentAp -
            currentChar.CharStats.PrimaryWeapon.Abilities[selectedAbility.Value].ApCost;


        if (item.layer == 9 && tileInRange && remainingAP >= 0 )
        {
            currentChar.CharStats.PrimaryWeapon.Abilities[selectedAbility.Value].Attack(item.GetComponent<TileContainer>().OccupiedGameObject, grid.GetRange(currentChar.OccupiedTile, item));
            currentChar.CharStats.CurrentAp -=
                currentChar.CharStats.PrimaryWeapon.Abilities[selectedAbility.Value].ApCost;
            ResetTiles(tileAttributes);
        }
    }

    public void attackTargetTwo(GameObject item)
    {
        Character currentChar = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>();
        
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
        
        int remainingAP = currentChar.CharStats.CurrentAp -
                          currentChar.CharStats.SecondaryWeapon.Abilities[selectedAbility.Value].ApCost;

        if (item.layer == 9 && tileInRange && remainingAP >= 0)
        {
            currentChar.CharStats.SecondaryWeapon.Abilities[selectedAbility.Value].Attack(item.GetComponent<TileContainer>().OccupiedGameObject, grid.GetRange(currentChar.OccupiedTile, item));
            currentChar.CharStats.CurrentAp -=
                currentChar.CharStats.SecondaryWeapon.Abilities[selectedAbility.Value].ApCost;
            ResetTiles(tileAttributes);
        }
    }

    public void moveChar(GameObject item)
    {
        Character currentChar = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>();
        
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
                TileContainer.tileState state = t.node.GetComponent<TileContainer>().State;
                if (state == TileContainer.tileState.SELECTED || state == TileContainer.tileState.HOVERING)
                {
                    continue;
                }
                t.node.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;                
            }
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = null;
        }
    }
}
