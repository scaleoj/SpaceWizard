﻿using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Character.Stats;
using _Game.Scripts.GameFlow;
using UnityEngine;

public class CharCanvasControl : MonoBehaviour
{
    [SerializeField] private GameObject canvasPreset;

    [SerializeField] private GameFlowControl _gameFlowControl;

    [SerializeField] private Camera _camera;

    void Start()
    {
        for (int i = 0; i < _gameFlowControl.Units.Length; i++)
        {
            GameObject currCharacterGO = _gameFlowControl.Units[i];
            GameObject newCanvas = Instantiate(canvasPreset, this.transform);
            DmgIndicator dmgIndic = currCharacterGO.GetComponent<DmgIndicator>();
            CanvasPositioner canPos = newCanvas.GetComponentInChildren<CanvasPositioner>();
            canPos.Cam = _camera;
            canPos.Target = currCharacterGO;
            canPos.CharStats = currCharacterGO.GetComponent<Character>().CharStats;
            dmgIndic.EffectText = canPos.EffText;
            dmgIndic.HealthText = canPos.PureDmgText;
            dmgIndic.HitText = canPos.HitOrMissText;
            dmgIndic.MagicText = canPos.MagicDmgText;
            dmgIndic.PhysText = canPos.PhysDmgText;
            
        }
    }

    
}
