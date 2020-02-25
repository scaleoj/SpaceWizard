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
        private GameObject _retreat;
        private LayerMask _walkableMask;
        [SerializeField] private GridObject gridObject;
        
        private void Awake()
        {
            _grid = gridObject.grid;
            Debug.Log(_grid);
            _walkableMask = _grid.GetMask();
            _pathfinder = new Pathfinder(_grid);
            _retreat = _grid.GetRetreat();
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

        public GameObject Retreat => _retreat;
    }
}
