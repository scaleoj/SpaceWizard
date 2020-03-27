using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsControl",menuName = "ScriptableObjects/SettingsControl")]
public class Settings : ScriptableObject
{
    [SerializeField] private bool chromaticAberattion;
    [SerializeField] private bool depthOfField;

    public bool ChromaticAberattion
    {
        get => chromaticAberattion;
        set => chromaticAberattion = value;
    }

    public bool DepthOfField
    {
        get => depthOfField;
        set => depthOfField = value;
    }
}
