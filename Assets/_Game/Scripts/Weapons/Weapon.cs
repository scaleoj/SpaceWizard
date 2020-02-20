﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon",menuName = "ScriptableObjects/Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    //Parameter
    [SerializeField] private string name;
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
    
    private Ability[] abilityClones;

    public void InitiateAbilities()
    {
        abilityClones = new Ability[abilities.Length];
        for (int i = 0; i < abilities.Length; i++)
        {
            abilityClones[i] = ScriptableObject.Instantiate(abilities[i]);
            abilityClones[i].ParentWeapon = this;
        }
    }

    public CharacterStat ParentChar
    {
        get => parentChar;
        set => parentChar = value;
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
        get => abilityClones;
        set => abilityClones = value;
    }

    public int Range
    {
        get => range;
        set => range = value;
    }
}
