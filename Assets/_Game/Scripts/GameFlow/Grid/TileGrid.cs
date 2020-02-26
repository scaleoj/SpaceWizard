using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace _Game.Scripts.GameFlow.Grid
{
    [System.Serializable]
    public class TileGrid
    {

        private int _width;
        private int _depth;
        private float _distanceBetweenPoints;

        private Transform _parent;
        private GameObject _retreat;
        private LayerMask _mask;
        private List<List<GameObject>> _cubes;
        private TileAttribute[,] _neighbours;

        private List<GameObject> _lines;
        

        public TileGrid(int wid, int dep, float distance, Transform par)
        {
            _width = wid;
            _depth = dep;
            _distanceBetweenPoints = distance;
            _parent = par;
            _cubes = new List<List<GameObject>>();
            _lines = new List<GameObject>();
            _neighbours = new TileAttribute[_depth, _width];     
        }

        public TileGrid()
        {

        }

        public TileAttribute[,] GetNeighbours()
        {
            return _neighbours;
        }

        public void SetNeighbours(TileAttribute[,] temp)
        {
            _neighbours = temp;
        }

        public List<List<GameObject>> GetCubes()
        {
            return _cubes;
        }

        public void SetCubes(List<List<GameObject>> cubes)
        {
            _cubes = cubes;
        }
        
        public GameObject GetParent()
        {
            return _parent.gameObject;
        }

        public void SetParent(GameObject par)
        {
            _parent = par.transform;
        }

        public void SetRetreat(GameObject retreat)
        {
            foreach (var i in _cubes)
            {
                if (!i.Contains(retreat)) continue;
                _retreat = retreat;
            }
        }

        public GameObject GetRetreat()
        {
            return _retreat;
        }
        
        public void ScanGrid()
        {
            _width = _parent.GetChild(0).childCount;
            _depth = _parent.childCount;
            _cubes = new List<List<GameObject>>();
            _neighbours = new TileAttribute[_depth, _width];
            for (var i = 0; i < _depth; ++i)
            {
                _lines.Add(_parent.GetChild(i).gameObject);
                _lines[i].transform.position = _lines[i].transform.parent.position;
                _cubes.Add(new List<GameObject>());
                for (var j = 0; j < _width; ++j)
                {
                    _cubes[i].Add(_lines[i].transform.GetChild(j).gameObject);
                    var tile = new TileAttribute(_cubes[i][j], i, j);
                    _neighbours[i, j] = tile;
                }
            }
            
        }
        
        public void UpdateNeighbours()
        {  
            _neighbours = null;
            _neighbours = new TileAttribute[_depth, _width];
            for (var i = 0; i < _cubes.Count; ++i)
            {
                for (var j = 0; j < _cubes[i].Count; ++j)
                {
                    var tile = new TileAttribute(_cubes[i][j], i, j);
                    _neighbours[i, j] = tile;
                }
            }
        }
        
        private bool CubeContains(GameObject obj)
        {
            foreach (var list in _cubes)
            {
                if ((list.Contains(obj)))
                {
                    return true;
                }
            }

            return false;
        }
        
        public IEnumerable<TileAttribute> GetNeighboursTiles(GameObject start)
        {
            if (!CubeContains(start))
            {
                return null;
            }

            TileAttribute left = null;
            TileAttribute right = null;
            TileAttribute bot = null;
            TileAttribute top = null;
            var found = false;
            for(var i = 0; i < _depth; ++i)
            {
                for (var j = 0; j < _width; j++)
                {
                    if (_cubes[j][i] != start) continue;
                    left = j - 1 < 0 ? null : _neighbours[j - 1,i];
                    bot = i - 1 < 0 ? null : _neighbours[j,i-1];
                    right = j+1 >= _width ? null : _neighbours[j + 1,i];
                    top = i+1 >= _depth ? null : _neighbours[j,i+1];  
                    
                    found = true;
                    break;
                }

                if (found)
                {
                    break;
                }
            }
            return new[]{left, right, top, bot};
        }

        public TileAttribute[] GetTilesInRange(GameObject start, GameObject prior, int range, LayerMask mask)
        {
            //Debug.Log(range);
            var tilesInRange = GetNeighboursTiles(start).ToList();

            tilesInRange = tilesInRange.Where(c => c != null).ToList();

            if (prior != null)
            {
                tilesInRange = tilesInRange.Where(c => c.node != prior).ToList(); 

            }

            tilesInRange = tilesInRange.Where(d => 1<<d.node.layer == mask).ToList();

            if (range <= 1) return tilesInRange.ToArray();
            var countTiles = tilesInRange.ToArray();
            foreach (var tile in countTiles)
            {
                var tempTiles = GetTilesInRange(tile.Node, start, range - 1, mask);
                tempTiles = tempTiles.Where(e => e != null).ToArray();
                tempTiles = tempTiles.Where(f => 1<<f.node.layer == mask).ToArray();
                tilesInRange.AddRange(tempTiles);
            }

            return tilesInRange.Distinct().ToArray();
        }

        public LayerMask GetMask()
        {
            return _mask;
        }

        public void SetMask(LayerMask mask)
        {
            _mask = mask;
        }
    }
}
