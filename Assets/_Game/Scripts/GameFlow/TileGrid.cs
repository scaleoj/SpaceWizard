using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.GameFlow
{
    [ExecuteAlways]
    public class TileGrid : MonoBehaviour
    {
        [Header("The width of the Grid")]
        [Range(1,50)]
        [SerializeField]
        private int width = 10;
        [Header("The Depth of the Grid")]
        [Range(1,50)]
        [SerializeField]
        private int depth = 10;
        [SerializeField]
        private float distanceBetweenPoints = 1f;

        private int _oldWidth;
        private int _oldDepth;
        private float _oldDistance;
        
        
    
        public List<List<GameObject>> cubes;
        private List<GameObject> Lines;
        private GameObject[,] _neighbours;

        private void Awake()
        {
            cubes = new List<List<GameObject>>();
            Lines = new List<GameObject>();
            if (transform.childCount > 0)
            {
                depth = transform.childCount;
                width = transform.GetChild(0).childCount;
            }
            _neighbours = new GameObject[width,depth];
            _oldWidth = width;
            _oldDepth = depth;
            _oldDistance = distanceBetweenPoints;

            if (transform.childCount > 0)
            {
                ScanGrid();
            }
            else
            {
                BuildGrid();
            }

        }

    

        //O(c <-> 2n^2+c)
        private void Update()
        {
            if (_oldWidth != width)
            {
                UpdateWidth();
                UpdateNeighbours();
            }
            if (_oldDepth != depth)
            {
                UpdateDepth();
                UpdateNeighbours();
            }
            if (!(_oldDistance + 0.1f >= distanceBetweenPoints && _oldDistance - 0.1f <= distanceBetweenPoints))
            {      
                UpdateDistance();
            }
        }

       
        //O(n^2 + c)
        /*
        public GameObject[] GetNeighbours(GameObject start)
        {
            if (!(cubes.Contains(start)))
            {
                return null;
            }

            GameObject left = null;
            GameObject right = null;
            GameObject bot = null;
            GameObject top = null;
            var found = false;
            for(var i = 0; i < depth; ++i)
            {
                for (var j = 0; j < width; j++)
                {
                    if (_neighbours[j, i] != start) continue;
                    left = j-1 < 0 ? null : _neighbours[j - 1, i];
                    bot = i-1 < 0 ? null : _neighbours[j, i-1];
                    right = j+1 > width ? null : _neighbours[j + 1, i];
                    top = i+1 > depth ? null : _neighbours[j, i+1];
                    //QuerNachbarn auch?
                    found = true;
                    break;
                }

                if (found)
                {
                    break;
                }
            }
            return new[]{left, right, bot, top};
        }
        
        */
    
        //O(n^2 + c)
        private void BuildGrid()
        {
            for (var i = 0; i < depth; i++)
            {
                cubes.Add(new List<GameObject>());
                Lines.Add(new GameObject("Line" + (i + 1)));
                Lines[i].transform.SetParent(gameObject.transform);
                for (var j = 0; j < width; j++)
                {
                    cubes[i].Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                    _neighbours[j, i] = cubes[i][j];
                    cubes[i][j].transform.SetParent(Lines[i].transform);
                    cubes[i][j].name = "Tile " + (i *(1 +j));
                    cubes[i][j].transform.position = new Vector3(j * distanceBetweenPoints, 0, i * distanceBetweenPoints);
                    cubes[i][j].transform.localScale = new Vector3(cubes[i][j].transform.localScale.x, cubes[i][j].transform.localScale.y / 4, cubes[i][j].transform.localScale.z);
                    cubes[i][j].AddComponent<TileContainer>();
                }
            } 
        }

        //O(n^2 + c)
        private void ClearGrid()
        {
            for (var i = 0; i < _oldDepth; ++i)
            {
                for (var j = 0; j < _oldWidth; ++j)
                {
                    DestroyImmediate(cubes[i][j]);
                    DestroyImmediate(Lines[i]);;

                }
            }
            _oldDepth = depth;
            _oldWidth = width;
            _oldDistance = distanceBetweenPoints;
            cubes.Clear();
            Lines.Clear();

        }
    
        //O(n^2 + c)
        private void ScanGrid()
        {
            for (var i = 0; i < depth; ++i)
            {
                Lines.Add(gameObject.transform.GetChild(i).gameObject);
                for (var j = 0; j < width; ++j)
                {
                    cubes[i].Add(Lines[i].transform.GetChild(j).gameObject);
                    _neighbours[j, i] = cubes[i][j];
                    var script = cubes[i][j].GetComponent<TileContainer>();
                    if (script != null)
                    {
                        cubes[i][j].AddComponent<TileContainer>();
                    }
                
                }
            }
        

        }

        private void UpdateWidth()
        {
            if (width < _oldWidth)
            {
                for (var i = 0; i < depth; ++i)
                {
                    for (var j = _oldWidth-1; j >= width; --j)
                    {
                        DestroyImmediate(cubes[i][j]);
                        cubes[i].Remove(cubes[i][j]);
                    }
                }
            }
            else if (width > _oldWidth)
            {
                for (var i = 0; i < depth; ++i)
                {
                    for (var j = _oldWidth; j < width; ++j)
                    {
                        cubes[i].Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                        cubes[i][j].transform.SetParent(Lines[i].transform);
                        cubes[i][j].name = "Tile " + (j*(i+1));
                        cubes[i][j].transform.position = new Vector3(j * distanceBetweenPoints, 0, i * distanceBetweenPoints);
                        cubes[i][j].transform.localScale = new Vector3(cubes[i][j].transform.localScale.x, cubes[i][j].transform.localScale.y / 4, cubes[i][j].transform.localScale.z);
                        cubes[i][j].AddComponent<TileContainer>();
                    }
                }
            }
            _oldWidth = width;

        }

        private void UpdateDepth()
        {
             if (depth < _oldDepth)
                        {
                            for(var i = _oldDepth-1; i >= depth; --i)
                            {
                                foreach (Transform item in Lines[i].transform)
                                {
                                    cubes[i].Remove(item.gameObject);
                                }
                                cubes.Remove(cubes[i]);
                                DestroyImmediate(Lines[i]);
                                Lines.Remove(Lines[i]);
                            }
                        }else if (depth > _oldDepth)
                        {
                            for (var i = _oldDepth; i < depth; ++i)
                            {
                                cubes.Add(new List<GameObject>());
                                Lines.Add(new GameObject("Line" + (i + 1)));
                                Lines[i].transform.SetParent(gameObject.transform);
                                for (var j = 0; j < width; j++)
                                {
                                    cubes[i].Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                                    cubes[i][j].transform.SetParent(Lines[i].transform);
                                    cubes[i][j].name = "Tile " + (j*(i+1));
                                    cubes[i][j].transform.position = new Vector3(j * distanceBetweenPoints, 0, i * distanceBetweenPoints);
                                    cubes[i][j].transform.localScale = new Vector3(cubes[i][j].transform.localScale.x, cubes[i][j].transform.localScale.y / 4, cubes[i][j].transform.localScale.z);
                                    cubes[i][j].AddComponent<TileContainer>();
            
                                }
                                 
                              
                            }
                        }
            
                        _oldDepth = depth;
        }


        private void UpdateDistance()
        {
            if (!(distanceBetweenPoints > 0.1f)) return;
            for(var i = 0; i < cubes.Count; ++i)
            {
                for(var j = 0; j < cubes[i].Count; ++j)
                {
                    var local = cubes[i][j].transform;
                    var localScale = local.localScale;
                    localScale = new Vector3(localScale.x / _oldDistance, localScale.y, localScale.z / _oldDistance);
                    localScale = new Vector3(localScale.x * distanceBetweenPoints, localScale.y, localScale.z* distanceBetweenPoints);
                    local.localScale = localScale;
                    var localPosition = local.localPosition;
                    var position = local.transform.position;
                    position = new Vector3(localPosition.x / _oldDistance, 0, localPosition.z /_oldDistance);
                    position = new Vector3(localPosition.x * distanceBetweenPoints, 0, localPosition.z * distanceBetweenPoints);
                    local.transform.position = position;
                }
            }

            _oldDistance = distanceBetweenPoints;

        }
        
        private void UpdateNeighbours()
        {
            
            Debug.Log("Update Grid");
           
            
           
            _neighbours = null;
            _neighbours = new GameObject[depth, width];
            for (var i = 0; i < cubes.Count; ++i)
            {
                for (var j = 0; j < cubes[i].Count; ++j)
                {
                    _neighbours[i, j] = cubes[i][j];
                }
            }




        }

    }
}
