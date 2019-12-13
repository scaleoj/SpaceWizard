using System.Collections.Generic;
using System.Linq;
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
            Neighbours = new TileAttributes[_depth, _width];
        }

        public TileAttributes[,] Neighbours { get; private set; }
        
        public List<List<GameObject>> Cubes { get; }

        public void SetPrefab(GameObject pref)
        {
            _prefab = pref;
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
                    //cubes[i].Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                    Cubes[i].Add(Object.Instantiate(_prefab));
                    var script = Cubes[i][j].GetComponent<TileAttributes>();
                    script.Node = Cubes[i][j];
                    script.G = 0;
                    script.H = 0;
                    script.Walkable = true;
                    script.gridX = j;
                    script.gridY = i;
                    Neighbours[j, i] = script;
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
                    var script = Cubes[i][j].GetComponent<TileAttributes>();
                    script.Node = Cubes[i][j];
                    script.G = 0;
                    script.H = 0;
                    script.Walkable = true;
                    script.gridX = j;
                    script.gridY = i;
                    Neighbours[i, j] = script;
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
                        Cubes[i].Add(Object.Instantiate(_prefab));
                        var script = Cubes[i][j].GetComponent<TileAttributes>();
                        script.Node = Cubes[i][j];
                        script.G = 0;
                        script.H = 0;
                        script.Walkable = true;
                        script.gridX = j;
                        script.gridY = i;
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
                        Cubes[i].Add(Object.Instantiate(_prefab));
                        var script = Cubes[i][j].GetComponent<TileAttributes>();
                        script.Node = Cubes[i][j];
                        script.G = 0;
                        script.H = 0;
                        script.Walkable = true;
                        script.gridX = j;
                        script.gridY = i;
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
            Neighbours = new TileAttributes[_depth, _width];
            for (var i = 0; i < Cubes.Count; ++i)
            {
                for (var j = 0; j < Cubes[i].Count; ++j)
                {
                    var script = Cubes[i][j].GetComponent<TileAttributes>();
                    script.G = 0;
                    script.H = 0;
                    script.Walkable = true;
                    script.gridX = j;
                    script.gridY = i;
                    Neighbours[i, j] = script;
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
        
        public GameObject[] GetNeighboursTiles(GameObject start)
        {
            if (!CubeContains(start))
            {
                return null;
            }

            GameObject left = null;
            GameObject right = null;
            GameObject bot = null;
            GameObject top = null;
            var found = false;
            for(var i = 0; i < _depth; ++i)
            {
                for (var j = 0; j < _width; j++)
                {
                    if (Cubes[j][i] != start) continue;
                    left = j - 1 < 0 ? null : Cubes[j - 1][i];
                    bot = i - 1 < 0 ? null : Cubes[j][i-1];
                    right = j+1 >= _width ? null : Cubes[j + 1][i];
                    top = i+1 >= _depth ? null : Cubes[j][i+1];  
                    
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

        public GameObject[] GetTilesInRange(GameObject start, int range)
        {
            var tilesInRange = GetNeighboursTiles(start);
            tilesInRange = tilesInRange.Where(c => c != null).ToArray();


            if (range <= 1) return tilesInRange;
            {
                foreach (var tile in tilesInRange)
                {
                    var tempTiles = GetTilesInRange(tile, range - 1);
                    tempTiles = tempTiles.Where(c => c != null).ToArray();
                    foreach (var tile2 in tempTiles)
                    {
                        tilesInRange.Append(tile2);
                    }
                }
            }
            return tilesInRange;
        }
        
    }
}
