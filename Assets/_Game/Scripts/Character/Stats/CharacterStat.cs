using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using _Game.Scripts.GameFlow;

/*
 *    Container for all Stat of a Character. If a value Changes an Event will be called.
 *
 *
 * 
 */

[CreateAssetMenu(menuName = "Character")]
public class CharacterStat : ScriptableObject
{

   public enum CharType {Base, Melee, Support, Tank, Sniper}

   public enum DamageType{Physical, Magic}
   
   
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

   [SerializeField] private int apgain;

   private int _moveRange; //Move Range gets controlled by the AP count, look into the "GDD Tabelle" for the details.

   [Header("Other")] [SerializeField] private CharType mChartype;
   [SerializeField] private QueueManager queue;
   [SerializeField] private VoidEvent updateHUD;
   [SerializeField] private BoolVariable runUpdates;
   
   [Header("Weapons")] 
   [SerializeField] private Weapon primaryWeapon;
   [SerializeField] private Weapon secondaryWeapon;

   [Header("Initiative")] [Range(1, 10)] [SerializeField]
   private int initiative;

   private bool _active; //ShowinInspector missing

   [Header("Team des Characters")]
   [SerializeField]
   [Range(0,1)]
   private int team;//Teamcodes have to be decided, Option for several Teams at the same time

   public CharType MChartype => mChartype;

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
       
       //PrimaryWeapon.MotherChar = this;
       //SecondaryWeapon.MotherChar = this;
   }

   public void TakeDamage(int phyAmount, int magicAmount)
   {
       int save;
       
       //Physical
       save = CurrentArmor - phyAmount;
       if (save < 0)
       {
            CurrentHealth += save; //Working?
            CurrentArmor = 0;
       }
       else
       {
            CurrentArmor -= phyAmount;
       }

       //Magic
       save = CurrentMs - magicAmount;
       if (save < 0)
       {
            CurrentHealth += save; //Working?
            CurrentMs = 0;
       }
       else
       {
            CurrentMs -= magicAmount;
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

   public int CurrentAp
   {
       get => currentAp;
       set
       {
           if (value <= 0)
           {
               currentAp = 0;
           }
           else
           {
               currentAp = value;
           }
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
           //updateHUD.Raise();
       } 
   }

   public Weapon PrimaryWeapon
   {
       get => primaryWeapon;
       set
       {
           primaryWeapon = value;
           //if(runUpdates != null && runUpdates.Value) updateHUD.Raise();
       }
   }

   public Weapon SecondaryWeapon
   {
       get => secondaryWeapon;
       set
       {
           secondaryWeapon = value;
           //if(runUpdates != null && runUpdates.Value) updateHUD.Raise();
       }
   }

   public bool Active { get; set; }

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
