using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour, IInputprovider
{
    public bool mouseDown()
    {
        throw new System.NotImplementedException();
    }

    public bool mouseRelease()
    {
        throw new System.NotImplementedException();
    }

    public bool mouseIsPressed()
    {
        throw new System.NotImplementedException();
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
}
