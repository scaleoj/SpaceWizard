

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
            if (_hub.Retreat.GetComponent<TileContainer>().OccupiedGameObject != null)
            {
                bool foundSuitableTarget = false;
                int count = 2;
                var hypotheticalPath = _hub.FindPathOnlyRange(_character.OccupiedTile, _hub.Retreat);
                while (!foundSuitableTarget)
                {
                    if (hypotheticalPath.Count == count)
                    {
                        //No Suitable Path found, dont move
                        return;
                    }
                    
                    if (hypotheticalPath[hypotheticalPath.Count - count].node.GetComponent<TileContainer>()
                        .OccupiedGameObject == null)
                    {
                        foundSuitableTarget = true;
                        moveTarget = hypotheticalPath[hypotheticalPath.Count - count].node;
                    }
                    count++;
                }
                
                var tempPath = _hub.FindPathOnlyRange(_character.OccupiedTile, moveTarget); 
            
                if (tempPath.Count > 3)
                {
                    tempPath = tempPath.Take(4).ToList();
                }
                _aiHub.startMoveRoutine(_hub, tempPath, _character, 0.5f);
            }
            else
            {
                var tempPath = _hub.FindPathOnlyRange(_character.OccupiedTile, _hub.Retreat); 
            
                if (tempPath.Count > 3)
                {
                    tempPath = tempPath.Take(4).ToList();
                }
                _aiHub.startMoveRoutine(_hub, tempPath, _character, 0.5f);
            }
            /*if (tempPath[tempPath.Count - 1].node.GetComponent<TileContainer>().OccupiedGameObject != null)
            {
                _aiHub.startMoveRoutine(_hub,  tempPath.Take(tempPath.Count - 2).ToList(), _character, 0.5f);
            }
            else
            {
                _aiHub.startMoveRoutine(_hub, tempPath, _character, 0.5f);
            }*/
            
            //StartCoroutine(AImoveSlow(_hub, tempPath, _character, 0.5f));
            //move
        }
        
        public void Move(Stats.Character target)
        {
            GameObject moveTarget = target.OccupiedTile;
            if (target.OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject != null)
            {
                bool foundSuitableTarget = false;
                int count = 2;
                var hypotheticalPath = _hub.FindPathOnlyRange(_character.OccupiedTile, target.OccupiedTile);
                while (!foundSuitableTarget)
                {
                    if (hypotheticalPath.Count == count)
                    {
                        //No Suitable Path found, dont move
                        return;
                    }
                    
                    if (hypotheticalPath[hypotheticalPath.Count - count].node.GetComponent<TileContainer>()
                        .OccupiedGameObject == null)
                    {
                        foundSuitableTarget = true;
                        moveTarget = hypotheticalPath[hypotheticalPath.Count - count].node;
                    }
                    count++;
                }
                
                var tempPath = _hub.FindPathOnlyRange(_character.OccupiedTile, moveTarget);
                _aiHub.startMoveRoutine(_hub, tempPath, _character, 0.5f);
            }
            else
            {
                var tempPath = _hub.FindPathOnlyRange(_character.OccupiedTile, target.OccupiedTile);
                _aiHub.startMoveRoutine(_hub, tempPath, _character, 0.5f);
            }
            
           // var tempPath = _hub.FindPath(_character.OccupiedTile, target.OccupiedTile); 
           // _aiHub.startMoveRoutine(_hub, tempPath.Take(tempPath.Count - 2).ToList(), _character, 0.5f);
            
            //StartCoroutine(AImoveSlow(_hub, tempPath, _character, 0.5f));
            //move

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
    }
    //Gather Infos
}
