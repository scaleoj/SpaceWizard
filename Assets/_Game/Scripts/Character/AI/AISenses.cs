﻿

using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Character.AI
{
    public class AiSenses
    {

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

        public void MoveToOptimalPosition()
        {
            //git gud
        }
        
        public void Move(GameObject end)
        {
            _character.OccupiedTile = end;
            UpdateRanges();
        }
        
        
    }
    
    //Gather Infos
}
