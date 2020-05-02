using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Character.AI
{
    public class AIHub : MonoBehaviour
    {
        private AiState _state;

        public enum AiState
        {
            Awake,
            Asleep
        }

        public AiState _State
        {
            get => _state;
            set => _state = value;
        }


        
        private AiBrain _brain;
        private AiSenses _senses;
        private Stats.Character _character;

        [SerializeField] private TileHub hub;
        [SerializeField] private QueueManager queueManager;
        
        
        // Start is called before the first frame update
        private void Start()
        {
            _state = AiState.Asleep;
            
            
            _character = GetComponent<global::_Game.Scripts.Character.Stats.Character>();
            if (_character.CharStats.Team == 0) return;
            while (hub == null)
            {   
                Debug.Log(gameObject.name + ": No TileHub found!");
            }
            _senses = new AiSenses(hub, queueManager, _character, this);
            _brain = new AiBrain(_senses, hub, _character);
        }
        
        public void PositionUpdate()
        {
            _senses.UpdateRanges();
        }
        
        public void BeActive()
        {
            Coroutine blub = StartCoroutine(_brain.MakeDecision());
            //StopCoroutine(blub);
        }
        
        public void startMoveRoutine(TileHub grid, List<TileAttribute> path, Stats.Character character, float moveTime)
        {
            StartCoroutine(AImoveSlow(grid, path, character, moveTime));
        }
        
        public static IEnumerator AImoveSlow(TileHub grid, List<TileAttribute> path, Stats.Character character, float moveTime)
        {
            //0 in Path list is the tile the Character is standing on
            //Last one is the Destination, try to go as far as possible
            
            //NOTE: FULL AP COST OF THE MOVE IS CALCULATED AT THE END

            /* int moveableDistance = 0;
             
             for (int i = path.Count; 0 < i; i -= Stats.Character.getAPMoveCosts(1, character.CharStats.MChartype))
             {
                 moveableDistance++;
             }
 
             float timePerMove = moveTime / moveableDistance;*/
            
            //character.inMoveProcess = true;
            
            AiSenses.isMoving = true;
            for (int i = 1; i < path.Count; i++)
            {
                if (Stats.Character.getAPMoveCosts(1, character.CharStats.MChartype) <= character.CharStats.CurrentAp)
                {
                    //int distance = grid.GetRange(OccupiedTile, path[i - 1].node);
                    //Debug.Log("AI moved distance: " +  distance);
                    
                    character.CharStats.MoveReduceAp(1);
                    character.OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = null;
                    character.OccupiedTile = path[i - 1].node;
                    character.OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = character.gameObject;
                    yield return new WaitForSeconds(moveTime);
                }
                else
                {
                    break;
                }
            }
            AiSenses.isMoving = false;
            //character.inMoveProcess = false;
        }
    }
}
