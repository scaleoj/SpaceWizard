using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using TMPro;
using UnityAtoms;
using UnityEngine;

public class UIQueueControl : MonoBehaviour, IAtomListener<Void>
{
    [SerializeField] private VoidEvent updateHUD;

    [SerializeField] private GameObject[] queueSlots;

    [SerializeField] private QueueManager _queueManager;

    private TextMeshProUGUI[] charTypeTexts;
    private TextMeshProUGUI[] nameTexts;
    private TextMeshProUGUI[] positionTexts;
    private TileContainer.tileState cachedState;
    
    void Start()
    {
        updateHUD.RegisterListener(this);
        nameTexts = new TextMeshProUGUI[queueSlots.Length];
        positionTexts = new TextMeshProUGUI[queueSlots.Length];
        charTypeTexts = new TextMeshProUGUI[queueSlots.Length];
        
        for (int i = 0; i < queueSlots.Length; i++)
        {
            //Set the slotvariables for performance reasons before
            charTypeTexts[i] = queueSlots[i].GetComponent<QueueSlotContainer>().CharTypeText;
            nameTexts[i] = queueSlots[i].GetComponent<QueueSlotContainer>().NameText;
            positionTexts[i] = queueSlots[i].GetComponent<QueueSlotContainer>().PositionText;

            if (i < _queueManager.Queue.Count)
            {
                queueSlots[i].SetActive(true);
                nameTexts[i].text = _queueManager.Queue[i].Key.GetComponent<Character>().CharStats.CharName;
                charTypeTexts[i].text = _queueManager.Queue[i].Key.GetComponent<Character>().CharStats.MChartype.ToString();
                if (i > 0)
                {
                    charTypeTexts[i].text = _queueManager.Queue[_queueManager.ActivePosition + i].Key
                        .GetComponent<Character>().CharStats.MChartype.ToString();
                    nameTexts[i].text = _queueManager.Queue[_queueManager.ActivePosition + i].Key.GetComponent<Character>().CharStats.CharName;
                }
                else
                {
                    nameTexts[0].text = _queueManager.Queue[_queueManager.ActivePosition].Key.GetComponent<Character>().CharStats.CharName;
                    charTypeTexts[0].text = _queueManager.Queue[_queueManager.ActivePosition].Key.GetComponent<Character>().CharStats.MChartype.ToString();
                }
            }
            else
            {
                queueSlots[i].SetActive(false);
            }
        }
        
    }

    public void OnEventRaised(Void item)
    {
        nameTexts[0].text = _queueManager.Queue[_queueManager.ActivePosition].Key.GetComponent<Character>().CharStats.CharName;
        charTypeTexts[0].text = _queueManager.Queue[_queueManager.ActivePosition].Key.GetComponent<Character>().CharStats.MChartype.ToString();
        for (int i = 0; i < queueSlots.Length; i++)
        {  
            if (i < _queueManager.Queue.Count)
            {
                queueSlots[i].SetActive(true);
                if (i > 0)
                {
                    if (_queueManager.ActivePosition + i < _queueManager.Queue.Count)
                    {
                        nameTexts[i].text = _queueManager.Queue[_queueManager.ActivePosition + i].Key.GetComponent<Character>().CharStats.CharName;
                        charTypeTexts[i].text = _queueManager.Queue[_queueManager.ActivePosition + i].Key
                            .GetComponent<Character>().CharStats.MChartype.ToString();
                    }
                    else
                    {
                        nameTexts[i].text = _queueManager.Queue[_queueManager.ActivePosition + i - _queueManager.Queue.Count].Key.GetComponent<Character>().CharStats.CharName;
                        charTypeTexts[i].text = _queueManager.Queue[_queueManager.ActivePosition + i - _queueManager.Queue.Count].Key.GetComponent<Character>().CharStats.MChartype.ToString();
                    }
                }
            }
            else
            {
                queueSlots[i].SetActive(false);
            }
        }
    }

    public void highlightOnPointerEnter(GameObject originSlot)
    {
        for (int i = 0; i < queueSlots.Length; i++)
        {
            if (originSlot == queueSlots[i])
            {
                if (_queueManager.ActivePosition + i < _queueManager.Queue.Count)
                {
                    cachedState = _queueManager.Queue[_queueManager.ActivePosition + i].Key.GetComponent<Character>().OccupiedTile
                        .GetComponent<TileContainer>().State;
                    _queueManager.Queue[_queueManager.ActivePosition + i].Key.GetComponent<Character>().OccupiedTile
                        .GetComponent<TileContainer>().State = TileContainer.tileState.HIGHLIGHTED;
                }
                else
                {
                    cachedState = _queueManager.Queue[_queueManager.ActivePosition + i - _queueManager.Queue.Count].Key.GetComponent<Character>().OccupiedTile
                        .GetComponent<TileContainer>().State;
                    _queueManager.Queue[_queueManager.ActivePosition + i - _queueManager.Queue.Count].Key.GetComponent<Character>().OccupiedTile
                        .GetComponent<TileContainer>().State = TileContainer.tileState.HIGHLIGHTED;
                }
            }
        }
    }

    public void highlightOnPointerExit(GameObject originSlot)
    {
        for (int i = 0; i < queueSlots.Length; i++)
        {
            if (originSlot == queueSlots[i])
            {
                if (_queueManager.ActivePosition + i < _queueManager.Queue.Count)
                {
                    _queueManager.Queue[_queueManager.ActivePosition + i].Key.GetComponent<Character>().OccupiedTile
                        .GetComponent<TileContainer>().State = cachedState;                }
                else
                {
                    _queueManager.Queue[_queueManager.ActivePosition + i - _queueManager.Queue.Count].Key.GetComponent<Character>().OccupiedTile
                        .GetComponent<TileContainer>().State = cachedState;
                }
            }
        }
    }
}
