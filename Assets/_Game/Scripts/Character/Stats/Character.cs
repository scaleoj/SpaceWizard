using UnityEngine;

namespace _Game.Scripts.Character.Stats
{
    public class Character : MonoBehaviour, IResettable
    {
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
            CharStats.CurrentArmour = CharStats.MaxArmour;
            CharStats.CurrentHealth = CharStats.MaxHealth;
            CharStats.CurrentAP = 0;
            CharStats.CurrentMP = CharStats.MaxMP;
            CharStats.CurrentMS = CharStats.MaxMS;
        }

        [field: SerializeField]
        public CharacterStat CharStats { get; set; }

        [field: SerializeField]
        public GameObject OccupiedTile { get; set; }
    }
}
