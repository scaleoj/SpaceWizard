using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject
{
    public enum currentAction{MOVE,ATTACK,WAIT,IDLE}
    [SerializeField]private currentAction test;
}
