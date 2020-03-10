using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;

public class HUDMouseListener : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObject statTexts, ActionMenuContainer, WeaponOneButton, WeaponTwoButton;
    [FormerlySerializedAs("background")] [SerializeField] private GameObject abilityBackground;
    [SerializeField] private GameObject statbackground;
    [Header("Weapon Ability Container")] 
    [SerializeField]
    private GameObject W1AC, W2AC;
    [Header("AbilityFelder")] [SerializeField]
    private GameObject[] Weapon1AbilityButtons, Weapon2AbilityButtons;
    
    [SerializeField] private GameObjectEvent nextinQueue;

    [SerializeField] private BoolVariable isOverUIObject;

    [SerializeField] private QueueManager queue;

    [SerializeField] private VoidEvent updateHUD;

    private EventSystem _eventSystem;

    private TextUpdater textUpdater;

    private TextMeshProUGUI WeaponOneText, WeaponTwoText;

    //private GameObject lastClickedGameObject;
    // Start is called before the first frame update
    void Awake()
    {
        nextinQueue.RegisterListener(this);
        textUpdater = statTexts.GetComponent<TextUpdater>();
        WeaponOneText = WeaponOneButton.GetComponentInChildren<TextMeshProUGUI>();
        WeaponTwoText = WeaponTwoButton.GetComponentInChildren<TextMeshProUGUI>();
        _eventSystem = GetComponent<EventSystem>();
        OnEventRaised(queue.Queue[queue.ActivePosition].Key);
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
           // lastClickedGameObject = item;
            statTexts.SetActive(true);
            statbackground.SetActive(true);
            if (/*item == queue.Queue[queue.ActivePosition].Key &&*/ item.GetComponent<Character>().CharStats.Team == 0)
            {
                abilityBackground.SetActive(true);
                ActionMenuContainer.SetActive(true);
                if (item.GetComponent<Character>().CharStats.PrimaryWeapon.WeaponName == "EMPTY")
                {
                    WeaponOneButton.SetActive(false);
                }
                else
                {
                    WeaponOneButton.SetActive(true);
                    WeaponOneText.text = item.GetComponent<Character>().CharStats.PrimaryWeapon.WeaponName;     
                }
                if (item.GetComponent<Character>().CharStats.SecondaryWeapon.WeaponName == "EMPTY")
                {
                    WeaponTwoButton.SetActive(false);
                }
                else
                {
                    WeaponTwoButton.SetActive(true);
                    WeaponTwoText.text = item.GetComponent<Character>().CharStats.SecondaryWeapon.WeaponName;     
                }
            }
            else
            {
                ActionMenuContainer.SetActive(false);
                abilityBackground.SetActive(false);
            }
            
            updateHUD.Raise();
            //textUpdater.UpdateText(item);
        }
        else
        {
            abilityBackground.SetActive(false);
            statTexts.SetActive(false);
            statbackground.SetActive(false);
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
            Weapon1AbilityButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = abilitiesDummy[i].AbilityName;
        }
    }

    public void ShowWeaponTwoAbilities()
    {
        Ability[] abilitiesDummy = queue.Queue[queue.ActivePosition].Key.GetComponent<Character>().CharStats
            .SecondaryWeapon.Abilities;
        for (int i = 0; i < abilitiesDummy.Length; i++)
        {
            Weapon2AbilityButtons[i].SetActive(true);
            Weapon2AbilityButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = abilitiesDummy[i].AbilityName;
        }
    }
}
