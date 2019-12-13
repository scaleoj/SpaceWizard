using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;

public class HUDMouseListener : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObject statTexts, ActionMenuContainer, WeaponOneButton, WeaponTwoButton;

    [SerializeField] private GameObjectEvent currentGameObjectChanged;

    private TextUpdater textUpdater;

    private TextMeshProUGUI WeaponOneText, WeaponTwoText;
    // Start is called before the first frame update
    void Start()
    {
        currentGameObjectChanged.RegisterListener(this);
        textUpdater = statTexts.GetComponent<TextUpdater>();
        WeaponOneText = WeaponOneButton.GetComponentInChildren<TextMeshProUGUI>();
        WeaponTwoText = WeaponTwoButton.GetComponentInChildren<TextMeshProUGUI>();
    }


    public void OnEventRaised(GameObject item)
    {
        
        if (item.GetComponent<Character>() != null)
        {
            statTexts.SetActive(true);
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
            
            textUpdater.UpdateText();
        }
        else
        {
            statTexts.SetActive(false);
            ActionMenuContainer.SetActive(false);
        }
    }
}
