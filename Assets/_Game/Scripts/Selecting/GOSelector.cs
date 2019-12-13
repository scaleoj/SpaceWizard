using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityAtoms;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GOSelector : MonoBehaviour
{
    [SerializeField] private PlayerInputProvider m_input;
    [SerializeField] private GameObjectVariable mouseSelectedGameObject;
    [SerializeField] private GameObjectEvent currentGameObjectChanged;
    [SerializeField] private GameObject stdselector;
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        RaycastHit hit;
        Ray rayfromMouse = cam.ScreenPointToRay(m_input.mousePos());
        if (Physics.Raycast(rayfromMouse.origin, rayfromMouse.direction, out hit, 50f))
        {
            if (m_input.mouse0Down())
            {
                mouseSelectedGameObject.Value = hit.transform.gameObject;
                currentGameObjectChanged.Raise(mouseSelectedGameObject.Value);
            }

            if (hit.transform.gameObject.GetComponent<TileContainer>() != null)
            {
                if (hit.transform.gameObject.GetComponent<TileContainer>().Walkable && hit.transform.gameObject.GetComponent<TileContainer>().State == TileContainer.tileState.NORMAL)
                {
                    stdselector.SetActive(true);
                    stdselector.transform.position = hit.transform.position;   
                }
                else
                {
                    stdselector.SetActive(false);
                }
            }
            else
            {
                stdselector.SetActive(false);
            }    
        }
        else
        {
            stdselector.SetActive(false);  
        }
    }
}
