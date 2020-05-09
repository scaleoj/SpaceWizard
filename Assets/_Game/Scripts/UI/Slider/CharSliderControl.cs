using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;
using Void = UnityAtoms.Void;

public class CharSliderControl : MonoBehaviour, IAtomListener<Void>
{
    [SerializeField] private BoolVariable runUpdates;
    [SerializeField] private VoidEvent updateHUD;
    [SerializeField] private TextMeshProUGUI textMesh;
    [Tooltip("Normal = HP, Magic = MagicShield, Physical = Armor")][SerializeField] private CharacterStat.DamageType _type;
    private CharacterStat _characterStat;
    private Slider _slider;
    
    public void Init()
    {
        updateHUD.RegisterListener(this);
        _slider = GetComponent<Slider>();
        //_characterStat = GetComponentInParent<Character>().CharStats; //-------------CHANGE
        OnEventRaised(new Void());
    }
    
    private void OnDestroy()
    {
        updateHUD.UnregisterListener(this);
    }

    public void OnEventRaised(Void item)
    {
        if (runUpdates.Value && _characterStat != null)
        {
            switch (_type)
            {
                case CharacterStat.DamageType.Magic:
                    _slider.maxValue = _characterStat.MaxMs;
                    _slider.value = _characterStat.CurrentMs;
                    textMesh.text = _characterStat.CurrentMs + "/" + _characterStat.MaxMs;
                    break;
                case CharacterStat.DamageType.Normal:
                    _slider.maxValue = _characterStat.MaxHealth;
                    _slider.value = _characterStat.CurrentHealth;
                    textMesh.text = _characterStat.CurrentHealth + "/" + _characterStat.MaxHealth;
                    break;
                case CharacterStat.DamageType.Physical:
                    _slider.maxValue = _characterStat.MaxArmor;
                    _slider.value = _characterStat.CurrentArmor;
                    textMesh.text = _characterStat.CurrentArmor + "/" + _characterStat.MaxArmor;
                    break;
            }
        }
    }

    public CharacterStat _CharacterStat
    {
        get => _characterStat;
        set => _characterStat = value;
    }
}
