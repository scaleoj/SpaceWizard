using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class HUDState : MonoBehaviour
{
    [SerializeField] private State currentActionState;
    [SerializeField] private IntEvent actionChanged;

    public void setCurrentActionState(int newState)
    {
        currentActionState.setSelectedActionByIndex(newState);
        actionChanged.Raise(newState);
    }
}
