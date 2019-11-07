using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;
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
   [Header("Armour")]
   [SerializeField] private int currentArmour;
   [SerializeField] private int maxArmour;
   [Header("Magic Shield")]
   [SerializeField] private int currentMS;
   [SerializeField] private int maxMS;
   [Header("Action Points")]
   [SerializeField] private int currentAP;

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
       get => maxArmour;
       set => maxArmour = value < 0 ? 0 : value;
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
       get => currentArmour;
       set => currentArmour =  Mathf.Clamp(currentArmour, 0, maxArmour);
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

   public bool Active { get; set; }

   public int Team
   {
       get => team;
       set => team = value < 0 ? 0 : value;
   }
}
