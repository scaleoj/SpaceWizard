using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "GridObject", menuName = "ScriptableObjects/TileGrid/GridObject", order = 2)]
    public class GridObject : ScriptableObject
    {
        public TileGrid grid;

    }
}
