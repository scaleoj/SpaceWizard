using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;

public class EnemyStatControl : MonoBehaviour, IAtomListener<Void>, IAtomListener<GameObject>
{
    [SerializeField] private VoidEvent charStatChange;
    
    [SerializeField] private GameObjectEvent CurrentGameObjectChanged;
    [SerializeField] private GameObjectVariable selectedGameobject;
    [SerializeField] private QueueManager _queueManager;

    [SerializeField] private GameObject HP;
    [SerializeField] private GameObject PA;
    [SerializeField] private GameObject MS;
    [SerializeField] private GameObject AP;
    [SerializeField] private GameObject enemyName;
    [SerializeField] private GameObject Container;
    

    private TextMeshProUGUI HPText;
    private TextMeshProUGUI PAText;
    private TextMeshProUGUI MSText;
    private TextMeshProUGUI APText;
    private TextMeshProUGUI enemyNameText;

    //private TextMeshProUGUI guiText;
    // Start is called before the first frame update
    void Start()
    {
        HPText = HP.GetComponent<TextMeshProUGUI>();
        PAText = PA.GetComponent<TextMeshProUGUI>();
        MSText = MS.GetComponent<TextMeshProUGUI>();
        APText = AP.GetComponent<TextMeshProUGUI>();
        enemyNameText = enemyName.GetComponent<TextMeshProUGUI>();
        CurrentGameObjectChanged.RegisterListener(this);
        charStatChange.RegisterListener(this);
    }


    public void OnEventRaised(Void item)
    {
        UpdateText();
    }

    public void OnEventRaised(GameObject item)
    {
        UpdateText(item);
    }

    public void UpdateText(GameObject item)
    {

        if (item != null && item.GetComponent<TileContainer>().OccupiedGameObject != null && item.GetComponent<TileContainer>().OccupiedGameObject.GetComponent<Character>().CharStats.Team == 1)
        {
            Container.SetActive(true);
            CharacterStat stats = item.GetComponent<TileContainer>().OccupiedGameObject.GetComponent<Character>().CharStats;
            HPText.text = "HP: " + stats.CurrentHealth + "/" + stats.MaxHealth;
            PAText.text = "PA: " +stats.CurrentArmor + "/" + stats.MaxArmor;
            MSText.text = "MS: " +stats.CurrentMs + "/" + stats.MaxMs;
            APText.text = "AP: " +stats.CurrentAp.ToString();
            enemyNameText.text = "Enemy \n" + stats.CharName;
        }
        else
        {
            Container.SetActive(false);
        }
    }

    public void UpdateText()
    {
            
        if (selectedGameobject != null && selectedGameobject.Value.GetComponent<TileContainer>().OccupiedGameObject != null && selectedGameobject.Value.GetComponent<TileContainer>().OccupiedGameObject.GetComponent<Character>().CharStats.Team == 1)
        {
            Container.SetActive(true);
            CharacterStat stats = selectedGameobject.Value.GetComponent<TileContainer>().OccupiedGameObject.GetComponent<Character>().CharStats;
            HPText.text = "HP: " + stats.CurrentHealth + "/" + stats.MaxHealth;
            PAText.text = "PA: " +stats.CurrentArmor + "/" + stats.MaxArmor;
            MSText.text = "MS: " +stats.CurrentMs + "/" + stats.MaxMs;
            APText.text = "AP: " +stats.CurrentAp.ToString();
            enemyNameText.text = "Enemy \n" + stats.CharName;
        }
        else
        {
            Container.SetActive(false);
        }
    }
}
