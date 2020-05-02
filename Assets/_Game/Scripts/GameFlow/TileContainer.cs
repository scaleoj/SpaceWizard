﻿using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
        NORMAL, SELECTED, TARGET, IN_MOVE_RANGE, HOVERING, HIGHLIGHTED
    }
    [Header("Meshes and Materials")]
    [SerializeField] private Mesh selectedMesh;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Mesh targetMesh;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private Mesh inMoveRangeMesh;
    [SerializeField] private Material inMoveRangeMaterial;
    [SerializeField] private Mesh hoveringMesh;
    [SerializeField] private Material hoveringMaterial;
    [SerializeField] private Mesh highlightMesh;
    [SerializeField] private Material highlightMaterial;

    [Header("Highlighter")]
    [SerializeField]private GameObject selectedHighlighter;
    private tileState state = tileState.NORMAL;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Coroutine currRoutine;
    
    [Header("Other")]
    [SerializeField] private GameObject occupiedGameObject;
    [SerializeField] private bool walkable;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public IEnumerator wobbleRoutine(float minSize, float speedMultiplier)
    {
        Debug.Log("ayo");
        Vector3 currScale = new Vector3(1f,1f,1f);
        //Wobble in
        for (float i = 1f; i >= minSize; i -= Time.deltaTime * speedMultiplier)
        {
            yield return null;
            currScale.x = i;
            currScale.y = i;
            currScale.z = i;
            gameObject.transform.localScale = currScale;
            Debug.Log(i);
        }
        
        gameObject.transform.localScale = new Vector3(minSize,minSize,minSize);
        
        //Wobble out
        for (float i = minSize; i <= 1f; i += Time.deltaTime * speedMultiplier)
        {
            yield return null;
            currScale.x = i;
            currScale.y = i;
            currScale.z = i;
            gameObject.transform.localScale = currScale;
        }
        
        gameObject.transform.localScale = new Vector3(1f,1f,1f);

        currRoutine = StartCoroutine(wobbleRoutine(minSize, speedMultiplier));
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
           /* if (currRoutine != null)
            {
                StopCoroutine(currRoutine);
            }
            
            if (state == tileState.HOVERING)
            {
                currRoutine = StartCoroutine(wobbleRoutine(0.75f, 1f));
            }*/

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
                    selectedHighlighter.GetComponent<Light>().color = Color.cyan;
                    break;
                case tileState.TARGET:
                    meshRenderer.enabled = true;
                    meshFilter.mesh = targetMesh;
                    meshRenderer.material = targetMaterial;
                    selectedHighlighter.SetActive(true);
                    selectedHighlighter.GetComponent<Light>().color = Color.red;
                    break;
                case tileState.IN_MOVE_RANGE:
                    meshRenderer.enabled = true;
                    meshFilter.mesh = inMoveRangeMesh;
                    meshRenderer.material = inMoveRangeMaterial;
                    selectedHighlighter.SetActive(true);                 
                    selectedHighlighter.GetComponent<Light>().color = Color.blue;
                    break;
                case tileState.HOVERING:
                    meshRenderer.enabled = true;
                    meshFilter.mesh = hoveringMesh;
                    meshRenderer.material = hoveringMaterial;
                    selectedHighlighter.SetActive(true);
                    selectedHighlighter.GetComponent<Light>().color = Color.yellow;
                    break;
                case tileState.HIGHLIGHTED:
                    meshRenderer.enabled = true;
                    meshFilter.mesh = highlightMesh;
                    meshRenderer.material = highlightMaterial;
                    selectedHighlighter.SetActive(true);
                    selectedHighlighter.GetComponent<Light>().color = Color.green;
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
