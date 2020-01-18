using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public virtual void Attack(GameObject origin, GameObject[] target, Weapon weapon, int distance) {}
    
    [SerializeField] private int APCost;

    public int ApCost
    {
        get => APCost;
        set => APCost = value;
    }
}
