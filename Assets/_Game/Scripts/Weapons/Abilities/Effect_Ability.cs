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
            stats.ActiveEffects.Add(copiedEffect);
            copiedEffect.EnableEffect(target);   
        }
    }
}
