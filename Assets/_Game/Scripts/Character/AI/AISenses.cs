

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Character.AI
{
    public class AiSenses
    {
        public static bool isMoving = false;

        private readonly TileHub _hub;
        private readonly QueueManager _queueManager;
        private List<KeyValuePair<GameObject, int>> _otherCharacterRange; //Other Characters range in relation to your owns
        private Stats.Character _character;

        public AiSenses(TileHub hub, QueueManager queueManager, Stats.Character character)
        {
            _hub = hub;
            _queueManager = queueManager;
            _character = character;
            UpdateRanges();
        }

        public void UpdateRanges()
        {
            var list = new List<KeyValuePair<GameObject, int>>();
            foreach (var entry in _queueManager.Queue)
            {
                var temp = new KeyValuePair<GameObject, int>(entry.Key, _hub.GetRange(_character.OccupiedTile, entry.Key.GetComponent<Stats.Character>().OccupiedTile));
                list.Add(temp);
            }

            _otherCharacterRange = list;
        }

        public List<KeyValuePair<GameObject, int>> GetRanges()
        {
            var copy = _otherCharacterRange;
            copy.Sort((x, y) => y.Value.CompareTo(x.Value));
            return copy;
        }

        public void Retreat()
        {
            var tempPath = _hub.FindPath(_character.OccupiedTile, _hub.Retreat);
            
            if (tempPath.Count > 3)
            {
                tempPath = tempPath.Take(4).ToList();
            }
            AImoveSlow(_hub, tempPath, _character, 0.5f);
        }
        
        public void Move(Stats.Character target)
        {
 
            var tempPath = _hub.FindPath(_character.OccupiedTile, target.OccupiedTile); 
            AImoveSlow(_hub, tempPath, _character, 0.5f);
             /*
 UpdateRanges();
 */
        }

        public int ApCount()
        {
            return _character.CharStats.CurrentAp;
        }

        public void DecreaseAP()
        {
            _character.CharStats.CurrentAp--;
        }


        public void Next()
        {
            _queueManager.Next();
        }

        public String Name()
        {
            return _character.name;
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
            isMoving = true;
            for (int i = 1; i < path.Count; i++)
            {
                if (Stats.Character.getAPMoveCosts(i, character.CharStats.MChartype) < character.CharStats.CurrentAp)
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

            isMoving = false;
            //character.inMoveProcess = false;
        }
    }
    //Gather Infos
}
