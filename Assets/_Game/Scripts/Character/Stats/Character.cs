using System;
using System.Collections;
using System.Collections.Generic;
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

        [Tooltip("Defines the height the character will be placed at when repositioning.")][SerializeField] private float yPosOffset;

        private TileContainer CTContainer;

        private CharacterStat charStatCopy;

        private bool inMoveProcess;
        
        
        // Configures and copies SO`s
        void Awake()
        {
            charStatCopy = Instantiate(charStats);

            //Create Copies
            Weapon primWeapon = Instantiate(charStats.PrimaryWeapon);
            Weapon secondWeapon = Instantiate(charStats.SecondaryWeapon);

            Ability[] weapOneAbilites = new Ability[charStats.PrimaryWeapon.Abilities.Length];
            Ability[] weapTwoAbilities = new Ability[charStats.SecondaryWeapon.Abilities.Length];

            for (int i = 0; i < charStats.PrimaryWeapon.Abilities.Length; i++)
            {
                weapOneAbilites[i] = Instantiate(charStats.PrimaryWeapon.Abilities[i]);
            }
            
            for (int i = 0; i < charStats.SecondaryWeapon.Abilities.Length; i++)
            {
                weapTwoAbilities[i] = Instantiate(charStats.SecondaryWeapon.Abilities[i]);
            }
            
            //Link Character -> Weapons
            charStatCopy.PrimaryWeapon = primWeapon;
            charStatCopy.SecondaryWeapon = secondWeapon;

            //Link Weapons -> Abilities
            charStatCopy.PrimaryWeapon.Abilities = weapOneAbilites;
            charStatCopy.SecondaryWeapon.Abilities = weapTwoAbilities;

            //Link Character <- Weapons
            charStatCopy.PrimaryWeapon.ParentChar = charStatCopy;
            charStatCopy.SecondaryWeapon.ParentChar = charStatCopy;

            //Link Weapons <- Abilties
            for (int i = 0; i < charStats.PrimaryWeapon.Abilities.Length; i++)
            {
                charStatCopy.PrimaryWeapon.Abilities[i].ParentWeapon = charStatCopy.PrimaryWeapon;
            }
            
            for (int i = 0; i < charStats.SecondaryWeapon.Abilities.Length; i++)
            {
                charStatCopy.SecondaryWeapon.Abilities[i].ParentWeapon = charStatCopy.SecondaryWeapon;
            }

            CTContainer = occupiedTile.GetComponent<TileContainer>();
        }

        private void Update()
        {
            if (CTContainer != null) //Optimize maybe
            {
                if (_queueManager.Queue[_queueManager.ActivePosition].Key == gameObject)
                {
                    if (CTContainer.State != TileContainer.tileState.HIGHLIGHTED)
                    {
                        CTContainer.State = TileContainer.tileState.SELECTED;
                    }
                }
                else
                {
                    if (CTContainer.State != TileContainer.tileState.TARGET && CTContainer.State != TileContainer.tileState.HOVERING && CTContainer.State != TileContainer.tileState.HIGHLIGHTED)
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

        public CharacterStat OriginalCharStats
        {
            get => charStats;
            set => charStats = value;
        }

        //Reminder: Ugly might need to change
        /*Returns the Charstat Copy. For changing the original one use OriginalCharStats().*/
        public CharacterStat CharStats
        {
            get => charStatCopy;
            set => charStatCopy = value;
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
                    }
                    occupiedTile.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;
                    gameObject.transform.position = new Vector3(value.transform.position.x,yPosOffset,value.transform.position.z);
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

        /*public void AImove(TileHub grid, List<TileAttribute> path)
        {
            //0 in Path list is the tile the Character is standing on
            //Last one is the Destination, try to go as far as possible
            
            pathfinished = false;
            for (int i = 1; i < path.Count; i++)
            {
                if (getAPMoveCosts(i, charStats.MChartype) < charStats.CurrentAp)
                {
                    //int distance = grid.GetRange(OccupiedTile, path[i - 1].node);
                    //Debug.Log("AI moved distance: " +  distance);
                    
                    CharStats.MoveReduceAp(1);
                    OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = null;
                    OccupiedTile = path[i - 1].node;
                    OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = gameObject;
                }
                else
                {
                    Debug.Log("Cant move hi");
                    return false;
                }
            }
            return true;
           StartCoroutine(AImoveSlow(grid, path, this, ));
        }*/

      /**  public static IEnumerator AImoveSlow(TileHub grid, List<TileAttribute> path, Character character, float moveTime)
        {
            //0 in Path list is the tile the Character is standing on
            //Last one is the Destination, try to go as far as possible
            
            //NOTE: FULL AP COST OF THE MOVE IS CALCULATED AT THE END

            int moveableDistance = 0;
            
            for (int i = path.Count; 0 < i; i -= getAPMoveCosts(1, character.charStats.MChartype))
            {
                moveableDistance++;
            }

            float timePerMove = moveTime / moveableDistance;
            
            character.inMoveProcess = true;
            for (int i = 1; i < path.Count; i++)
            {
                if (getAPMoveCosts(i, character.charStats.MChartype) < character.charStats.CurrentAp)
                {
                    //int distance = grid.GetRange(OccupiedTile, path[i - 1].node);
                    //Debug.Log("AI moved distance: " +  distance);
                    
                    character.CharStats.MoveReduceAp(1);
                    character.OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = null;
                    character.OccupiedTile = path[i - 1].node;
                    character.OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = character.gameObject;
                    yield return new WaitForSeconds(timePerMove);
                }
                else
                {
                    break;
                }
            }
            character.inMoveProcess = false;
        }*/

        public static int getAPMoveCosts(int distance, CharacterStat.CharType type)
        {
            switch (type)
            {
                case CharacterStat.CharType.Base: return distance;
                case CharacterStat.CharType.Melee: return  distance / 3 + 1;
                case CharacterStat.CharType.Support: return distance / 2 + 1;
                case CharacterStat.CharType.Tank: return distance;
                case CharacterStat.CharType.Sniper: return  distance;
                default: return 0;
            }
        }

        public bool InMoveProcess
        {
            get => inMoveProcess;
            set => inMoveProcess = value;
        }
    }
}
