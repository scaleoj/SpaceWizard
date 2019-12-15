using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class SelectorController : MonoBehaviour, IAtomListener<GameObject>
{
    
    [SerializeField] private GameObjectEvent currentGameObjectChanged;

    private GameObject lastClicked;
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
                lastClicked.GetComponent<TileContainer>().State = TileContainer.tileState.NORMAL;
            }
            //gameObject.transform.position = item.transform.position;
            item.GetComponent<TileContainer>().State = TileContainer.tileState.SELECTED;
            lastClicked = item;
        }
        else
        {
            Debug.Log("Not a Tile.");
        }
    }
}
