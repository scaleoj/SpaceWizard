using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class HoverControl : MonoBehaviour, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent hoverGOChanged;
    [SerializeField] private GameObjectVariable clickedGameObject;
    [SerializeField] private PlayerInputProvider m_input;
    [SerializeField] private BoolVariable mouseOverUI;

    private GameObject lastHovered;

    private bool wasClicked;

    private TileContainer.tileState saveTileState;

    void Start()
    {
        hoverGOChanged.RegisterListener(this);
        saveTileState = TileContainer.tileState.NORMAL;
        lastHovered = null;
    }

    void Update()
    {
        if (!mouseOverUI.Value)
        {
            if (!wasClicked)
            {
                wasClicked = m_input.mouse0Down();
            }   
        }

    }


    public void OnEventRaised(GameObject item)
    {
        if (item != null && item.layer == 9)
       {
           if (item.GetComponent<TileContainer>().State != TileContainer.tileState.SELECTED)
           {
               if (lastHovered != null)
               {
                   if (wasClicked)
                   {
                       lastHovered.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;
                       wasClicked = false;
                   }
                   else
                   {
                       lastHovered.GetComponent<TileContainer>().State = saveTileState;
                   }
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
