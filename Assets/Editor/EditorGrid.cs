using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using _Game.Scripts.GameFlow.Grid;
using _Game.Scripts.Scriptable_Objects;
using Object = UnityEngine.Object;

namespace Editor
{
    public class EditorGrid
    {
        private int _width;
        private int _depth;
        private float _distanceBetweenPoints;

        private readonly Transform _parent;
        private GameObject _prefab;

        private readonly List<GameObject> _lines;


        public EditorGrid(string name, int wid, int dep, float distance, GameObject pre)
        {
            _width = wid;
            _depth = dep;
            _distanceBetweenPoints = distance;
            _parent = new GameObject(name).transform;
            _parent.gameObject.AddComponent<TileHub>();
            Cubes = new List<List<GameObject>>();
            _lines = new List<GameObject>();
            Neighbours = new TileAttribute[_depth, _width];
            _prefab = pre;
        }

        public EditorGrid(GameObject par)
        {
            _parent = par.transform;
            _width = _parent.GetChild(0).childCount;
            _depth = _parent.childCount;
            if (_width > 1)
            {
                _distanceBetweenPoints = Vector3.Distance(_parent.GetChild(0).GetChild(0).position,
                    _parent.GetChild(0).GetChild(1).position);
            }
            else if (_depth > 1)
            {
                _distanceBetweenPoints = Vector3.Distance(_parent.GetChild(0).GetChild(0).position,
                    _parent.GetChild(1).GetChild(0).position);
            }
            else
            {
                _distanceBetweenPoints = 1;
            }

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
        public int GetWidth()
        {
            return _width;
        }

        public int GetDepth()
        {
            return _depth;
        }

        public float GetDistance()
        {
            return _distanceBetweenPoints;
        }


        public string GetName()
        {
            return _parent.name;
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
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
                    Neighbours[i, j] = tile;
                    Cubes[i][j].transform.SetParent(_lines[i].transform);
                    Cubes[i][j].name = "Tile " + (j);
                    Cubes[i][j].transform.position = _parent.position +
                                                     (new Vector3((j * _distanceBetweenPoints), 0,
                                                         -(i * _distanceBetweenPoints)));
                    Cubes[i][j].transform.localScale =
                        new Vector3(Cubes[i][j].transform.localScale.x * _distanceBetweenPoints,
                            Cubes[i][j].transform.localScale.y / 4,
                            Cubes[i][j].transform.localScale.z * _distanceBetweenPoints);
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
                    for (var j = _width - 1; j >= newWidth; --j)
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
                        Cubes[i].Add(PrefabUtility.InstantiatePrefab(_prefab) as GameObject);
                        var tile = new TileAttribute(Cubes[i][j], i, j);
                        Cubes[i][j].transform.SetParent(_lines[i].transform);
                        Cubes[i][j].name = "Tile " + (j);
                        Cubes[i][j].transform.position =
                            _parent.position + new Vector3(j * _distanceBetweenPoints, 0,
                                -(i * _distanceBetweenPoints));
                        Cubes[i][j].transform.localScale = new Vector3(
                            Cubes[i][j].transform.localScale.x * _distanceBetweenPoints,
                            Cubes[i][j].transform.localScale.y / 4,
                            Cubes[i][j].transform.localScale.z * _distanceBetweenPoints);
                    }
                }
            }

            _width = newWidth;
        }

        public void UpdateDepth(int newDepth)
        {
            if (newDepth < _depth)
            {
                for (var i = _depth - 1; i >= newDepth; --i)
                {
                    foreach (Transform item in _lines[i].transform)
                    {
                        Cubes[i].Remove(item.gameObject);
                    }

                    Cubes.Remove(Cubes[i]);
                    Object.DestroyImmediate(_lines[i]);
                    _lines.Remove(_lines[i]);
                }
            }
            else if (newDepth > _depth)
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
                        Cubes[i][j].transform.position =
                            _parent.position + new Vector3(j * _distanceBetweenPoints, 0,
                                -(i * _distanceBetweenPoints));
                        Cubes[i][j].transform.localScale = new Vector3(
                            Cubes[i][j].transform.localScale.x * _distanceBetweenPoints,
                            Cubes[i][j].transform.localScale.y / 4,
                            Cubes[i][j].transform.localScale.z * _distanceBetweenPoints);
                    }
                }
            }

            _depth = newDepth;
        }

        public void UpdateDistance(float newDistance)
        {
            if (!(newDistance > 0.1f)) return;
            for (var i = 0; i < Cubes.Count; ++i)
            {
                for (var j = 0; j < Cubes[i].Count; ++j)
                {
                    var local = Cubes[i][j].transform;
                    var localScale = local.localScale;
                    var anchor = _parent.position;
                    localScale = new Vector3(localScale.x / _distanceBetweenPoints, localScale.y,
                        localScale.z / _distanceBetweenPoints);
                    localScale = new Vector3(localScale.x * newDistance, localScale.y, localScale.z * newDistance);
                    local.localScale = localScale;
                    var position = anchor + new Vector3(j * newDistance, 0, -(i * newDistance));
                    local.transform.position = position;
                }
            }

            _distanceBetweenPoints = newDistance;
        }
    }
}