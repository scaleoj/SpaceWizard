using System.Collections.Generic;
using _Game.Scripts.GameFlow.Grid;
using UnityEngine;

namespace _Game.Scripts.Character.Stats
{
    public class Character : MonoBehaviour, IResettable
    {
        [SerializeField] private CharacterStat charStats;

        [SerializeField] private GameObject occupiedTile;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
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
                gameObject.transform.position = new Vector3(value.transform.position.x,1f,value.transform.position.z);
                occupiedTile = value;
            }
        }

        public bool AImove(TileHub grid,GameObject destination, List<TileAttribute> path)
        {
            int distance = grid.GetRange(OccupiedTile, destination);
            Debug.Log("AI moved distance: " +  distance);
            CharStats.MoveReduceAp(distance);
            OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = null;
            OccupiedTile = destination;
            OccupiedTile.GetComponent<TileContainer>().OccupiedGameObject = gameObject;
            return true;
        }
        
        
    }
}
