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
        _slider.maxValue = currentChar.MaxAp;
        _slider.value = currentChar.CurrentAp;
    }
}
