using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;
using Void = UnityAtoms.Void;

public class CharSliderControl : MonoBehaviour, IAtomListener<Void>
{
    [SerializeField] private VoidEvent updateHUD;
    [Tooltip("Normal = HP, Magic = MagicShield, Physical = Armor")][SerializeField] private CharacterStat.DamageType _type;
    private CharacterStat _characterStat;
    private Slider _slider;
    void Start()
    {
        updateHUD.RegisterListener(this);
        _slider = GetComponent<Slider>();
        _characterStat = GetComponentInParent<Character>().CharStats;
        OnEventRaised(new Void());
    }

    public void OnEventRaised(Void item)
    {
        switch (_type)
        {
               case CharacterStat.DamageType.Magic:
                   _slider.maxValue = _characterStat.MaxMs;
                   _slider.value = _characterStat.CurrentMs;
                   break;
               case CharacterStat.DamageType.Normal:
                   _slider.maxValue = _characterStat.MaxHealth;
                   _slider.value = _characterStat.CurrentHealth;
                   break;
               case CharacterStat.DamageType.Physical:
                   _slider.maxValue = _characterStat.MaxArmor;
                   _slider.value = _characterStat.CurrentArmor;
                   break;
        }
    }
}
