using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLinker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilityUIText;
    [SerializeField] private TextMeshProUGUI AP_CostUI;

    public TextMeshProUGUI AbilityUiText
    {
        get => abilityUIText;
        set => abilityUIText = value;
    }

    public TextMeshProUGUI ApCostUi
    {
        get => AP_CostUI;
        set => AP_CostUI = value;
    }
}
