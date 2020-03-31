using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QueueSlotContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI positionText;

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
}
