using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDMouseListener : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObject statTexts, ActionMenuContainer, WeaponOneButton, WeaponTwoButton, background, statbackground;

    [SerializeField] private GameObjectEvent currentGameObjectChanged;

    [SerializeField] private BoolVariable isOverUIObject;

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

    public void OnEventRaised(GameObject item)
    {
        
        if (item != null)
        {
            statTexts.SetActive(true);
            background.SetActive(true);
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
            
            textUpdater.UpdateText(item);
        }
        else
        {
            background.SetActive(false);
            statTexts.SetActive(false);
            ActionMenuContainer.SetActive(false);
        }
    }
}
