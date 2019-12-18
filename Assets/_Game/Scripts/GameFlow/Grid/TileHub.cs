using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.GameFlow.Grid
{
    [ExecuteAlways]
    public class TileHub : MonoBehaviour
    {
        [Header("The width of the Grid")]
        [Range(0,50)]
        [SerializeField]
        private int width = 10;
        [Header("The Depth of the Grid")]
        [Range(0,50)]
        [SerializeField]
        private int depth = 10;
        [Header("The Distance between TileCentres")]
        [SerializeField]
        private float distanceBetweenPoints = 1f;
        [Header("TilePrefab")] [SerializeField]
        private GameObject prefab;

        [Header("Walkable Layer")] 
        [SerializeField]
        private LayerMask walkableMask;
        
        
        private int _oldWidth;
        private int _oldDepth;
        private float _oldDistance;
        private bool _prefabCheck;
        private TileGrid _grid;
        private Pathfinder _pathfinder;
        
        private void Awake()
        {
            if (transform.childCount > 0)
            {
                depth = transform.childCount;
                width = transform.GetChild(0).childCount;
            }
            _grid = new TileGrid(width, depth, distanceBetweenPoints, gameObject.transform);
            _pathfinder = new Pathfinder(_grid);
            _oldWidth = width;
            _oldDepth = depth;
            _oldDistance = distanceBetweenPoints;

            if (transform.childCount <= 0) return;
            _grid.ScanGrid();
            _prefabCheck = true;
            _grid.SetPrefab(prefab);

        }
        
        private void Update()
        {
            
            if (prefab != null && !_prefabCheck)
            {
                _grid.SetPrefab(prefab);
                _grid.BuildGrid();
                _prefabCheck = true;
            }
            if (prefab == null)
            {
                _prefabCheck = false;
            }
            if (_oldWidth != width && _prefabCheck)
            {
                _grid.UpdateWidth(width);
                _oldWidth = width;
                _grid.UpdateNeighbours();
            }
            if (_oldDepth != depth && _prefabCheck)
            {
                _grid.UpdateDepth(depth);
                _oldDepth = depth;
                _grid.UpdateNeighbours();
            }

            if (_oldDistance + 0.1f >= distanceBetweenPoints && _oldDistance - 0.1f <= distanceBetweenPoints ||
                !_prefabCheck) return;
            _grid.UpdateDistance(distanceBetweenPoints);
            _oldDistance = distanceBetweenPoints;
        }

        public TileAttribute[] GetTilesInRange(GameObject start, int range)
        {
            return _grid.GetTilesInRange(start, range, walkableMask);
        }

        public List<TileAttribute> FindPath(GameObject start, GameObject end)
        {
            return _pathfinder.FindPath(start, end);
        }

        public int GetRange(GameObject start, GameObject end)
        {
            return FindPath(start, end).Count;
        }
    }
}
