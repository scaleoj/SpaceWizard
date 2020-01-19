﻿using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class HoverControl : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent hoverGOChanged;
    [SerializeField] private GameObjectVariable clickedGameObject;

    private GameObject lastHovered;

    private TileContainer.tileState saveTileState;

    void Start()
    {
        hoverGOChanged.RegisterListener(this);
        saveTileState = TileContainer.tileState.NORMAL;
        lastHovered = null;
    }


    public void OnEventRaised(GameObject item)
    {
       /* if (item.GetComponent<TileContainer>() != null && item.layer == 9 && item.GetComponent<TileContainer>().State != TileContainer.tileState.SELECTED)
        {
            if (lastHovered != null && clickedGameObject.Value != lastHovered)
            {
                lastHovered.GetComponent<TileContainer>().State = saveTileState;
            }
            
            saveTileState = item.GetComponent<TileContainer>().State;
            lastHovered = item;
            item.GetComponent<TileContainer>().State = TileContainer.tileState.HOVERING;
        }
        else
        {
            lastHovered.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;
        }*/
       
       

       if (item != null && item.layer == 9)
       {
           if (item.GetComponent<TileContainer>().State != TileContainer.tileState.SELECTED)
           {
               if (lastHovered != null)
               {
                   lastHovered.GetComponent<TileContainer>().State = saveTileState;
               }
               saveTileState = item.GetComponent<TileContainer>().State;
               lastHovered = item;
               item.GetComponent<TileContainer>().State = TileContainer.tileState.HOVERING;
           }
           else
           {
               if (lastHovered != null)
               {
                   lastHovered.GetComponent<TileContainer>().State = saveTileState;
               }
           }
       }
       else
       {
           if (lastHovered != null)
           {
               lastHovered.GetComponent<TileContainer>().State = saveTileState;
               lastHovered = null;
           }
       }
    }
}
