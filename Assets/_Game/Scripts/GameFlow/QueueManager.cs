﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.GameFlow
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameFlow/QueueManager")]
    public class QueueManager : ScriptableObject
    {
    
        private Dictionary<GameObject, int> _initSheet; //Sheet for the initiative
        private List<KeyValuePair<GameObject, int>> _queue;
        private int _activePosition = 0;
    
    
        // Start is called before the first frame update
        private void OnEnable()
        {
            _initSheet = new Dictionary<GameObject, int>();
            List<KeyValuePair<GameObject, int>> queue = new List<KeyValuePair<GameObject, int>>();
        }

        public void SpawnUnit(GameObject c)
        {
            var link = c.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
            var link2 = c.GetComponent<_Game.Scripts.Character.AI.AIHub>();
        
            if (!_initSheet.ContainsKey(c))
            {
                _initSheet.Add(c, link.CharStats.Initiative);
                link.CharStats.Active = false;
                UpdateList();
            }
            else
            {
                Debug.Log("Character konnte nicht hinzugefügt werden");
            }
        }

        public void KillUnit(GameObject c)
        {
            if (_initSheet.ContainsKey(c))
            {
                _initSheet.Remove(c);
                UpdateList();
                CheckEnd();
            }
            else
            {
                Debug.LogError("Killed Character is not in Queue");
            }
        }

        public void Next()
        {
            if (_activePosition < _queue.Count - 1)
            {
                var link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                link.CharStats.Active = false;
                ++_activePosition;
                link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                link.CharStats.Active = true;
                var link2 = _queue[_activePosition].Key.GetComponent<_Game.Scripts.Character.AI.AIHub>();
                link2.PositionUpdate();
            }
            else
            {
                var link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                link.CharStats.Active = false;
                _activePosition = 0;
                link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                link.CharStats.Active = true;
                var link2 = _queue[_activePosition].Key.GetComponent<_Game.Scripts.Character.AI.AIHub>();
                link2.PositionUpdate();
            }
        }

        private bool CheckEnd()
        {
            var check = -1;
            foreach (var element in _queue)
            {
                var link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                if (check == -1)
                {
                    check = link.CharStats.Team;
                }
                else
                {
                    if (check != link.CharStats.Team)
                    {
                        return false;
                    }
                }
            
            }

            return true;
        }

        private void UpdateList()
        {
            _queue = _initSheet.ToList();
        }

        private void Sort()
        {
            UpdateList();
            _queue.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value)
            );
        }
    
    
        //---GET,SET---
        public List<KeyValuePair<GameObject, int>> Queue => _queue;
        public Dictionary<GameObject, int> InitSheet => _initSheet;

        public int ActivePosition
        {
            get => _activePosition;
            set => _activePosition = value;
        }
    }
}
