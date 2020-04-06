using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.GameFlow;
using TMPro;
using UnityAtoms;
using UnityEngine;

public class UIAPToolTip : MonoBehaviour, IAtomListener<int>
{
    [SerializeField] private Vector2 offset;

    [SerializeField] private GameObject container;

    [SerializeField] private IntEvent currentMoveAPCost;
    
    private TextMeshProUGUI costText;

    private bool toolTipIsEnabled = false;
    
    void Awake()
    {
        costText = container.GetComponentInChildren<TextMeshProUGUI>();
        currentMoveAPCost.RegisterListener(this);
    }

    private void Update()
    {
        if (toolTipIsEnabled) container.gameObject.transform.position = new Vector3(Input.mousePosition.x + offset.x * Screen.width, Input.mousePosition.y + offset.y *  Screen.height, Input.mousePosition.z);
    }

    public void EnableAPToolTip(int costValue)
    {
        container.SetActive(true);
        container.gameObject.transform.position = new Vector3(Input.mousePosition.x + offset.x * Screen.width, Input.mousePosition.y + offset.y *  Screen.height, Input.mousePosition.z);
        costText.text = "AP: -" + costValue;
    }

    public void OnEventRaised(int item)
    {
        if (item != -1)
        {
            toolTipIsEnabled = true;
            EnableAPToolTip(item);
        }
        else
        {
            toolTipIsEnabled = false;
            container.SetActive(false); ;
        }
    }
}
