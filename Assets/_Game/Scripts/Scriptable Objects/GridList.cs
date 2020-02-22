using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "GridList", menuName = "ScriptableObjects/TileGrid/GridList", order = 2)]
    public class GridList : ScriptableObject
    {
        public List<TileGrid> gridList;

        public void OnEnable()
        {
            gridList = new List<TileGrid>();
        }
    }
}
