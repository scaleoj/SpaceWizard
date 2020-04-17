using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Abilities/Effect")]
public class Effect_Ablity : Ability
{
    [SerializeField] private Effect _effect;

    public override void Attack(GameObject target, int distance)
    {
        CharacterStat stats = target.GetComponent<Character>().CharStats;
        Effect copiedEffect = Instantiate(_effect);
        copiedEffect.effectID = _effect.effectID;

        Effect containedEffect = stats.containsEffect(copiedEffect.effectID);
        
        if (containedEffect != null)
        {
            containedEffect.LeftDuration =
                containedEffect.duration;
        }
        else
        {
            float randomNumber = UnityEngine.Random.Range(0f,1f);
            if (randomNumber >= (distance * ParentWeapon.MissChance))
            {
                //HIT
                stats.ActiveEffects.Add(copiedEffect);
                copiedEffect.EnableEffect(target);  
                target.GetComponent<DmgIndicator>().showHitOrMiss(true, Color.green, "Poisoned", Color.green);
            }
            else
            {
                //MISS
                target.GetComponent<DmgIndicator>().showHitOrMiss(false, Color.white, String.Empty, Color.white);
            }
        }
    }
}
