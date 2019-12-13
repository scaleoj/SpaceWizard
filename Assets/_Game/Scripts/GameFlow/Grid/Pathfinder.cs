using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

namespace _Game.Scripts.GameFlow.Grid
{
    public class Pathfinder
    {
        private readonly TileGrid _grid;

        public Pathfinder(TileGrid tileGrid)
        {
            _grid = tileGrid;
        }
        

        public List<GameObject> FindPath(GameObject start, GameObject end)
        {
            
            //Implementierung A*

            var startScript = start.GetComponent<TileAttributes>();
            var startX = startScript.gridX;
            var startY = startScript.gridY;

            var endScript = end.GetComponent<TileAttributes>();
            var endX = endScript.gridX;
            var endY = endScript.gridY;

            var openList = new List<TileAttributes>();
            var closedList = new HashSet<TileAttributes>();
            
            openList.Add(_grid.Neighbours[startX, startY]);
            while (openList.Count > 0)
            {
                var currentTile = openList[0];
                for (var i = 1; i < openList.Count; ++i)
                {
                    if (openList[i].F <= currentTile.F && openList[i].H == currentTile.H)
                    {
                        currentTile = openList[i];
                    }
                }

                openList.Remove(currentTile);
                closedList.Add(currentTile);

                if (currentTile.Node == end)
                {
                    return GetFinalPath(start, end);
                }
                foreach (var tiles in _grid.GetNeighboursTiles(currentTile.Node))
                {
                    if (tiles.GetComponent<TileAttributes>().walkable || closedList.Contains(tiles.GetComponent<TileAttributes>()))
                    {
                        continue;
                    }

                    var moveCost = currentTile.G + GetManhattenDistance(currentTile, tiles);

                    if (moveCost >= tiles.GetComponent<TileAttributes>().G &&
                        openList.Contains(tiles.GetComponent<TileAttributes>())) continue;
                    tiles.GetComponent<TileAttributes>().G = moveCost;
                    tiles.GetComponent<TileAttributes>().H = GetManhattenDistance(tiles.GetComponent<TileAttributes>(), end);
                    tiles.GetComponent<TileAttributes>().Parent = currentTile;
                    if (!openList.Contains(tiles.GetComponent<TileAttributes>()))
                    {
                        openList.Add(tiles.GetComponent<TileAttributes>());
                    }
                }
            }

            

            return null;
        }

        private int GetManhattenDistance(TileAttributes currentTile, GameObject tiles)
        {
            var ix = Mathf.Abs(currentTile.gridX -tiles.GetComponent<TileAttributes>().gridX);
            var iy = Mathf.Abs(currentTile.gridY -tiles.GetComponent<TileAttributes>().gridY);
            return ix + iy;
        }

        private List<GameObject> GetFinalPath(GameObject start, GameObject end)
        {
        
            
            var finalPath = new List<GameObject>();
            var currentTile = end.GetComponent<TileAttributes>().Node;

            while (currentTile != start.GetComponent<TileAttributes>().Node)
            {
                finalPath.Add(currentTile);
                currentTile = currentTile.GetComponent<TileAttributes>().Parent.node;
            }
            finalPath.Reverse();

            return finalPath;
        }
    }
}
