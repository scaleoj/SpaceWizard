using System.Collections.Generic;
using UnityEngine;

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
        
        private void Awake()
        {
            _pathfinder = new Pathfinder(_grid);
            _retreat = _grid.GetRetreat();
            _walkableMask = _grid.GetMask();
        }

        private void Update()
        {
            Debug.Log(_retreat);
            Debug.Log(_walkableMask);
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
