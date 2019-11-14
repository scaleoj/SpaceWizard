using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour, IAtomListener<GameObject>
{

    [SerializeField] private GameObjectEvent CurrentGameObjectChanged;
    [SerializeField] private GameObjectVariable selectedGameobject;

    private TextMeshProUGUI guiText;
    // Start is called before the first frame update
    void Start()
    {
        guiText = GetComponent<TextMeshProUGUI>();
        CurrentGameObjectChanged.RegisterListener(this);
    }

    public void OnEventRaised(GameObject item)
    {
        UpdateText(item);   
    }
    
    //OnEventRaised(CharacterStat item)

    public void UpdateText()
    {
        try
        {
            CharacterStat stats = selectedGameobject.Value.GetComponent<Character>().CharStats;
            guiText.text = "HP    " + stats.CurrentHealth + "/" +
                           stats.MaxHealth + "\n" + "Armor " + stats.CurrentArmour + "/" + stats.MaxArmour + "\n" +
                           "MS    " + stats.CurrentMS + "/" + stats.MaxMS + "\n" + "AP    " +  stats.CurrentAP;
        }
        catch (Exception e)
        {
            Debug.Log("Warning: The Current Stats Text cannot be changed. Error" + e);
        }
    }

    public void UpdateText(GameObject item)
    {
        if (item.GetComponent<Character>() != null)
        {
            CharacterStat stats = item.GetComponent<Character>().CharStats;
            guiText.text = "HP    " + stats.CurrentHealth + "/" +
                           stats.MaxHealth + "\n" + "Armor " + stats.CurrentArmour + "/" + stats.MaxArmour + "\n" + "MS    " + stats.CurrentMS + "/" + stats.MaxMS + "\n" + "AP    " +  stats.CurrentAP;
        }
    }
}
