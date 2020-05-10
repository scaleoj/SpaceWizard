using System;
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
         //Debug.Log("MISS");
         return;
      }
      
      float randomNumber = UnityEngine.Random.Range(0f,1f);
      if (randomNumber >= (distance * ParentWeapon.MissChance))
      {
         float crit = UnityEngine.Random.Range(0f, 1f) <= 0.05f ? ParentWeapon.CritMultiplier : 1.0f;
         int magicdmg = (int) (UnityEngine.Random.Range(ParentWeapon.MinMagicDmg, ParentWeapon.MaxMagicDmg) * crit);
         int physdmg = (int) (UnityEngine.Random.Range(ParentWeapon.MinPhysicalDmg, ParentWeapon.MaxPhysicalDmg) *
                              crit); 
         target.GetComponent<Character>().CharStats.TakeDamage( physdmg,magicdmg, target);
         
         //Visual Representation
         if (crit > 1.0f)
         {
            //Crit
            target.GetComponent<DmgIndicator>().showHitOrMiss(true, dmgColor, "CRIT", Color.red);
         }
         else
         {
            //No Crit
            target.GetComponent<DmgIndicator>().showHitOrMiss(true, dmgColor, String.Empty, Color.white);
         }
      }
      else
      {
         //Visual Representation
         target.GetComponent<DmgIndicator>().showHitOrMiss(false, Color.white, String.Empty, Color.white);
      }
   }
}
