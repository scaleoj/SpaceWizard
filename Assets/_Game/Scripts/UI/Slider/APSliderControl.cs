using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

public class APSliderControl : MonoBehaviour, IAtomListener<Void>
{
    [SerializeField] private VoidEvent updateHUD;

    [SerializeField] private QueueManager _queue;

    [SerializeField] private GameObject text, background, filled;

    private Slider _slider;
    // Start is called before the first frame update
    void Start()
    {
        updateHUD.RegisterListener(this);
        _slider = GetComponent<Slider>();
        OnEventRaised(new Void());
    }

    public void OnEventRaised(Void item)
    {
        CharacterStat currentChar = _queue.Queue[_queue.ActivePosition].Key.GetComponent<Character>().CharStats;
        if (currentChar.Team == 0)
        {
            text.SetActive(true);
            background.SetActive(true);
            filled.SetActive(true);
            _slider.maxValue = currentChar.MaxAp;
            _slider.value = currentChar.CurrentAp;
        }
        else
        {
            filled.SetActive(false);
            text.SetActive(false);
            background.SetActive(false);
        }
    }
}
