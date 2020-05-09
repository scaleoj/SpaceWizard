using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Abilities/HealMagicShield")]

public class HealMS : Ability
{ 
    public override void Attack(GameObject target, int distance)
    {
        if (target == null)
        {
            Debug.Log("MISS, Target is NULL!");
            return;
        }
        
        //Maybe change pure damage to use InstanceOf so subclasses of Weapon are usable, also AOE might be necessary
        float crit = UnityEngine.Random.Range(0f, 1f) <= 0.05f ? ParentWeapon.CritMultiplier : 1.0f;
        int magic_heal = (int) (UnityEngine.Random.Range(ParentWeapon.MinMagicDmg, ParentWeapon.MaxMagicDmg) * crit);
        int physical_heal = (int) (UnityEngine.Random.Range(ParentWeapon.MinPhysicalDmg, ParentWeapon.MaxPhysicalDmg) *
                             crit);
        int health_heal = (int) (UnityEngine.Random.Range(ParentWeapon.MinPureDmg, ParentWeapon.MaxPureDmg) *
                                 crit);
        
        target.GetComponent<Character>().CharStats.TakeHeal(0,0, magic_heal, target);
        //Visual Representation
        //TODO DIFFERENT REPRESENTATION THAN WHEN HIT
        target.GetComponent<DmgIndicator>().showHitOrMiss(true, dmgColor, "Healed", Color.cyan);
    }
}
