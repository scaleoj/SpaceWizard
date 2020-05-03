using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class AbilityControl : MonoBehaviour, IAtomListener<int>
{
    /*Class is a messenger for the IntEvent abilitySelectChanged, since GameFlowControl already has an IntEvent its listening to.*/
    [SerializeField] private GameFlowControl gameflowControl;

    [SerializeField] private IntEvent abilitySelectChanged;

    [SerializeField] private State hudstate;

    void Awake()
    {
        abilitySelectChanged.RegisterListener(this);
    }

    public void OnEventRaised(int item)
    {
        gameflowControl.OnEventRaised(hudstate.getSelectedActionByIndex());
    }

    private void OnDestroy()
    {
        abilitySelectChanged.UnregisterListener(this);
    }
}
