using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public virtual void Attack(GameObject target, int distance) {}
    
    [SerializeField] private int APCost;
    [SerializeField] private string abilityName;
    [SerializeField] private int aoeRange;

    private Weapon parentWeapon;
    public int ApCost
    {
        get => APCost;
        set => APCost = value;
    }

    public int AoeRange
    {
        get => aoeRange;
        set => aoeRange = value;
    }

    public Weapon ParentWeapon
    {
        get => parentWeapon;
        set => parentWeapon = value;
    }

    public string AbilityName
    {
        get => abilityName;
        set => abilityName = value;
    }
}
