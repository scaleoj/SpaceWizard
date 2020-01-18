using System;
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
    [Header("ORDER ABILITIES FROM HIGHEST TO LOWEST AP-COST, MAX 3 ABILITIES")]
    [SerializeField] private Ability[] abilities;

    private CharacterStat motherChar;

    private void OnEnable()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].MotherWeapon = this;
        }
    }

    public CharacterStat MotherChar
    {
        get => motherChar;
        set => motherChar = value;
    }

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

    public Ability[] Abilities
    {
        get => abilities;
        set => abilities = value;
    }
}
