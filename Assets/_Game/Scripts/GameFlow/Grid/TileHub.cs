using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.Scriptable_Objects;

namespace _Game.Scripts.GameFlow.Grid
{
    public class TileHub : MonoBehaviour
    {        
        private int _oldWidth;
        private int _oldDepth;
        private float _oldDistance;
        private bool _prefabCheck;
        private TileGrid _grid;
        private Pathfinder _pathfinder;
        [SerializeField]private GameObject _retreat;
        private LayerMask _walkableMask;
        [SerializeField] private GridObject gridObject;
        
        
        public GameObject Retreat => _retreat;
        
        private void Awake()
        {
            _grid = new TileGrid(gridObject.Width, gridObject.Depth, gridObject.DistanceBetweenPoints, gameObject.transform);
            _grid.ScanGrid();
            Debug.Log("tilehub retreat"+ _retreat.name);
            _walkableMask = gridObject.Mask;
            _pathfinder = new Pathfinder(_grid);
            
        }
        
        public TileAttribute[] GetTilesInRange(GameObject start, int range)
        {

            return _grid.GetTilesInRange(start, null, range, _walkableMask);
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
