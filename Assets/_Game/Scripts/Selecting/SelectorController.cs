using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class SelectorController : MonoBehaviour, IAtomListener<GameObject>
{
    
    [SerializeField] private GameObjectEvent currentGameObjectChanged;

    private GameObject lastClicked;

    private TileContainer.tileState saveTileState;
    // Start is called before the first frame update
    void Start()
    {
        currentGameObjectChanged.RegisterListener(this);
    }


    public void OnEventRaised(GameObject item)
    {
        if (item.GetComponent<TileContainer>() != null /*&& item.GetComponent<TileContainer>().Walkable*/)
        {
            if (lastClicked != null)
            {
                lastClicked.GetComponent<TileContainer>().State = saveTileState;
            }
            saveTileState = item.GetComponent<TileContainer>().State;
            lastClicked = item;
            item.GetComponent<TileContainer>().State = TileContainer.tileState.SELECTED;
            
        }
        else
        {
            Debug.Log("Not a Tile.");
        }
    }
}
