﻿using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Character.Stats;
using UnityAtoms;
using UnityEngine;

namespace _Game.Scripts.GameFlow
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameFlow/QueueManager")]
    public class QueueManager : ScriptableObject
    {
        [SerializeField] private VoidEvent newQueueGameObject;
        [SerializeField] private GameObjectEvent currentQueueObjectChanged;
        [SerializeField] private StringEvent alertBox;
        private Dictionary<GameObject, int> _initSheet; //Sheet for the initiative
        private List<KeyValuePair<GameObject, int>> _queue;
        private int _activePosition = 0;
    
    
        // Start is called before the first frame update
        public void OnEnable()
        {
            _initSheet = new Dictionary<GameObject, int>();
            _queue = new List<KeyValuePair<GameObject, int>>();
            _activePosition = 0;
        }

        public void SpawnUnit(GameObject c)
        {
            var link = c.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
            var link2 = c.GetComponent<_Game.Scripts.Character.AI.AIHub>();
        
            if (!_initSheet.ContainsKey(c))
            {
                //Debug.Log("ADDING GAMEOBJECT----->" + c);
                //Debug.Log("LINK" + link);
                //Debug.Log("CHARSTATS" + link.CharStats);
                //Debug.Log("INITIATIVE" + link.CharStats.Initiative);
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
            //Debug.Log(this._queue[_activePosition].Key);
            //Debug.Log(c);
            if (_initSheet.ContainsKey(c))
            {
                _initSheet.Remove(c);
                UpdateList();
                //CheckEnd();
            }
            else
            {
                Debug.LogError("Killed Character is not in Queue");
            }
        }

        public void Next()
        {
            CharacterStat stat = _queue[_activePosition].Key.GetComponent<Character.Stats.Character>().CharStats;
            stat.CurrentAp += stat.Apgain;
            
           // _queue[_activePosition].Key.GetComponent<Character.Stats.Character>().OccupiedTile
                //.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;

            if (_activePosition < _queue.Count - 1)
            {
                var link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                link.CharStats.Active = false;
                ++_activePosition;
                link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                link.CharStats.Active = true;
                newQueueGameObject.Raise();
                currentQueueObjectChanged.Raise(_queue[_activePosition].Key);

                if (_queue[_activePosition].Key != null)
                {
                    alertBox.Raise(_queue[_activePosition].Key.GetComponent<Character.Stats.Character>().CharStats.CharName + "`s Turn");
                }
                var link2 = _queue[_activePosition].Key.GetComponent<Character.AI.AIHub>();
                if (link2 == null) return;
                link2.PositionUpdate(); 
                link2.BeActive();
            }
            else
            {
                var link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                link.CharStats.Active = false;
                _activePosition = 0;
                link = _queue[_activePosition].Key.GetComponent<global::_Game.Scripts.Character.Stats.Character>();
                link.CharStats.Active = true;
                newQueueGameObject.Raise();
                currentQueueObjectChanged.Raise(_queue[_activePosition].Key);
                if (_queue[_activePosition].Key != null)
                {
                    alertBox.Raise(_queue[_activePosition].Key.GetComponent<Character.Stats.Character>().CharStats.CharName + "`s Turn");
                }
                var link2 = _queue[_activePosition].Key.GetComponent<Character.AI.AIHub>();
                if (link2 == null) return;
                link2.PositionUpdate();
                link2.BeActive();
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
            
            if (_activePosition >= _queue.Count - 1)
            {
                _activePosition = 0;
            }
            
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
