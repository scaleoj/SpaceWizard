using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    //TODO -------------------DISCLAIMER: NEEDS IMPLEMENTATION FOR DYNAMIC UPDATES DEPENDING ON THE CURRENT GAMEOBJECT IN QUEUE
    //Also this is ugly might need a rework
    
    [SerializeField] private ToolTipControl.Tool_Tip_Type toolTipType;
    [Tooltip("Only use ScriptableObjects according to the <toolTipType>")][SerializeField] private ScriptableObject buttonLinkedObject;
    //[SerializeField] private ToolTipControl toolTipControl;
    
    //Variables to cover all types
    private Weapon buttonLinkedWeapon;
    private Ability buttonLinkedAbility;
    private ToolTipInfo buttonLinkedTooltip;
    
    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            switch (toolTipType)
            {
                case ToolTipControl.Tool_Tip_Type.WEAPON:
                    buttonLinkedWeapon = (Weapon) buttonLinkedObject;
                    break;
                case ToolTipControl.Tool_Tip_Type.ABILITY:
                    buttonLinkedAbility = (Ability) buttonLinkedObject;
                    break;
                case ToolTipControl.Tool_Tip_Type.TOOLTIP_ONLY:
                    buttonLinkedTooltip = (ToolTipInfo) buttonLinkedObject;
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Scriptable Object Type is Invalid! - \n" + e);
        }
    }

    public void SetLinkedObject(ScriptableObject scriptableObject)
    {
        try
        {
            switch (toolTipType)
            {
                case ToolTipControl.Tool_Tip_Type.WEAPON:
                    buttonLinkedWeapon = (Weapon) scriptableObject;
                    break;
                case ToolTipControl.Tool_Tip_Type.ABILITY:
                    buttonLinkedAbility = (Ability) scriptableObject;
                    break;
                case ToolTipControl.Tool_Tip_Type.TOOLTIP_ONLY:
                    buttonLinkedTooltip = (ToolTipInfo) buttonLinkedObject;
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Scriptable Object Type is Invalid! - \n" + e);
        }
    }

    public string GetDescription()
    {
        switch (toolTipType)
        {
            case ToolTipControl.Tool_Tip_Type.WEAPON:
                return buttonLinkedWeapon.WeaponDescription;
            case ToolTipControl.Tool_Tip_Type.ABILITY:
                return buttonLinkedAbility.AbilityDescription;
            case ToolTipControl.Tool_Tip_Type.TOOLTIP_ONLY:
                return buttonLinkedTooltip.Description;
            default: return null;
        }
    }


    public string GetName()
    {
        switch (toolTipType)
        {
            case ToolTipControl.Tool_Tip_Type.WEAPON:
                return buttonLinkedWeapon.WeaponName;
            case ToolTipControl.Tool_Tip_Type.ABILITY:
                return buttonLinkedAbility.AbilityName;
            case ToolTipControl.Tool_Tip_Type.TOOLTIP_ONLY:
                return buttonLinkedTooltip.Header;
            default: return null;
        }
    }

    //Normal GetSet
    

    public ToolTipControl.Tool_Tip_Type ToolTipType
    {
        get => toolTipType;
        set => toolTipType = value;
    }
}
