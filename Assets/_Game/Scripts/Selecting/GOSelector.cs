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
    [SerializeField] private GameObjectVariable currentCharacter;
    [SerializeField] private GameObjectEvent currentCharacterChanged;
    [SerializeField] private GameObjectVariable hoverGO;
    [SerializeField] private GameObjectEvent hoverGOChanged;
    [SerializeField] private GameObject stdselector;
    [SerializeField] private BoolVariable mouseOverUI;

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
            
            if (hit.transform.gameObject.GetComponent<TileContainer>() != null)
            {
                TileContainer tile = hit.transform.gameObject.GetComponent<TileContainer>();
                if (m_input.mouse0Down() && !mouseOverUI.Value)
                {
                    mouseSelectedGameObject.Value = hit.transform.gameObject;
                    currentGameObjectChanged.Raise(mouseSelectedGameObject.Value);
                    if (tile.OccupiedGameObject != null)
                    {
                        currentCharacter.Value = tile.OccupiedGameObject;
                        currentCharacterChanged.Raise(tile.OccupiedGameObject);
                    }
                    else
                    {
                        currentCharacter.Value = null;
                        currentCharacterChanged.Raise(null);
                    }
                    
                    if (tile.gameObject.layer == 9 && tile.State == TileContainer.tileState.NORMAL)
                    {
                       // stdselector.SetActive(true);
                       // stdselector.transform.position = hit.transform.position;   
                    }
                    else
                    {
                       // stdselector.SetActive(false);
                    }
                }


                if (tile.gameObject.layer == 9 && (tile.State == TileContainer.tileState.NORMAL || tile.State == TileContainer.tileState.IN_MOVE_RANGE || tile.State == TileContainer.tileState.TARGET) && !mouseOverUI.Value)
                {
                    hoverGO.Value = hit.transform.gameObject;
                    hoverGOChanged.Raise(hit.transform.gameObject);
                }
                
                

                if (mouseOverUI.Value || tile.State == TileContainer.tileState.SELECTED)
                {
                    hoverGO.Value = null;
                    hoverGOChanged.Raise(null);
                }
                
            }
            else
            {
                //stdselector.SetActive(false);
                hoverGO.Value = null;
                hoverGOChanged.Raise(null);
            }    
        }
        else
        {
            //stdselector.SetActive(false);  
            hoverGO.Value = null;
            hoverGOChanged.Raise(null);
        }
    }
}
