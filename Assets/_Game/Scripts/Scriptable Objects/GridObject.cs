using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.GameFlow.Grid;

namespace _Game.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "GridObject", menuName = "ScriptableObjects/TileGrid/GridObject", order = 2)]
    public class GridObject : ScriptableObject
    {

        [SerializeField]private int _width;
        [SerializeField]private int _depth;
        [SerializeField]private float _distanceBetweenPoints;
        [SerializeField]private GameObject _retreat;
        [SerializeField]private int _mask;

        
        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int Depth
        {
            get => _depth;
            set => _depth = value;
        }

        public float DistanceBetweenPoints
        {
            get => _distanceBetweenPoints;
            set => _distanceBetweenPoints = value;
        }

        public GameObject Retreat
        {
            get => _retreat;
            set => _retreat = value;
        }

        public LayerMask Mask
        {
            get => _mask;
            set => _mask = value;
        }

    }
}
