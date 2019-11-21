using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, IInputprovider
{
    [SerializeField] private float deadButtonTime;
    private bool buttonQwasPressed = false;
    private bool buttonEwasPressed = false;
    
    public bool mouse0Down()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool mouse0Release()
    {
        return Input.GetMouseButtonUp(0);
    }

    public bool mouse0IsPressed()
    {
        return Input.GetMouseButton(0);
    }

    public float Vertical()
    {
        return Input.GetAxis("Horizontal");
    }

    public float scrollWheelDelta()
    {
        return Input.mouseScrollDelta.y;
    }

    public float Horizontal()
    {
        return Input.GetAxis("Vertical");
    }

    public Vector3 mousePos()
    {
        return Input.mousePosition;
    }

    public bool RotateLeft()
    {
        if (Input.GetAxis("Q") == 0f)
        {
            buttonQwasPressed = false;
        }
        
        if (buttonQwasPressed)
        {
            return false;
        }
        
        if (Input.GetAxis("Q") == 1f)
        {
            buttonQwasPressed = true;
        }

        return buttonQwasPressed;
    }

    public bool RotateRight()
    {
        if (Input.GetAxis("E") == 0f)
        {
            buttonEwasPressed = false;
        }
        
        if (buttonEwasPressed)
        {
            return false;
        }
        
        if (Input.GetAxis("E") == 1f)
        {
            buttonEwasPressed = true;
        }

        return buttonEwasPressed;
    }
}
