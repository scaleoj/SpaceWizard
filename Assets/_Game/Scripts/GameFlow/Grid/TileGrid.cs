using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.GameFlow.Grid
{
    [ExecuteAlways]
    public class TileGrid
    {

        private int _width;
        private int _depth;
        private float _distanceBetweenPoints;

        private readonly Transform _parent;
        private GameObject _prefab;
        private GameObject _retreat;
        private LayerMask _mask;

        private readonly List<GameObject> _lines;

        //check implementierung und mögliche Auslagerung von Neighbour

        public TileGrid(int wid, int dep, float distance, Transform par)
        {
            _width = wid;
            _depth = dep;
            _distanceBetweenPoints = distance;
            _parent = par;
            Cubes = new List<List<GameObject>>();
            _lines = new List<GameObject>();
            Neighbours = new TileAttribute[_depth, _width];     
        }

        public TileAttribute[,] Neighbours { get; private set; }
        
        public List<List<GameObject>> Cubes { get; }


        public GameObject GetParent()
        {
            return _parent.gameObject;
        }

        public void SetRetreat(GameObject retreat)
        {
            foreach (var i in Cubes)
            {
                if (!i.Contains(retreat)) continue;
                _retreat = retreat;
                Debug.Log("Set Retreat Success");
            }
        }

        public GameObject GetRetreat()
        {
            return _retreat;
        }
        
        public void UpdateNeighbours()
        {  
            Neighbours = null;
            Neighbours = new TileAttribute[_depth, _width];
            for (var i = 0; i < Cubes.Count; ++i)
            {
                for (var j = 0; j < Cubes[i].Count; ++j)
                {
                    var tile = new TileAttribute(Cubes[i][j], i, j);
                    Neighbours[i, j] = tile;
                }
            }
        }
        
        private bool CubeContains(GameObject obj)
        {
            foreach (var list in Cubes)
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
                    if (Cubes[j][i] != start) continue;
                    left = j - 1 < 0 ? null : Neighbours[j - 1,i];
                    bot = i - 1 < 0 ? null : Neighbours[j,i-1];
                    right = j+1 >= _width ? null : Neighbours[j + 1,i];
                    top = i+1 >= _depth ? null : Neighbours[j,i+1];  
                    
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
