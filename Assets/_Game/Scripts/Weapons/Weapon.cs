using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon",menuName = "ScriptableObjects/Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    //TODO GET
    [SerializeField] private string name;
    [SerializeField] private int minPhysicalDmg;
    [SerializeField] private int maxPhysicalDmg;
    [SerializeField] private int minMagicDmg;
    [SerializeField] private int maxMagicDmg;
    [SerializeField] private int range;
    [SerializeField] [Range(0f,1f)] private float missChance;
    [SerializeField] private float critMultiplier;
    [SerializeField] private Ability[] abilities;
    
    public string Name
    {
        get => name;
        set => name = value;
    }

    public int MinPhysicalDmg
    {
        get => minPhysicalDmg;
        set => minPhysicalDmg = value;
    }

    public int MaxPhysicalDmg
    {
        get => maxPhysicalDmg;
        set => maxPhysicalDmg = value;
    }

    public int MinMagicDmg
    {
        get => minMagicDmg;
        set => minMagicDmg = value;
    }

    public int MaxMagicDmg
    {
        get => maxMagicDmg;
        set => maxMagicDmg = value;
    }

    public float MissChance
    {
        get => missChance;
        set => missChance = value;
    }

    public float CritMultiplier
    {
        get => critMultiplier;
        set => critMultiplier = value;
    }
}
