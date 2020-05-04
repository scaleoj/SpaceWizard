using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Serialization;

/*
 *    Container for all Stat of a Character. If a value Changes an Event will be called.
 *
 *
 * 
 */

namespace _Game.Scripts.Character.Stats
{
    [CreateAssetMenu(menuName = "Character")]
    public class CharacterStat : ScriptableObject
    {
        //------GENERAL------------------------------------//
        [Header("General")] 
        [SerializeField] private CharType mChartype;
        [SerializeField] [Range(0,1)] private int team;//Teamcodes have to be decided, Option for several Teams at the same time
        [Range(1, 10)] [SerializeField] private int initiative;


        //--------STATS------------------------------------//
        [SerializeField] private string charName;
        [Header("Health")]
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;
        [Header("Magic Points")]
        [SerializeField] private int currentMp;
        [SerializeField] private int maxMp;
        [FormerlySerializedAs("currentArmour")]
        [Header("Armour")]
        [SerializeField] private int currentArmor;
        [FormerlySerializedAs("maxArmour")] [SerializeField] private int maxArmor;
        [Header("Magic Shield")]
        [SerializeField] private int currentMs;
        [SerializeField] private int maxMs;
        [Header("Action Points")]
        [SerializeField] private int currentAp;

        [SerializeField] private int maxAP;

        [SerializeField] private int apgain;
   
   
        //---------WEAPONS, ARMOR AND OTHER EQUIPMENT---------//
        [Header("Equipment")] 
        [SerializeField] private Weapon primaryWeapon;
        [SerializeField] private Weapon secondaryWeapon;

        //---------ATOMS EVENTS, VARIABLES ETC-----------------//
        //[SerializeField] private QueueManager queue;
        [SerializeField] private VoidEvent updateHUD;
        [SerializeField] private BoolVariable runUpdates;
        [SerializeField] private GameObjectEvent killChar;
   
   
        //--------NOT EDITOR EXPOSED VARIABLES--------------//
        private int _moveRange; //Move Range gets controlled by the AP count, look into the "GDD Tabelle" for the details.
        private bool _active; //ShowinInspector missing
        private List<Effect> _activeEffects;

        //ENUMS
        public enum CharType {Base, Melee, Support, Tank, Sniper}
        public enum DamageType{Normal,Physical, Magic} //Normal is for the Heal to just Heal the HP
   
        //--------METHODS----------------------------------//
        public CharType MChartype => mChartype;

        public string CharName
        {
            get => charName;
            set => charName = value;
        }

        private void OnEnable()
        {
            switch (mChartype)
            {
                case CharType.Base: MoveRange = CurrentAp;
                    break;
                case CharType.Melee: MoveRange = CurrentAp * 3;
                    break;
                case CharType.Support: MoveRange = CurrentAp * 2;
                    break;
                case CharType.Tank: MoveRange = CurrentAp;
                    break;
                case CharType.Sniper: MoveRange = CurrentAp;
                    break;
            }
       
            //Init
            _activeEffects = new List<Effect>();
        }

        public void TakeHeal(int healthAmount, int phyAmount, int magicAmount, GameObject target)
        {
            CurrentHealth += healthAmount;
            CurrentArmor += phyAmount;
            CurrentMs += magicAmount;
       
            //TODO DAMAGE INDICATOR
        }
   
        public void TakeDamage(int phyAmount, int magicAmount, GameObject target)
        {
            int healthdmgtotal = 0;
            int savePhysDMG;
       
            //Physical
            savePhysDMG = CurrentArmor - phyAmount;
            if (savePhysDMG < 0)
            {
                reduceHealth_Deathcheck(-savePhysDMG, target); //Working?
                CurrentArmor = 0;
                healthdmgtotal = healthdmgtotal + (-savePhysDMG);
            }
            else
            {
                CurrentArmor -= phyAmount;
            }

            //Magic
            int saveMagicDMG = CurrentMs - magicAmount;
            if (saveMagicDMG < 0)
            {
                reduceHealth_Deathcheck(-saveMagicDMG, target); //Working?
                CurrentMs = 0;
                healthdmgtotal = healthdmgtotal + (-saveMagicDMG);
            }
            else
            {
                CurrentMs -= magicAmount;
            }

            if (healthdmgtotal != 0)
            {
                target.GetComponent<DmgIndicator>().showDamage(phyAmount, magicAmount, healthdmgtotal);           
            }
            else
            {
                target.GetComponent<DmgIndicator>().showDamage(phyAmount, magicAmount, 0);           
            }
        }

        public Effect containsEffect(int effectID)
        {
            if (_activeEffects != null && _activeEffects.Count > 0)
            {
                for (int i = 0; i < _activeEffects.Count; i++)
                {
                    if (_activeEffects[i].effectID == effectID) return _activeEffects[i];
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        //Getter, Setter
        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = value < 0 ? 0 : value;
                if(runUpdates != null && runUpdates.Value)  updateHUD.Raise();
            }
        }

        public int MaxMp
        {
            get => maxMp;
            set
            {
                maxMp = value < 0 ? 0 : value;
                if(runUpdates != null && runUpdates.Value) updateHUD.Raise();
            }
        }

        public int MaxArmor
        {
            get => maxArmor;
            set
            {
                maxArmor = value < 0 ? 0 : value;
                if(runUpdates != null && runUpdates.Value) updateHUD.Raise();
            }
        }

        public int MaxMs
        {
            get => maxMs;
            set
            {
                maxMs = value < 0 ? 0 : value;
                if(runUpdates != null && runUpdates.Value) updateHUD.Raise();
            }
        }

        public int MaxAp
        {
            get => maxAP;
            set => maxAP = value;
        }

        public int CurrentAp
        {
            get => currentAp;
            set
            {
                currentAp = Mathf.Clamp(value, 0, maxAP);
                switch (mChartype)
                {
                    case CharType.Base: MoveRange = CurrentAp;
                        break;
                    case CharType.Melee: MoveRange = CurrentAp * 3;
                        break;
                    case CharType.Support: MoveRange = CurrentAp * 2;
                        break;
                    case CharType.Tank: MoveRange = CurrentAp;
                        break;
                    case CharType.Sniper: MoveRange = CurrentAp;
                        break;
                }
           
                if(runUpdates != null && runUpdates.Value) updateHUD.Raise();
            }
        }

        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = Mathf.Clamp(value, 0, maxHealth);
                if(runUpdates != null && runUpdates.Value)updateHUD.Raise();
            } 
        }

        public void reduceHealth_Deathcheck(int value ,GameObject target)
        {
            if (currentHealth - value <= 0)
            {
                CurrentHealth -= value;
                killChar.Raise(target);
            }
            else
            {
                CurrentHealth -= value;
            }
        }

        public int CurrentMp
        {
            get => currentMp;
            set
            {
                currentMp = Mathf.Clamp(value, 0, maxMp);
                if(runUpdates != null && runUpdates.Value) updateHUD.Raise();
            }
        }

        public int CurrentArmor
        {
            get => currentArmor;
            set
            {
                currentArmor = Mathf.Clamp(value, 0, maxArmor);
                if(runUpdates != null && runUpdates.Value) updateHUD.Raise();

            }
        }

        public int CurrentMs
        {
            get => currentMs;
            set
            {
                currentMs = Mathf.Clamp(value, 0, maxMs);
                if(runUpdates != null && runUpdates.Value) updateHUD.Raise();
            }
        }

        public int Initiative
        {
            get => initiative;
            set
            {
                initiative = value < 0 ? 0 : value;
            } 
        }
   
        /* Returns the runtime created clones. Use this if u want to access any Weapons. */
        public Weapon PrimaryWeapon
        {
            get => primaryWeapon;
            set
            {
                primaryWeapon = value;
            }
        }
   
        public Weapon SecondaryWeapon
        {
            get => secondaryWeapon;
            set
            {
                secondaryWeapon = value;
            }
        }

        public bool Active { get; set; }

        public List<Effect> ActiveEffects
        {
            get => _activeEffects;
            set => _activeEffects = value;
        }


        public int Team
        {
            get => team;
            set => team = value < 0 ? 0 : value;
        }

        public int MoveRange
        {
            get => _moveRange;
            set => _moveRange = value;
        }

        public int Apgain
        {
            get => apgain;
            set => apgain = value;
        }

        public void MoveReduceAp(int distance)
        {
            switch (mChartype)
            {
                case CharType.Base: CurrentAp -= distance;
                    break;
                case CharType.Melee: CurrentAp -= distance / 3 + 1;
                    break;
                case CharType.Support: CurrentAp -= distance / 2 + 1;
                    break;
                case CharType.Tank: CurrentAp -= distance;
                    break;
                case CharType.Sniper: CurrentAp -= distance;
                    break;
            }
        }
    }
}
