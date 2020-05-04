using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Weapon",menuName = "ScriptableObjects/Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    //ToolTip and purely visual Stuff
    [FormerlySerializedAs("name")] [TextArea] [SerializeField] private string weaponName;
    [TextArea][SerializeField] private string weaponDescription;
    
    //Parameter
    [SerializeField] private int minPhysicalDmg;
    [SerializeField] private int maxPhysicalDmg;
    [SerializeField] private int minMagicDmg;
    [SerializeField] private int maxMagicDmg;
    [SerializeField] private int minPureDmg;
    [SerializeField] private int maxPureDmg;
    [SerializeField] private int range;
    [SerializeField] [Range(0f,1f)] private float missChance;
    [SerializeField] private float critMultiplier;
    [Header("ORDER ABILITIES FROM HIGHEST TO LOWEST AP-COST, MAX 3 ABILITIES")]
    [SerializeField] private Ability[] abilities;
    
    //Internal Attributes
    private CharacterStat parentChar;
    
    

    public CharacterStat ParentChar
    {
        get => parentChar;
        set => parentChar = value;
    }

    public string WeaponName
    {
        get => weaponName;
        set => weaponName = value;
    }

    public string WeaponDescription
    {
        get => weaponDescription;
        set => weaponDescription = value;
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

    public int MinPureDmg
    {
        get => minPureDmg;
        set => minPureDmg = value;
    }

    public int MaxPureDmg
    {
        get => maxPureDmg;
        set => maxPureDmg = value;
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

    public Ability[] Abilities
    {
        get => abilities;
        set => abilities = value;
    }

    public int Range
    {
        get => range;
        set => range = value;
    }
}
