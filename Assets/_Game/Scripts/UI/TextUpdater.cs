using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;
using _Game.Scripts.Character.Stats;

public class TextUpdater : MonoBehaviour, IAtomListener<GameObject>
{

    [SerializeField] private GameObjectEvent CurrentGameObjectChanged;
    [SerializeField] private GameObjectVariable selectedGameobject;

    [SerializeField] private GameObject HP;
    [SerializeField] private GameObject PA;
    [SerializeField] private GameObject MS;
    [SerializeField] private GameObject AP;

    private TextMeshProUGUI HPText;
    private TextMeshProUGUI PAText;
    private TextMeshProUGUI MSText;
    private TextMeshProUGUI APText;

    //private TextMeshProUGUI guiText;
    // Start is called before the first frame update
    void Start()
    {
        HPText = HP.GetComponent<TextMeshProUGUI>();
        PAText = PA.GetComponent<TextMeshProUGUI>();
        MSText = MS.GetComponent<TextMeshProUGUI>();
        APText = AP.GetComponent<TextMeshProUGUI>();
        CurrentGameObjectChanged.RegisterListener(this);
    }

    public void OnEventRaised(GameObject item)
    {
        if(item != null) UpdateText(item);   
    }
    
    //OnEventRaised(CharacterStat item)

    public void UpdateText()
    {
        try
        {
            CharacterStat stats = selectedGameobject.Value.GetComponent<Character>().CharStats;
            HPText.text = stats.CurrentHealth + "/" + stats.MaxHealth;
            PAText.text = stats.CurrentArmor + "/" + stats.MaxArmor;
            MSText.text = stats.CurrentMs + "/" + stats.MaxMs;
            APText.text = stats.CurrentAp.ToString();
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
            HPText.text = stats.CurrentHealth + "/" + stats.MaxHealth;
            PAText.text = stats.CurrentArmor + "/" + stats.MaxArmor;
            MSText.text = stats.CurrentMs + "/" + stats.MaxMs;
            APText.text = stats.CurrentAp.ToString();
        }
    }
}
