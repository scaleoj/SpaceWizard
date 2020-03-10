using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipControl : MonoBehaviour
{
    public enum Tool_Tip_Type{ABILITY, WEAPON}

    
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI headerName;
    
    [Header("Config")] 
    [Tooltip("Offset gets added to the MousePosition on the Screen")][SerializeField] private Vector2 offset;
    
    private void Awake()
    {
       
    }

    public void EnableToolTip(GameObject UIElement)
    {
        UIElement.gameObject.transform.position = new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y + offset.y,Input.mousePosition.z);
        description.text = UIElement.GetComponent<ToolTip>().GetDescription();
        headerName.text = UIElement.GetComponent<ToolTip>().GetName();
        container.SetActive(true);
    }

    public void DisableToolTip(GameObject UIElement)
    { 
        container.SetActive(false);
    }
}
