using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.Character.Stats;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Abilities/Handcannon Base Attack")]
//Handcannon Base Attacks
public class HCBaseATK : Ability 
{
   public override void Attack( GameObject[] target, int distance)
   {
      float randomNumber = UnityEngine.Random.Range(0f,1f);
      if (randomNumber >= (distance * MotherWeapon.MissChance))
      {
         float crit = UnityEngine.Random.Range(0f, 1f) >= 0.05f ? MotherWeapon.CritMultiplier : 1.0f;
         target[0].GetComponent<Character>().CharStats.TakeDamage( (int) (UnityEngine.Random.Range(MotherWeapon.MinPhysicalDmg, MotherWeapon.MaxPhysicalDmg) * crit), (int) (UnityEngine.Random.Range(MotherWeapon.MinMagicDmg, MotherWeapon.MaxMagicDmg) * crit));
         
         Debug.Log("Hit"); //TODO Visual implementation of a HIT / raising an event for that
      }
      else
      {
         Debug.Log("MISS"); //TODO Visual implementation of a MISS / raising an event for that
      }
   }
}
