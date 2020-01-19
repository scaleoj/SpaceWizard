using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.Character.Stats;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Abilities/Handcannon Base Attack")]
//Handcannon Base Attacks
public class HCBaseATK : Ability 
{
   public override void Attack( GameObject target, int distance)
   {
      if (target == null)
      {
         Debug.Log("MISS");
         return;
      }
      
      float randomNumber = UnityEngine.Random.Range(0f,1f);
      if (randomNumber >= (distance * MotherWeapon.MissChance))
      {
         float crit = UnityEngine.Random.Range(0f, 1f) >= 0.05f ? MotherWeapon.CritMultiplier : 1.0f;
         int magicdmg = (int) (UnityEngine.Random.Range(MotherWeapon.MinMagicDmg, MotherWeapon.MaxMagicDmg) * crit);
         int physdmg = (int) (UnityEngine.Random.Range(MotherWeapon.MinPhysicalDmg, MotherWeapon.MaxPhysicalDmg) *
                              crit); 
         target.GetComponent<Character>().CharStats.TakeDamage( physdmg,magicdmg, target);
         
         //Visual Representation
         target.GetComponent<DmgIndicator>().showHitOrMiss(true);
         Debug.Log("Hit, Character took " + magicdmg + " MagicDamage and " + physdmg + " PhysicalDamage"); //TODO Visual implementation of a HIT / raising an event for that
      }
      else
      {
         //Visual Representation
         target.GetComponent<DmgIndicator>().showHitOrMiss(false);
         Debug.Log("MISS"); //TODO Visual implementation of a MISS / raising an event for that
      }
   }
}
