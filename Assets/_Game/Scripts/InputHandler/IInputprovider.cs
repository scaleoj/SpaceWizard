using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputprovider
{
    bool mouseDown();

    bool mouseRelease();

    bool mouseIsPressed();

    float Vertical();

    float Horizontal();
}
