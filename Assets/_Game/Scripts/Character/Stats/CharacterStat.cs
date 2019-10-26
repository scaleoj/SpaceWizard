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
}
