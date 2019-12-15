﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class HUDMouseListener : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObject statTexts, ActionMenuContainer, WeaponOneButton, WeaponTwoButton;
    [FormerlySerializedAs("background")] [SerializeField] private GameObject abilityBackground;
    [SerializeField] private GameObject statbackground;

    [SerializeField] private GameObjectEvent currentGameObjectChanged;

    [SerializeField] private BoolVariable isOverUIObject;

    [SerializeField] private QueueManager queue;

    private EventSystem _eventSystem;

    private TextUpdater textUpdater;

    private TextMeshProUGUI WeaponOneText, WeaponTwoText;
    // Start is called before the first frame update
    void Awake()
    {
        currentGameObjectChanged.RegisterListener(this);
        textUpdater = statTexts.GetComponent<TextUpdater>();
        WeaponOneText = WeaponOneButton.GetComponentInChildren<TextMeshProUGUI>();
        WeaponTwoText = WeaponTwoButton.GetComponentInChildren<TextMeshProUGUI>();
        _eventSystem = GetComponent<EventSystem>();
    }


    private void Update()
    {
        if (_eventSystem.IsPointerOverGameObject())
        {
            isOverUIObject.Value = true;
        }
        else
        {
            isOverUIObject.Value = false;
        }
    }

    public void WaitUpdate()
    {
        ActionMenuContainer.SetActive(false);
        abilityBackground.SetActive(false);
    }

    public void OnEventRaised(GameObject item)
    {
        
        if (item != null)
        {
            statTexts.SetActive(true);
            statbackground.SetActive(true);
            if (item == queue.Queue[queue.ActivePosition].Key)
            {
                abilityBackground.SetActive(true);
                ActionMenuContainer.SetActive(true);
                if (item.GetComponent<Character>().CharStats.PrimaryWeapon.Name == "EMPTY")
                {
                    WeaponOneButton.SetActive(false);
                }
                else
                {
                    WeaponOneButton.SetActive(true);
                    WeaponOneText.text = item.GetComponent<Character>().CharStats.PrimaryWeapon.Name;     
                }
                if (item.GetComponent<Character>().CharStats.SecondaryWeapon.Name == "EMPTY")
                {
                    WeaponTwoButton.SetActive(false);
                }
                else
                {
                    WeaponTwoButton.SetActive(true);
                    WeaponTwoText.text = item.GetComponent<Character>().CharStats.SecondaryWeapon.Name;     
                }
            }
            else
            {
                ActionMenuContainer.SetActive(false);
                abilityBackground.SetActive(false);
            }
            
            textUpdater.UpdateText(item);
        }
        else
        {
            abilityBackground.SetActive(false);
            statTexts.SetActive(false);
            statbackground.SetActive(false);
            ActionMenuContainer.SetActive(false);
        }
    }
}
