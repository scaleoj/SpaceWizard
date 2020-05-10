using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;
using UnityAtoms;
using UnityEngine;

public class HoverControl : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent hoverGOChanged;
    [SerializeField] private GameObjectVariable clickedGameObject;
    [SerializeField] private PlayerInputProvider m_input;
    [SerializeField] private BoolVariable mouseOverUI;
    [SerializeField] private State currentHudState;
    [SerializeField] private TileHub grid;
    [SerializeField] private QueueManager _queueManager;
    [SerializeField] private IntEvent current_Move_Ap_Cost;

    private GameObject lastHovered;

    private bool wasClicked;
    
    private TileContainer.tileState saveTileState;

    private TileContainer.tileState[] savedPathTileStates;
    
    //private List<TileAttribute> savedPathTiles;

    private TileAttribute[] currentPathTiles;

    //private bool first = true;


    void Start()
    {
        hoverGOChanged.RegisterListener(this);
        saveTileState = TileContainer.tileState.NORMAL;
        lastHovered = null;
    }

    private void OnDestroy()
    {
        hoverGOChanged.UnregisterListener(this);
    }

    void Update()
    {
        if (!mouseOverUI.Value)
        {
            if (!wasClicked)
            {
                wasClicked = m_input.mouse0Down();

               /* Debug.Log("Wasnt clicked");
                Debug.Log(clickedGameObject.Value);
                Debug.Log(clickedGameObject.Value.GetComponent<TileContainer>());
                if (clickedGameObject.Value != null && clickedGameObject.Value.GetComponent<TileContainer>() == null) 
                { 
                    Debug.Log("ELLOM8");
                    return;
                }
                else
                {
                    wasClicked = m_input.mouse0Down();
                }*/
            }   
        }
    }

    public void OnEventRaised(GameObject item)
    {
        if (wasClicked && item == null)
        {
            wasClicked = false;
        }
        
        if (item != null && item.layer == 9)
        {
            TileContainer itemTileContainer = item.GetComponent<TileContainer>(); //Optimization
           if (itemTileContainer.State != TileContainer.tileState.SELECTED)
           {
               if (lastHovered != null)
               {
                   if (wasClicked)
                   {
                       lastHovered.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;
                       wasClicked = false;
                       //Debug.Log("Switch was clicked to false");
                   }
                   else
                   {
                       lastHovered.GetComponent<TileContainer>().State = saveTileState;
                   }
               }

               if (itemTileContainer.State == TileContainer.tileState.HIGHLIGHTED && savedPathTileStates != null /*&& savedPathTileStates.Length > 1*/ && currentHudState.SelectedAction == State.currentAction.MOVE) //When u move from the hovered tile to the last tile in the highlighted path to prevent it saving "HIGHLIGHTED" as the tile state
               {
                   saveTileState = savedPathTileStates[savedPathTileStates.Length - 1];
                   lastHovered = item;
                   itemTileContainer.State = TileContainer.tileState.HOVERING;
               }
               /*else if(itemTileContainer.State == TileContainer.tileState.HIGHLIGHTED && savedPathTileStates != null && savedPathTileStates.Length == 0 && currentHudState.SelectedAction == State.currentAction.MOVE)
               {
                   
               }*/
               else if (itemTileContainer.State == TileContainer.tileState.HIGHLIGHTED && currentHudState.SelectedAction == State.currentAction.IDLE) //Else it bugs when moving cursor from the queue to the field directly over a highlighted tile
               {
                   saveTileState = TileContainer.tileState.NORMAL;
                   lastHovered = item;
                   itemTileContainer.State = TileContainer.tileState.HOVERING;
               }
               else
               {
                   saveTileState = itemTileContainer.State;
                   lastHovered = item;
                   itemTileContainer.State = TileContainer.tileState.HOVERING;
               }
           }
           else
           {
               if (lastHovered != null)
               {
                   lastHovered.GetComponent<TileContainer>().State = saveTileState;
               }
           }
       }
       else
       {
           if (lastHovered != null)
           {
               lastHovered.GetComponent<TileContainer>().State = saveTileState;
               lastHovered = null;
           }
       }

        /*Highlight of pathtiles*/
        
        if (lastHovered != null && (item.GetComponent<TileContainer>().OccupiedGameObject == null) && item != null) //Lasthovered is the currenthovered Tile here. Its set when a viable tile is hovered. Otherwise its null
        {
            switch (currentHudState.SelectedAction) 
            {
                case State.currentAction.MOVE:
                    
                    //Remove old path
                    if (currentPathTiles != null && savedPathTileStates != null)
                    {
                        for (int i = 0; i < currentPathTiles.Length - 1; i++)
                        {
                            currentPathTiles[i].node.GetComponent<TileContainer>().State = savedPathTileStates[i];
                        }
                    } 
                    
                    // Array Init / Calc new path
                    currentPathTiles = grid.FindPath(
                            _queueManager.Queue[_queueManager.ActivePosition].Key.GetComponent<Character>()
                                .OccupiedTile, item).ToArray();
                    
                    //Raise Int Event so Path costs are shown on the UI
                    int moveCosts = Character.getAPMoveCosts(currentPathTiles.Length,
                        _queueManager.Queue[_queueManager.ActivePosition].Key.GetComponent<Character>().CharStats
                            .MChartype);
                    current_Move_Ap_Cost.Raise(moveCosts);


                    //Remove Final Tile which is the hovered tile/ set it null
                    if (currentPathTiles.Length > 0)
                    {
                        currentPathTiles[currentPathTiles.Length - 1] = null;
                        //Init SavedPath Tiles
                        savedPathTileStates = new TileContainer.tileState[currentPathTiles.Length - 1];
                    }

                    for (int i = 0; i < currentPathTiles.Length - 1; i++)
                    {
                        //Save state
                        savedPathTileStates[i] = currentPathTiles[i].node.GetComponent<TileContainer>().State;
                        //Highlight current path
                        currentPathTiles[i].node.GetComponent<TileContainer>().State =
                            TileContainer.tileState.HIGHLIGHTED;
                    }
                    
                    break;
            }
        }
        else //Reset Old Pathtiles when hovered object isnt a legit tile, or a click was entered
        {
            if (currentHudState.SelectedAction == State.currentAction.MOVE)
                if (currentPathTiles != null && currentPathTiles.Length > 0 && savedPathTileStates != null)
                { 
                    for (int i = 0; i < currentPathTiles.Length - 1; i++)
                    {
                        currentPathTiles[i].node.GetComponent<TileContainer>().State = savedPathTileStates[i];
                    }
                }
            savedPathTileStates = null;
            current_Move_Ap_Cost.Raise(-1);
        }
        
        if (lastHovered != null && wasClicked && lastHovered.GetComponent<TileContainer>().State == TileContainer.tileState.IN_MOVE_RANGE && currentHudState.SelectedAction == State.currentAction.MOVE)
        {
            if (currentHudState.SelectedAction == State.currentAction.MOVE)
                if (currentPathTiles != null && currentPathTiles.Length > 0 && savedPathTileStates != null)
                { 
                    for (int i = 0; i < currentPathTiles.Length - 1; i++)
                    {
                        currentPathTiles[i].node.GetComponent<TileContainer>().State = savedPathTileStates[i];
                    }
                }
            savedPathTileStates = null;
            current_Move_Ap_Cost.Raise(-1);
        }
    }
}
