using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleControl : MonoBehaviour
{
    public enum SettingsType{CHROMATIC_ABERRATION, DEPTH_OF_FIELD}
    [SerializeField] private Settings _settings;

    [SerializeField] private SettingsType type;

    private Toggle _toggle;
    // Start is called before the first frame update
    void Start()
    {
        _toggle = GetComponent<Toggle>();
        switch (type)
        {
            case SettingsType.DEPTH_OF_FIELD: _toggle.isOn = _settings.DepthOfField;
                break;
            case SettingsType.CHROMATIC_ABERRATION: _toggle.isOn = _settings.ChromaticAberattion;
                break;
            default: Debug.LogError("Unknown Settingstype!");
                break;
        }
    }
    
}
