using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Game.Scripts.GameFlow;

public class RangeCalculator : MonoBehaviour
{
    /*
     * GetNeighbours returns GameObject[] with: left, right, top, bot, topLeft, topRight, bottomLeft, bottomRight
     */
    public static GameObject[] GetTilesInRange(int range, TileGrid tilegrid, GameObject anchor )
    {
        GameObject[] tilesInRange = new GameObject[range * range + (range + 1) * (range + 1) ];
        GameObject[] tiles = tilegrid.GetNeighbours(anchor);

        int j = 0;
        for (int i = 0; i < tilesInRange.Length; i++)
        {
            GameObject[] tilesToCopy = GetUniqueTiles(tiles, tilesInRange);
            if (tilesToCopy != null)
            {
                for (int z = 0; z < tilesToCopy.Length; z++)
                {
                    tilesInRange[j] = tilesToCopy[z];
                    j++;
                }    
            }
        }

        return tilesInRange;
    }

    private static GameObject[] GetUniqueTiles(GameObject[] tiles, GameObject[] tilesInRange)
    {
        GameObject[] uniqueTilesDummy = new GameObject[8];

        int j = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null && tiles[i].GetComponent<TileContainer>().Walkable == true)
            {
                for (int z = 0; z < tilesInRange.Length; i++)
                {
                    if (tiles[i] == tilesInRange[z])
                    {
                        break;
                    }
                    else
                    {
                        uniqueTilesDummy[j] = tiles[i];
                        j++;
                    }
                }     
            }
        }

        if (j == 0)
        {
            return null;
        }
        
        GameObject[] uniqueTiles = new GameObject[j];
        for (int t = 0; t < uniqueTiles.Length; t++)
        {
            uniqueTiles[t] = uniqueTilesDummy[t];
        }
        
        return uniqueTiles;
    }
}
