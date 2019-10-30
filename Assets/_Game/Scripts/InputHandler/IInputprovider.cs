using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public interface IInputprovider
{
    bool mouse0Down();

    bool mouse0Release();

    bool mouse0IsPressed();

    float scrollWheelDelta();

    Vector3 mousePos();

    float Vertical();

    float Horizontal();
}
