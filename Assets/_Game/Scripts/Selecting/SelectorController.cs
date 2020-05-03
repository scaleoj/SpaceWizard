﻿using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class SelectorController : MonoBehaviour, IAtomListener<GameObject>
{
    /*MIGHT WANT TO DELETE THIS SCRIPT*/
    [SerializeField] private GameObjectEvent currentGameObjectChanged;

    private GameObject lastClicked;

    private TileContainer.tileState saveTileState;
    // Start is called before the first frame update
    void Start()
    {
        currentGameObjectChanged.RegisterListener(this);
    }

    private void OnDestroy()
    {
        currentGameObjectChanged.RegisterListener(this);
    }


    public void OnEventRaised(GameObject item)
    {
        /*if (item.GetComponent<TileContainer>() != null && item.layer == 9)
        {
            if (lastClicked != null)
            {
                if (lastClicked.GetComponent<TileContainer>().State != TileContainer.tileState.HOVERING)
                {
                    lastClicked.GetComponent<TileContainer>().State = saveTileState;
                }
            }

            if (item.GetComponent<TileContainer>().State != TileContainer.tileState.HOVERING)
            {
                saveTileState = item.GetComponent<TileContainer>().State;
            }
            lastClicked = item;
            item.GetComponent<TileContainer>().State = TileContainer.tileState.SELECTED;
        }
        else
        {
            Debug.Log("Not a Tile.");
        }*/
    }
}
