using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDState : MonoBehaviour
{
    [SerializeField] private State currentActionState;

    public void setCurrentActionState(int newState)
    {
        currentActionState.setSelectedActionByIndex(newState);
    }

   
}
