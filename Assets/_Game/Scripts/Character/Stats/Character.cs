using System.Collections.Generic;
using System.Linq;
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

        // Start is called before the first frame update
        void Start()
        {
            CharStats.PrimaryWeapon.MotherChar = CharStats;
            CharStats.SecondaryWeapon.MotherChar = CharStats;
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
                    gameObject.transform.position = new Vector3(value.transform.position.x,1f,value.transform.position.z);
                }
                occupiedTile = value;
            }
        }

        public bool AImove(TileHub grid, List<TileAttribute> path)
        {
            //0 in Path list is the tile the Character is standing on
            //Last one is the Destination, try to go as far as possible
            for (int i = 1; i < path.Count; i++)
            {
                if (getAPMoveCosts(i, charStats.MChartype) > charStats.CurrentAp)
                {
                    //int distance = grid.GetRange(OccupiedTile, path[i - 1].node);
                    //Debug.Log("AI moved distance: " +  distance);
                    
                    CharStats.MoveReduceAp(i - 1);
                    OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = null;
                    OccupiedTile = path[i - 1].node;
                    OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = gameObject;
                    return true;
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
