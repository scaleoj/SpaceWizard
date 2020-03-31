using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Screiptable Objects for ToolTips of buttons which arent linked to any other Scriptable Objects.*/
[CreateAssetMenu(fileName = "ToolTip",menuName = "ScriptableObjects/Other/ToolTip")]
public class ToolTipInfo : ScriptableObject
{
    [TextArea][SerializeField] private string header;
    [TextArea][SerializeField] private string description;

    public string Header
    {
        get => header;
        set => header = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }
}
