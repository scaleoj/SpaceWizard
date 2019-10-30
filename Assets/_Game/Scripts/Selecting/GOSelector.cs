﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GOSelector : MonoBehaviour
{
    [SerializeField] private PlayerInputProvider m_input;
    [SerializeField] private GoContainer selectGO;
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        if (m_input.mouse0Down())
        {
            RaycastHit hit;
            Ray rayfromMouse = cam.ScreenPointToRay(m_input.mousePos());
            if (Physics.Raycast(rayfromMouse.origin, rayfromMouse.direction, out hit, 50f))
            {
                selectGO.CurrSelectedGo = hit.transform.gameObject;
            }
        }
    }
}
