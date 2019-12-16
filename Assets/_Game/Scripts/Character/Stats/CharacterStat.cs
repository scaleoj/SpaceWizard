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

   [Header("Other")] [SerializeField] private int moveRange;
   
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

   public int MaxArmour
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
       set => currentAP = value < 0 ? 0 : value;
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

   public int CurrentArmour
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
