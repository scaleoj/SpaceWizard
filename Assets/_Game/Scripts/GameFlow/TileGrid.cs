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
    
    public List<GameObject> cubes;
    private GameObject[,] neighbours;

    private void Awake()
    {
        _oldWidth = width;
        _oldDepth = depth;
        var awakeCounter = 0;
        cubes = new List<GameObject>();
        neighbours = new GameObject[width,depth];
        for (var i = 0; i < depth; i++)
        {
            for (var j = 0; j < width; j++)
            {
                cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                neighbours[j, i] = cubes[awakeCounter];
                cubes[awakeCounter].transform.SetParent(gameObject.transform);
                cubes[awakeCounter].name = "Tile " + awakeCounter;
                cubes[awakeCounter].transform.position = new Vector3(j * distanceBetweenPoints, 0, i * distanceBetweenPoints);
                cubes[awakeCounter].transform.localScale = new Vector3(cubes[awakeCounter].transform.localScale.x, cubes[awakeCounter].transform.localScale.y / 4, cubes[awakeCounter].transform.localScale.z);
                ++awakeCounter;

            }
        }
    }

    private void Update()
    {
        if (_oldWidth == width)
        {
            if (_oldDepth == depth)
            {
                return;
            }
        }
        var updateCounter1 = 0;
        for (var i = 0; i < _oldDepth; ++i)
        {
            for (var j = 0; j < _oldWidth; ++j)
            {
                DestroyImmediate(cubes[updateCounter1]);
                ++updateCounter1;

            }
        }
        _oldDepth = depth;
        _oldWidth = width;

        cubes.Clear();
        neighbours = null;
        neighbours = new GameObject[width, depth];
        var updateCounter2 = 0;
        for (var i = 0; i < depth; ++i)
        {
            for (var j = 0; j < width; ++j)
            {
                cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                neighbours[j, i] = cubes[updateCounter2];
                cubes[updateCounter2].transform.SetParent(gameObject.transform);
                cubes[updateCounter2].name = "Tile " + updateCounter2;
                cubes[updateCounter2].transform.position = new Vector3(j * distanceBetweenPoints, 0, i * distanceBetweenPoints);
                cubes[updateCounter2].transform.localScale = new Vector3(cubes[updateCounter2].transform.localScale.x, cubes[updateCounter2].transform.localScale.y / 5, cubes[updateCounter2].transform.localScale.z);
                ++updateCounter2;

            }
        }
    }

    public GameObject[] getNeighbours(GameObject start)
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
                if (neighbours[j, i] != start) continue;
                left = j-1 < 0 ? null : neighbours[j - 1, i];
                bot = i-1 < 0 ? null : neighbours[j, i-1];
                right = j+1 > width ? null : neighbours[j + 1, i];
                top = i+1 > depth ? null : neighbours[j, i+1];
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
}
