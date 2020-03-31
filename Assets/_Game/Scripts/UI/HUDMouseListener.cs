using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;

public class HUDMouseListener : MonoBehaviour, IAtomListener<GameObject>, IAtomListener<Void>
{
    [SerializeField] private GameObject statTexts, ActionMenuContainer, WeaponOneButton, WeaponTwoButton;
    [FormerlySerializedAs("background")] [SerializeField] private GameObject abilityBackground;
    [SerializeField] private GameObject APBar;


    [Header("AbilityFelder")] [SerializeField]
    private GameObject[] Weapon1AbilityButtons, Weapon2AbilityButtons;
    
    [SerializeField] private GameObjectEvent nextinQueue;

    [SerializeField] private BoolVariable isOverUIObject;

    [SerializeField] private QueueManager queue;

    [SerializeField] private VoidEvent updateHUD;

    private EventSystem _eventSystem;

    private GameObject lastObjectInQueue;

    void Awake()
    {
        nextinQueue.RegisterListener(this);
        updateHUD.RegisterListener(this);
        _eventSystem = GetComponent<EventSystem>();
        OnEventRaised(queue.Queue[queue.ActivePosition].Key);
        lastObjectInQueue = queue.Queue[queue.ActivePosition].Key;
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

    /*-Gets raised whenever next() in the QueueManager is called, Updates UI-*/
    public void OnEventRaised(GameObject item)
    {
        
        if (item != null)
        {

            if (/*item == queue.Queue[queue.ActivePosition].Key &&*/ item.GetComponent<Character>().CharStats.Team == 0)
            {
                abilityBackground.SetActive(true);
                ActionMenuContainer.SetActive(true);
                statTexts.SetActive(true);
                APBar.SetActive(true);
                if (item.GetComponent<Character>().CharStats.PrimaryWeapon.WeaponName != "EMPTY")
                {
                    ShowWeaponOneAbilities();
                }
                else
                {
                    for (int i = 0; i < Weapon1AbilityButtons.Length; i++)
                    {
                        Weapon1AbilityButtons[i].SetActive(false);
                    }
                }

                if (item.GetComponent<Character>().CharStats.SecondaryWeapon.WeaponName != "EMPTY")
                {
                    ShowWeaponTwoAbilities();
                }
                else
                {
                    for (int i = 0; i < Weapon2AbilityButtons.Length; i++)
                    {
                        Weapon2AbilityButtons[i].SetActive(false);
                    }
                }
                
            }
            else
            {
                ActionMenuContainer.SetActive(false);
                abilityBackground.SetActive(false);
            }
            
            updateHUD.Raise();
        }
        else
        {
            abilityBackground.SetActive(false);
            statTexts.SetActive(false);
            APBar.SetActive(false);
            ActionMenuContainer.SetActive(false);
        }
    }
    
    //Control methods for the UI
    public void ShowWeaponOneAbilities()
    {
        Ability[] abilitiesDummy = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>().CharStats
            .PrimaryWeapon.Abilities;
        for (int i = 0; i < abilitiesDummy.Length; i++)
        {
            Weapon1AbilityButtons[i].SetActive(true);
            Weapon1AbilityButtons[i].GetComponent<TextLinker>().AbilityUiText.text = abilitiesDummy[i].AbilityName;
            Weapon1AbilityButtons[i].GetComponent<TextLinker>().ApCostUi.text = abilitiesDummy[i].ApCost.ToString();
        }

        if (abilitiesDummy.Length != 0)
        {
            for (int i = Weapon1AbilityButtons.Length - 1; i >= abilitiesDummy.Length; i--)
            {
                Weapon1AbilityButtons[i].SetActive(false);
            }
        }
        else
        {
            Debug.Log(abilitiesDummy.Length);
        }
    }

    public void ShowWeaponTwoAbilities()
    {
        Ability[] abilitiesDummy = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>().CharStats
            .SecondaryWeapon.Abilities;
        for (int i = 0; i < abilitiesDummy.Length; i++)
        {
            Weapon2AbilityButtons[i].SetActive(true);
            Weapon2AbilityButtons[i].GetComponent<TextLinker>().AbilityUiText.text = abilitiesDummy[i].AbilityName;
            Weapon2AbilityButtons[i].GetComponent<TextLinker>().ApCostUi.text = abilitiesDummy[i].ApCost.ToString();
        }

        if (abilitiesDummy.Length != 0)
        {
            for (int i = Weapon2AbilityButtons.Length - 1; i >= abilitiesDummy.Length; i--)
            {
                Weapon2AbilityButtons[i].SetActive(false);
            }  
        }
        else
        {
            Debug.Log(abilitiesDummy.Length);
        }
    }
    
    public void OnEventRaised(Void item)
    {
        if (lastObjectInQueue != queue.Queue[queue.ActivePosition].Key)
        {
            lastObjectInQueue = queue.Queue[queue.ActivePosition].Key;
            OnEventRaised(lastObjectInQueue);
        }
    }
}
