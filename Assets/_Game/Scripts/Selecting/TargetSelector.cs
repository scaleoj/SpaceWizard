using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

public class TargetSelector : MonoBehaviour,IAtomListener<bool>, IAtomListener<GameObject>
{
    [SerializeField] private BoolEvent attackModeChanged;
    [SerializeField] private GameObjectVariable selectedGO;
    [SerializeField] private GameObjectVariable target;
    [SerializeField] private QueueManager queue;

    private Vector3 initialPos;

    private bool recieveInput;
    // Start is called before the first frame update
    void Awake()
    {
        attackModeChanged.RegisterListener(this);
        initialPos = transform.position;
    }

    public void OnEventRaised(bool item)
    {
        if (item)
        {
            recieveInput = true;
        }
        else
        {
            transform.position = initialPos;
        }
    }

    public void OnEventRaised(GameObject item)
    {
        if (recieveInput)
        {
            gameObject.transform.position = item.transform.position;
        }
    }
}
