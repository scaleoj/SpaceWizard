using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, IInputprovider
{
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
}
