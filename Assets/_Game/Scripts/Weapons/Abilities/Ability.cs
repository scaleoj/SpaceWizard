using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public virtual void Attack(GameObject target, int distance) {}
    
    [SerializeField] private int APCost;
    [SerializeField] private string name;
    [SerializeField] private int aoeRange;

    private Weapon motherWeapon;
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

    public Weapon MotherWeapon
    {
        get => motherWeapon;
        set => motherWeapon = value;
    }
}
