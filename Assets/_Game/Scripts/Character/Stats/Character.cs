using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IResettable
{

    [SerializeField] private CharacterStat charStats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetOnMatchEnd()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        charStats.CurrentArmour = charStats.MaxArmour;
        charStats.CurrentHealth = charStats.MaxHealth;
        charStats.CurrentAP = 0;
        charStats.CurrentMP = charStats.MaxMP;
        charStats.CurrentMS = charStats.MaxMS;
    }

    public CharacterStat CharStats
    {
        get => charStats;
        set => charStats = value;
    }
}
