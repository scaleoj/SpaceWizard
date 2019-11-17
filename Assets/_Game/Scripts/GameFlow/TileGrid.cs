using System.Collections.Generic;
using UnityEngine;

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
    
    public List<GameObject> cubes;
    private List<GameObject> Lines;
    private GameObject[,] _neighbours;

    private void Awake()
    {
        cubes = new List<GameObject>();
        Lines = new List<GameObject>();
        if (transform.childCount > 0)
        {
            depth = transform.childCount;
            width = transform.GetChild(0).childCount;
        }
        _neighbours = new GameObject[width,depth];
        _oldWidth = width;
        _oldDepth = depth;

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
        if (_oldWidth == width)
        {
            if (_oldDepth == depth)
            {

                if (_oldDistance + 0.01f >= distanceBetweenPoints && _oldDistance - 0.01f <= distanceBetweenPoints)
                {
                    
                    return;
                }

            }
        }
        ClearGrid();
        BuildGrid();
    }

    //O(n^2 + c)
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
    
    //O(n^2 + c)
    private void BuildGrid()
    {
        var buildCounter = 0;
        for (var i = 0; i < depth; i++)
        {
            Lines.Add(new GameObject("Line" + (i + 1)));
            Lines[i].transform.SetParent(gameObject.transform);
            for (var j = 0; j < width; j++)
            {
                cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                _neighbours[j, i] = cubes[buildCounter];
                cubes[buildCounter].transform.SetParent(Lines[i].transform);
                cubes[buildCounter].name = "Tile " + buildCounter;
                cubes[buildCounter].transform.position = new Vector3(j * distanceBetweenPoints, 0, i * distanceBetweenPoints);
                cubes[buildCounter].transform.localScale = new Vector3(cubes[buildCounter].transform.localScale.x, cubes[buildCounter].transform.localScale.y / 4, cubes[buildCounter].transform.localScale.z);
                cubes[buildCounter].AddComponent<TileContainer>();
                ++buildCounter;

            }
        } 
    }

    //O(n^2 + c)
    private void ClearGrid()
    {
        var clearCounter = 0;
        for (var i = 0; i < _oldDepth; ++i)
        {
            for (var j = 0; j < _oldWidth; ++j)
            {
                DestroyImmediate(cubes[clearCounter]);
                DestroyImmediate(Lines[i]);
                ++clearCounter;

            }
        }
        _oldDepth = depth;
        _oldWidth = width;
        _oldDistance = distanceBetweenPoints;
        cubes.Clear();
        Lines.Clear();
        _neighbours = null;
        _neighbours = new GameObject[width, depth];
    }
    
    //O(n^2 + c)
    private void ScanGrid()
    {
        for (var i = 0; i < depth; ++i)
        {
            Lines.Add(gameObject.transform.GetChild(i).gameObject);
            for (var j = 0; j < width; ++j)
            {
                cubes.Add(Lines[i].transform.GetChild(j).gameObject);
                _neighbours[j, i] = cubes[i+j];
                var script = cubes[i + j].GetComponent<TileContainer>();
                if (script != null)
                {
                    cubes[i + j].AddComponent<TileContainer>();
                }
                
            }
        }
        

    }

}
