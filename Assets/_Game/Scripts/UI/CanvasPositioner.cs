using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using TMPro;
using UnityEngine;

public class CanvasPositioner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pureDmgText, physDmgText, magicDmgText, hitOrMissText, effText;
    [SerializeField] private CharSliderControl HPSlider, MSSlider, PSSlider;
    private CharacterStat charStats;
    [SerializeField] private GameObject UIContainer;
    [SerializeField] private float offsetx;
    [SerializeField] private float offsety;

    private GameObject target;
    private Camera cam;
    private Vector3 offset;
    private float distance;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.transform.position;
        Vector3 camPos = cam.transform.position;
        Vector3 distanceVec = targetPos - camPos; 
        //-----IMPERFOMANT
        distance = Mathf.Sqrt((distanceVec.x * distanceVec.x + distanceVec.y * distanceVec.y + distanceVec.z * distanceVec.z) * (distanceVec.x * distanceVec.x + distanceVec.y * distanceVec.y + distanceVec.z * distanceVec.z));
        offset.y = 1.2f + (distance / 750f);
        offset.x = 0.5f;
        offset.z = 0.5f;
        /*Transform uiTrans = UIContainer.transform;
        var localScale = uiTrans.localScale;
        localScale = new Vector3(localScale.x - distance * 1f,localScale.y - distance * 1f, localScale.z);
        uiTrans.localScale = localScale;*/
        UIContainer.transform.position = cam.WorldToScreenPoint(target.transform.position + offset);
    }

    public CharacterStat CharStats
    {
        get => charStats;
        set
        {
            HPSlider._CharacterStat = value;
            HPSlider.Init();
            MSSlider._CharacterStat = value;
            MSSlider.Init();
            PSSlider._CharacterStat = value;
            PSSlider.Init();
            
            charStats = value;
        } 
    }

    public Camera Cam
    {
        get => cam;
        set => cam = value;
    }
    
    public GameObject Target
    {
        get => target;
        set => target = value;
    }

    public TextMeshProUGUI PureDmgText
    {
        get => pureDmgText;
        set => pureDmgText = value;
    }

    public TextMeshProUGUI MagicDmgText
    {
        get => magicDmgText;
        set => magicDmgText = value;
    }

    public TextMeshProUGUI PhysDmgText
    {
        get => physDmgText;
        set => physDmgText = value;
    }

    public TextMeshProUGUI HitOrMissText
    {
        get => hitOrMissText;
        set => hitOrMissText = value;
    }

    public TextMeshProUGUI EffText
    {
        get => effText;
        set => effText = value;
    }
}
