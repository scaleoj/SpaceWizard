using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QueueSlotContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private TextMeshProUGUI charTypeText;

    public TextMeshProUGUI NameText
    {
        get => nameText;
        set => nameText = value;
    }

    public TextMeshProUGUI PositionText
    {
        get => positionText;
        set => positionText = value;
    }

    public TextMeshProUGUI CharTypeText
    {
        get => charTypeText;
        set => charTypeText = value;
    }
}
