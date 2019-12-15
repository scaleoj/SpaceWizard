using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HUDState", menuName = "ScriptableObjects/GameFlow/HUDState", order = 1)]
public class State : ScriptableObject
{
    public enum currentAction{IDLE,MOVE,ATTACK1,ATTACK2,WAIT}
    [Tooltip("Selected Action from the HUD. Can be accessed also as an Index.")]
    [SerializeField]private currentAction selectedAction = currentAction.IDLE;

    public currentAction SelectedAction
    {
        get => selectedAction;
        set => selectedAction = value;
    }

    //Mapping Enums to Indexes, -1 if null
    public int getSelectedActionByIndex()
    {
        switch (SelectedAction)
        {
           case currentAction.IDLE: return 0;
           case currentAction.MOVE: return 1;
           case currentAction.ATTACK1: return 2;
           case currentAction.ATTACK2: return 3;
           case currentAction.WAIT: return 4;
           default: return -1;
        }
    }

    public void setSelectedActionByIndex(int index)
    {
        switch (index)
        {
            case 0: SelectedAction = currentAction.IDLE;
                break;
            case 1: SelectedAction = currentAction.MOVE;
                break;
            case 2: SelectedAction = currentAction.ATTACK1;
                break;
            case 3: SelectedAction = currentAction.ATTACK2;
                break;
            case 4: SelectedAction = currentAction.WAIT;
                break;
            default: Debug.LogError("Invalid index ("+ index + ") set! Choose an index between 0 - 4.");
                break;
        }
    }
}
