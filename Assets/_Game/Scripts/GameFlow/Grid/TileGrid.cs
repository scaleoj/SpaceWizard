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

        public void SetPrefab(GameObject pref)
        {
            _prefab = pref;
        }

        public GameObject getPrefab()
        {
            return _prefab;
        }
    
        //O(n^2 + c)
        public void BuildGrid()
        {

            for (var i = 0; i < _depth; i++)
            {
                Cubes.Add(new List<GameObject>());
                _lines.Add(new GameObject("Line" + (i + 1)));
                _lines[i].transform.SetParent(_parent);
                _lines[i].transform.position = _lines[i].transform.parent.position;
                for (var j = 0; j < _width; j++)
                {
                    Cubes[i].Add(PrefabUtility.InstantiatePrefab(_prefab) as GameObject);
                    var tile = new TileAttribute(Cubes[i][j], i, j) {node = {layer = 9}};
                    Neighbours[j, i] = tile;
                    Cubes[i][j].transform.SetParent(_lines[i].transform);
                    Cubes[i][j].name = "Tile " + (j);
                    Cubes[i][j].transform.position = _parent.position + (new Vector3((j * _distanceBetweenPoints), 0, -(i * _distanceBetweenPoints)));
                    Cubes[i][j].transform.localScale = new Vector3(Cubes[i][j].transform.localScale.x* _distanceBetweenPoints, Cubes[i][j].transform.localScale.y / 4, Cubes[i][j].transform.localScale.z* _distanceBetweenPoints);
                    
                }
            } 
        }
    
        //O(n^2 + c)
        public void ScanGrid()
        {
            for (var i = 0; i < _depth; ++i)
            {
                _lines.Add(_parent.GetChild(i).gameObject);
                _lines[i].transform.position = _lines[i].transform.parent.position;
                Cubes.Add(new List<GameObject>());
                for (var j = 0; j < _width; ++j)
                {
                    Cubes[i].Add(_lines[i].transform.GetChild(j).gameObject);
                    var tile = new TileAttribute(Cubes[i][j], i, j);
                    Neighbours[i, j] = tile;
                }
            }
        }

        public void UpdateWidth(int newWidth)
        {
            if (newWidth < _width)
            {
                for (var i = 0; i < _depth; ++i)
                {
                    for (var j = _width-1; j >= newWidth; --j)
                    {
                        Object.DestroyImmediate(Cubes[i][j]);
                        Cubes[i].Remove(Cubes[i][j]);
                    }
                }
            }
            else if (newWidth > _width)
            {
                for (var i = 0; i < _depth; ++i)
                {
                    for (var j = _width; j < newWidth; ++j)
                    {
                        //cubes[i].Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                        Cubes[i].Add(PrefabUtility.InstantiatePrefab(_prefab) as GameObject);
                        var tile = new TileAttribute(Cubes[i][j], i, j);
                        Cubes[i][j].transform.SetParent(_lines[i].transform);
                        Cubes[i][j].name = "Tile " + (j);
                        Cubes[i][j].transform.position = _parent.position + new Vector3(j * _distanceBetweenPoints, 0, -(i * _distanceBetweenPoints));
                        Cubes[i][j].transform.localScale = new Vector3(Cubes[i][j].transform.localScale.x* _distanceBetweenPoints, Cubes[i][j].transform.localScale.y / 4, Cubes[i][j].transform.localScale.z* _distanceBetweenPoints);
                    }
                }
            }

            _width = newWidth;
        }

        public void UpdateDepth(int newDepth)
        {
             if (newDepth < _depth)
             {
                 
                for(var i = _depth-1; i >= newDepth; --i)
                {
                    
                    foreach (Transform item in _lines[i].transform)
                    {
                        Cubes[i].Remove(item.gameObject);
                    }
                    
                    Cubes.Remove(Cubes[i]);
                    Object.DestroyImmediate(_lines[i]);
                    _lines.Remove(_lines[i]);
                }
                
             }else if (newDepth > _depth)
             {
                for (var i = _depth; i < newDepth; ++i)
                {
                    Cubes.Add(new List<GameObject>());
                    _lines.Add(new GameObject("Line" + (i + 1)));
                    _lines[i].transform.SetParent(_parent);
                    _lines[i].transform.position = _lines[i].transform.parent.position;
                    
                    for (var j = 0; j < _width; j++)
                    {
                        Cubes[i].Add(PrefabUtility.InstantiatePrefab(_prefab) as GameObject);
                        var tile = new TileAttribute(Cubes[i][j], i, j);
                        Cubes[i][j].transform.SetParent(_lines[i].transform);
                        Cubes[i][j].name = "Tile " + (j);
                        Cubes[i][j].transform.position = _parent.position + new Vector3(j * _distanceBetweenPoints, 0, -(i * _distanceBetweenPoints));
                        Cubes[i][j].transform.localScale = new Vector3(Cubes[i][j].transform.localScale.x* _distanceBetweenPoints, Cubes[i][j].transform.localScale.y / 4, Cubes[i][j].transform.localScale.z* _distanceBetweenPoints);
     
                    }
                }
             }

             _depth = newDepth;
        }

        public void UpdateDistance(float newDistance)
        {
            
            if (!(newDistance > 0.1f)) return;
            for(var i = 0; i < Cubes.Count; ++i)
            {
                for(var j = 0; j < Cubes[i].Count; ++j)
                {
                    var local = Cubes[i][j].transform;
                    var localScale = local.localScale;
                    var anchor = _parent.position;
                    localScale = new Vector3(localScale.x / _distanceBetweenPoints, localScale.y, localScale.z / _distanceBetweenPoints);
                    localScale = new Vector3(localScale.x * newDistance, localScale.y, localScale.z* newDistance);
                    local.localScale = localScale;
                    var position = anchor + new Vector3(j*newDistance, 0, -(i*newDistance));
                    local.transform.position = position;
                }
            }

            _distanceBetweenPoints = newDistance;
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
    }
}
