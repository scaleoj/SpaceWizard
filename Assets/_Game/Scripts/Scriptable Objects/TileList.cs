using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileList", menuName = "ScriptableObjects/TileGrid/TileList", order = 1)]
public class TileList : ScriptableObject
{
    public List<GameObject> tiles;
}
