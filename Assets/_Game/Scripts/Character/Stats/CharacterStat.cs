using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/*
 *    Container for all Stat of a Character. If a value Changes an Event will be called.
 *
 *
 * 
 */

[CreateAssetMenu(menuName = "Character")]
public class CharacterStat : ScriptableObject
{

   public enum charType {BASE, MELEE, SUPPORT, TANK, SNIPER}
   public enum damageType{PHYSICAL, MAGIC}
   
   
   [Header("Health")]
   [SerializeField] private int currentHealth;
   [SerializeField] private int maxHealth;
   [Header("Magic Points")]
   [SerializeField] private int currentMP;
   [SerializeField] private int maxMP;
   [FormerlySerializedAs("currentArmour")]
   [Header("Armour")]
   [SerializeField] private int currentArmor;
   [FormerlySerializedAs("maxArmour")] [SerializeField] private int maxArmor;
   [Header("Magic Shield")]
   [SerializeField] private int currentMS;
   [SerializeField] private int maxMS;
   [Header("Action Points")]
   [SerializeField] private int currentAP;

   private int moveRange; //Move Range gets controlled by the AP count, look into the "GDD Tabelle" for the details.

   [Header("Other")] [SerializeField] private charType m_chartype;
   
   [Header("Weapons")] 
   [SerializeField] private Weapon primaryWeapon;
   [SerializeField] private Weapon secondaryWeapon;

   [Header("Initiative")] [Range(1, 10)] [SerializeField]
   private int initiative;

   private bool active; //ShowinInspector missing

   [Header("Team des Characters")]
   [SerializeField]
   [Range(0,1)]
   private int team;//Teamcodes have to be decided, Option for several Teams at the same time

   private void OnEnable()
   {
       switch (m_chartype)
       {
           case charType.BASE: MoveRange = CurrentAP;
               break;
           case charType.MELEE: MoveRange = CurrentAP * 3;
               break;
           case charType.SUPPORT: MoveRange = CurrentAP * 2;
               break;
           case charType.TANK: MoveRange = CurrentAP;
               break;
           case charType.SNIPER: MoveRange = CurrentAP;
               break;
       }
   }

   public void takeDamage(int phyAmount, int magicAmount)
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
       save = CurrentMS - magicAmount;
       if (save < 0)
       {
            CurrentHealth += save; //Working?
            CurrentMS = 0;
       }
       else
       {
            CurrentMS -= magicAmount;
       }       
      
   }

   //Getter, Setter
   public int MaxHealth
   {
       get => maxHealth;
       set => maxHealth = value < 0 ? 0 : value;
   }

   public int MaxMP
   {
       get => maxMP;
       set => maxMP = value < 0 ? 0 : value;
   }

   public int MaxArmor
   {
       get => maxArmor;
       set => maxArmor = value < 0 ? 0 : value;
   }

   public int MaxMS
   {
       get => maxMS;
       set => maxMS = value < 0 ? 0 : value;
   }

   public int CurrentAP
   {
       get => currentAP;
       set
       {
           currentAP = value < 0 ? 0 : value;
           switch (m_chartype)
           {
               case charType.BASE: MoveRange = CurrentAP;
                   break;
               case charType.MELEE: MoveRange = CurrentAP * 3;
                   break;
               case charType.SUPPORT: MoveRange = CurrentAP * 2;
                   break;
               case charType.TANK: MoveRange = CurrentAP;
                   break;
               case charType.SNIPER: MoveRange = CurrentAP;
                   break;
           }
       }
   }

   public int CurrentHealth
   {
       get => currentHealth;
       set => currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
   }

   public int CurrentMP
   {
       get => currentMP;
       set => currentMP = Mathf.Clamp(currentMP, 0, maxMP);
   }

   public int CurrentArmor
   {
       get => currentArmor;
       set => currentArmor =  Mathf.Clamp(currentArmor, 0, maxArmor);
   }

   public int CurrentMS
   {
       get => currentMS;
       set => currentMS = Mathf.Clamp(currentMS, 0, maxMS);
   }

   public int Initiative
   {
       get => initiative;
       set => initiative = value < 0 ? 0 : value;
   }

   public Weapon PrimaryWeapon
   {
       get => primaryWeapon;
       set => primaryWeapon = value;
   }

   public Weapon SecondaryWeapon
   {
       get => secondaryWeapon;
       set => secondaryWeapon = value;
   }

   public bool Active { get; set; }

   public int Team
   {
       get => team;
       set => team = value < 0 ? 0 : value;
   }

   public int MoveRange
   {
       get => moveRange;
       set => moveRange = value;
   }
}
