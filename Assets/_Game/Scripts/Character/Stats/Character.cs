﻿using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.GameFlow;
using _Game.Scripts.GameFlow.Grid;
using UnityAtoms;
using UnityEngine;

namespace _Game.Scripts.Character.Stats
{
    public class Character : MonoBehaviour, IResettable
    {
        [SerializeField] private CharacterStat charStats;

        [SerializeField] private GameObject occupiedTile;

        [SerializeField] private VoidEvent updateHUD;

        [SerializeField] private QueueManager _queueManager;

        private TileContainer CTContainer;

        // Start is called before the first frame update
        void Start()
        {
            CharStats.PrimaryWeapon.MotherChar = CharStats;
            CharStats.SecondaryWeapon.MotherChar = CharStats;
            CTContainer = occupiedTile.GetComponent<TileContainer>();
        }

        private void Update()
        {
            if (CTContainer != null)
            {
                if (_queueManager.Queue[_queueManager.ActivePosition].Key == gameObject)
                {
                    CTContainer.State = TileContainer.tileState.SELECTED;
                }
                else
                {
                    if (CTContainer.State != TileContainer.tileState.TARGET && CTContainer.State != TileContainer.tileState.HOVERING)
                    {
                        CTContainer.State = TileContainer.tileState.NORMAL;
                    }
                }
            }
            
        }

        public void ResetOnMatchEnd()
        {
            ResetStats();
        }

        public void ResetStats()
        {
            CharStats.CurrentArmor = CharStats.MaxArmor;
            CharStats.CurrentHealth = CharStats.MaxHealth;
            CharStats.CurrentAp = 0;
            CharStats.CurrentMp = CharStats.MaxMp;
            CharStats.CurrentMs = CharStats.MaxMs;
        }

        public CharacterStat CharStats
        {
            get => charStats;
            set => charStats = value;
        }

        public GameObject OccupiedTile
        {
            get => occupiedTile;
            set
            {
                if (value != null)
                {
                    if (_queueManager.Queue[_queueManager.ActivePosition].Key == gameObject)
                    {
                        occupiedTile.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;
                        occupiedTile = value;
                        occupiedTile.GetComponent<TileContainer>().State = TileContainer.tileState.SELECTED;
                        Debug.Log("Shoudl be good now");
                    }
                    occupiedTile.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;
                    gameObject.transform.position = new Vector3(value.transform.position.x,1f,value.transform.position.z);
                    CTContainer = occupiedTile.GetComponent<TileContainer>();
                }
                else
                {
                    occupiedTile.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;
                    occupiedTile = value;
                    CTContainer = null;
                }
            }
        }

        public bool AImove(TileHub grid, List<TileAttribute> path)
        {
            foreach (var VARIABLE in path)
            {
                Debug.Log(VARIABLE.gridX+"-"+ VARIABLE.gridY);
            }

            ;
            //0 in Path list is the tile the Character is standing on
            //Last one is the Destination, try to go as far as possible
            for (int i = 1; i < path.Count; i++)
            {
                if (getAPMoveCosts(i, charStats.MChartype) < charStats.CurrentAp)
                {
                    //int distance = grid.GetRange(OccupiedTile, path[i - 1].node);
                    //Debug.Log("AI moved distance: " +  distance);
                    
                    CharStats.MoveReduceAp(i - 1);
                    OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = null;
                    OccupiedTile = path[i - 1].node;
                    OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = gameObject;
                    return true;
                }
                else
                {
                    Debug.Log("Cant move hi");
                }
            }
            
            return false;
        }

        public int getAPMoveCosts(int distance, CharacterStat.CharType type)
        {
            switch (type)
            {
                case CharacterStat.CharType.Base: return distance;
                    break;
                case CharacterStat.CharType.Melee: return  distance / 3 + 1;
                    break;
                case CharacterStat.CharType.Support: return distance / 2 + 1;
                    break;
                case CharacterStat.CharType.Tank: return distance;
                    break;
                case CharacterStat.CharType.Sniper: return  distance;
                    break;
                default: return 0;
            }
        }
    }
}
