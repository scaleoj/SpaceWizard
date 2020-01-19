using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class AbilityControl : MonoBehaviour, IAtomListener<int>
{
    /*Class is a messenger for the IntEvent abilitySelectChanged, since GameFlowControl already has an IntEvent its listening to.*/
    [SerializeField] private GameFlowControl gameflowControl;

    [SerializeField] private IntEvent abilitySelectChanged;

    void Awake()
    {
        abilitySelectChanged.RegisterListener(this);
    }

    public void OnEventRaised(int item)
    {
        gameflowControl.OnEventRaised(item);
    }
}
