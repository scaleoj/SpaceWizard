using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class SelectorController : MonoBehaviour, IAtomListener<GameObject>
{
    
    [SerializeField] private GameObjectEvent currentGameObjectChanged;

    // Start is called before the first frame update
    void Start()
    {
        currentGameObjectChanged.RegisterListener(this);
    }


    public void OnEventRaised(GameObject item)
    {
        if (item.GetComponentInParent<TileGrid>() != null)
        {
            gameObject.transform.position = item.transform.position;
        }
        else
        {
            Debug.Log("Not a Tile.");
        }
    }
}
