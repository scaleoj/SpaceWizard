using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/Effects/Poison")]
public class Effect_Poison : Effect
{
    [Header("Details")]
    [SerializeField] private int physDamage;
    [SerializeField] private int magicDamage;

    public override void EnableEffect(GameObject target)
    {
        isActive = true;
        infectedCharacter = target.GetComponent<Character>();
    }

    public override void DisableEffect(GameObject target)
    {
        isActive = false;
        infectedCharacter = null;  
        target.GetComponent<Character>().CharStats.ActiveEffects.Remove(this);
    }

    public override void ApplyEffect(GameObject target)
    {
        CharacterStat stats = target.GetComponent<Character>().CharStats;
        stats.TakeDamage(physDamage, magicDamage, target);
    }
}
