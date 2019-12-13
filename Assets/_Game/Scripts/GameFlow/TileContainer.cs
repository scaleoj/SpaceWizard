using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    /*
           Normal: When not clicked
           Selected: When clicked
           Target: When Targeted by an Attack        
    */
    public enum tileState
    {
        NORMAL, SELECTED, TARGET
    }

    [SerializeField] private bool walkable;
    private tileState state = tileState.NORMAL;
    [SerializeField] private GameObject occupiedGameObject;

    public bool Walkable
    {
        get => walkable;
        set => walkable = value;
    }

    public tileState State
    {
        get => state;
        set => state = value;
    }

    public GameObject OccupiedGameObject
    {
        get => occupiedGameObject;
        set => occupiedGameObject = value;
    }
}
