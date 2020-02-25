using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "TileList", menuName = "ScriptableObjects/TileGrid/TileList", order = 1)]
    public class TileList : ScriptableObject
    {
        public List<GameObject> tiles;
    }
}
