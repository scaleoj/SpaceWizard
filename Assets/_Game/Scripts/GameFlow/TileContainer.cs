﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    /*
           Normal: When not clicked
           Selected: When clicked
           Target: When Targeted by an Attack        
    */
    public enum tileState
    {
        NORMAL, SELECTED, TARGET, IN_MOVE_RANGE
    }
    [Header("Meshes and Materials")]
    [SerializeField] private Mesh selectedMesh;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Mesh targetMesh;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private Mesh inMoveRangeMesh;
    [SerializeField] private Material inMoveRangeMaterial;

    [Header("Highlighter")]
    [SerializeField]private GameObject selectedHighlighter;
    private tileState state = tileState.NORMAL;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    
    [Header("Other")]
    [SerializeField] private GameObject occupiedGameObject;
    [SerializeField] private bool walkable;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public bool Walkable
    {
        get => walkable;
        set => walkable = value;
    }

    public tileState State
    {
        get => state;
        set
        {
            state = value;
            selectedHighlighter.SetActive(false);
            switch (value)
            {
                case tileState.NORMAL:
                    meshRenderer.enabled = false;
                    break;
                case tileState.SELECTED:
                    meshRenderer.enabled = true;
                    meshFilter.mesh = selectedMesh;
                    meshRenderer.material = selectedMaterial;
                    selectedHighlighter.SetActive(true);
                    break;
                case tileState.TARGET:
                    meshRenderer.enabled = true;
                    meshFilter.mesh = targetMesh;
                    meshRenderer.material = targetMaterial;
                    break;
                case tileState.IN_MOVE_RANGE:
                    meshRenderer.enabled = true;
                    meshFilter.mesh = inMoveRangeMesh;
                    meshRenderer.material = inMoveRangeMaterial;
                    break;
                default:
                    meshRenderer.enabled = false;
                    break;
            }
        } 
    }

    public GameObject OccupiedGameObject
    {
        get => occupiedGameObject;
        set => occupiedGameObject = value;
    }
}
