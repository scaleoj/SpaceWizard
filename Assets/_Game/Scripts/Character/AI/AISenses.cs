

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

        private readonly AIHub _aiHub;
        private readonly TileHub _hub;
        private readonly QueueManager _queueManager;
        private List<KeyValuePair<GameObject, int>> _otherCharacterRange; //Other Characters range in relation to your owns
        private Stats.Character _character;

        public AiSenses(TileHub hub, QueueManager queueManager, Stats.Character character, AIHub aiHub)
        {
            _aiHub = aiHub;
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
            GameObject moveTarget = _hub.Retreat;
            List<TileAttribute> hypotheticalPath = _hub.FindPathOnlyRange(_character.OccupiedTile, moveTarget);
            hypotheticalPath = hypotheticalPath.Take(_character.CharStats.CurrentAp).ToList();
            
            if (hypotheticalPath.Count > 3)
            {
                hypotheticalPath = hypotheticalPath.Take(4).ToList();
            }

            if (moveTarget.GetComponent<TileContainer>().OccupiedGameObject != null)
            {
                bool foundSuitableTarget = false;
                int count = 1;
                
                while (!foundSuitableTarget)
                { 
                    if (hypotheticalPath.Count == count)
                    {
                        //No Suitable Path found, dont move
                        //Debug.Log("No Suitable Path found!");
                        return;
                    }

                    if (hypotheticalPath[hypotheticalPath.Count - count].node.GetComponent<TileContainer>()
                        .OccupiedGameObject != null)
                    {
                        //Debug.Log("Unsuitable Target!");
                        count++;
                    }
                    else
                    {
                        //Debug.Log("Path found!");
                        foundSuitableTarget = true;
                        hypotheticalPath = hypotheticalPath.Take(hypotheticalPath.Count + 2 - count).ToList();
                    }
                }
                // var tempPath = _hub.FindPathOnlyRange(_character.OccupiedTile, moveTarget); 
               _aiHub.startMoveRoutine(_hub, hypotheticalPath, _character, 0.5f);
            }
            else
            {
                _aiHub.startMoveRoutine(_hub, hypotheticalPath, _character, 0.5f);
            }
        }
        
        public void Move(Stats.Character target)
        {
            GameObject moveTarget = target.OccupiedTile;
            List<TileAttribute> hypotheticalPath = _hub.FindPathOnlyRange(_character.OccupiedTile, moveTarget);
            hypotheticalPath = hypotheticalPath.Take(_character.CharStats.CurrentAp).ToList();
            
            if (moveTarget.GetComponent<TileContainer>().OccupiedGameObject != null) //Target is occupied by a character
            {
                bool foundSuitableTarget = false;
                int count = 2;
                
                while (!foundSuitableTarget)
                { 
                    if (hypotheticalPath.Count == count)
                    {
                        //No Suitable Path found, dont move
                        return;
                    }

                    if (hypotheticalPath[hypotheticalPath.Count - count].node.GetComponent<TileContainer>()
                            .OccupiedGameObject != null)
                    {
                        count++;
                    }
                    else
                    {
                        foundSuitableTarget = true;
                        hypotheticalPath = hypotheticalPath.Take(hypotheticalPath.Count + 2 - count).ToList();
                    }
                }
                //List<TileAttribute> tempPath = _hub.FindPathOnlyRange(_character.OccupiedTile, moveTarget);
                _aiHub.startMoveRoutine(_hub, hypotheticalPath, _character, 0.5f);
            }
            else
            {
                _aiHub.startMoveRoutine(_hub, hypotheticalPath, _character, 0.5f);
            }
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
    }
    //Gather Infos
}
