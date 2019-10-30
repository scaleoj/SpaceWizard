using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Selected GameObject Container")]
public class GoContainer : ScriptableObject
{
    private GameObject currSelectedGO;

    public GameObject CurrSelectedGo
    {
        get => currSelectedGO;
        set => currSelectedGO = value;
    }
}
