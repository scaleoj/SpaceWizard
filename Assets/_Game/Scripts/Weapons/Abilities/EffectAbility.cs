using System;
using _Game.Scripts.Character.Stats;
using UnityEngine;

namespace _Game.Scripts.Weapons.Abilities
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Abilities/Effect")]
    public class EffectAbility : Ability
    {
        [SerializeField] private Effect effect;
        

        public override void Attack(GameObject target, int distance)
        {
            CharacterStat stats = target.GetComponent<Character.Stats.Character>().CharStats;
            Effect copiedEffect = Instantiate(effect);
            copiedEffect.effectID = effect.effectID;

            Effect containedEffect = stats.containsEffect(copiedEffect.effectID);
        
            if (containedEffect != null)
            {
                float randomNumber = UnityEngine.Random.Range(0f,1f);
                if (randomNumber >= (distance * ParentWeapon.MissChance))
                {
                    //HIT
                    target.GetComponent<DmgIndicator>().showHitOrMiss(true, effect.dmgColor, effect.onHitText, effect.fontColor);
                    containedEffect.LeftDuration =
                        containedEffect.duration;
                }
                else
                {
                    //MISS
                    target.GetComponent<DmgIndicator>().showHitOrMiss(false, Color.white, String.Empty, Color.white);
                }
            }
            else
            {
                float randomNumber = UnityEngine.Random.Range(0f,1f);
                if (randomNumber >= (distance * ParentWeapon.MissChance))
                {
                    //HIT
                    stats.ActiveEffects.Add(copiedEffect);
                    copiedEffect.EnableEffect(target);
                    target.GetComponent<DmgIndicator>().showHitOrMiss(true, effect.dmgColor, effect.onHitText, effect.fontColor);
                }
                else
                {
                    //MISS
                    target.GetComponent<DmgIndicator>().showHitOrMiss(false, Color.white, String.Empty, Color.white);
                }
            }
        }
    }
}
