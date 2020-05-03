using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipControl : MonoBehaviour
{
    public enum Tool_Tip_Type{ABILITY, WEAPON, TOOLTIP_ONLY}

    
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI headerName;
    [SerializeField] private BoolVariable isMouseOverUI;
    [SerializeField] private EventSystem m_EventSystem;
    [SerializeField] private GraphicRaycaster m_Raycaster;
    [Header("Settings")] [SerializeField] private float delay;
    private PointerEventData m_PointerEventData;

    //For Update Logic
    private GameObject currentTTGO;
    private bool showingToolTip;
    private float currentTime;
    private bool startCooldown;

    [Header("Config")] 
    [Tooltip("Offset gets added to the MousePosition on the Screen")][SerializeField] private Vector2 offset;

    void Update()
    {
        if (isMouseOverUI.Value)
        {
            //Get GameObject the Pointer is over and save every Gameobject that was hit in the list.
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            
            //Look wether the Hovered ToolTip/Gameobject has changed
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<ToolTip>() != null)
                {
                    if (currentTTGO != result.gameObject)
                    {
                        DisableToolTip();
                        showingToolTip = false;
                    }
                    break;
                }
            }

                
            if (!showingToolTip)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= delay)
                {
                    currentTime = 0f;
                    
                    //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.GetComponent<ToolTip>() != null)
                        {
                            EnableToolTip(result.gameObject);
                            currentTTGO = result.gameObject;
                            showingToolTip = true;
                            break;
                        }
                        else
                        {
                            DisableToolTip();
                        }
                    }
                }
            }
        }
        else
        {
            currentTTGO = null;
            DisableToolTip();
        }
    }

    public void EnableToolTip(GameObject UIElement)
    {
        container.SetActive(true);
        container.gameObject.transform.position = new Vector3(Input.mousePosition.x + offset.x * Screen.width, Input.mousePosition.y + offset.y *  Screen.height, Input.mousePosition.z);
        description.text = UIElement.GetComponent<ToolTip>().GetDescription();
        headerName.text = UIElement.GetComponent<ToolTip>().GetName();
    }

    public void DisableToolTip()
    { 
        container.SetActive(false);
    }
}
