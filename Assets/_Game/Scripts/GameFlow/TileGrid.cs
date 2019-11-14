using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class TileGrid : MonoBehaviour
{
    [Header("The width of the Grid")]
    [SerializeField]
    private int _width = 10;
    [Header("The Depth of the Grid")]
    [SerializeField]
    private int _depth = 10;
    [SerializeField]
    private float distanceBetweenPoints = 1f;
    
    public List<GameObject> cubes;

    void Awake()
    {
        int k = 0;
        cubes = new List<GameObject>();
        for (int i = -_depth/2; i < _depth/2; i++)
        {
            for (int j = -_width/2; j < _width/2; j++)
            {
                cubes.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
                cubes[k].transform.position = new Vector3(j * distanceBetweenPoints, 0, i * distanceBetweenPoints);
                cubes[k].transform.localScale = new Vector3(cubes[k].transform.localScale.x, cubes[k].transform.localScale.y / 4, cubes[k].transform.localScale.z);
                ++k;

            }
        }
    }

}
