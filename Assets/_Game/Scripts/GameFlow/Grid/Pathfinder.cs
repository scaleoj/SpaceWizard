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
        

        public List<TileAttribute> FindPath(GameObject start, GameObject end)
        {
            
            //Implementierung A*
            var startX = 0;
            var startY = 0;
            var endX = 0;
            var endY = 0;

            var startBool = false;
            var endBool = false;

            
            foreach (var t in _grid.Cubes)
            {
                if (t.Contains(start))
                {
                    startX = _grid.Cubes.IndexOf(t);
                    startY = t.IndexOf(start);
                    startBool = true;
                }

                if (t.Contains(end))
                {
                    endX = _grid.Cubes.IndexOf(t);
                    endY = t.IndexOf(end);
                    endBool = true;
                }

                if (startBool && endBool)
                {
                    break;
                }             
            }
            

            var openList = new List<TileAttribute>();
            var closedList = new HashSet<TileAttribute>();
            
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
                    return GetFinalPath(_grid.Neighbours[startX, startY], _grid.Neighbours[endX, endY]);
                }
                foreach (var tiles in _grid.GetNeighboursTiles(currentTile.Node))
                {
                    if (tiles.walkable || closedList.Contains(tiles))
                    {
                        continue;
                    }

                    var moveCost = currentTile.G + GetManhattenDistance(currentTile, tiles);

                    if (moveCost >= tiles.G &&
                        openList.Contains(tiles)) continue;
                    tiles.G = moveCost;
                    tiles.H = GetManhattenDistance(tiles, _grid.Neighbours[endX, endY]);
                    tiles.Parent = currentTile;
                    if (!openList.Contains(tiles))
                    {
                        openList.Add(tiles);
                    }
                }
            }

            

            return null;
        }

        private int GetManhattenDistance(TileAttribute currentTile, TileAttribute tiles)
        {
            var ix = Mathf.Abs(currentTile.gridX -tiles.gridX);
            var iy = Mathf.Abs(currentTile.gridY -tiles.gridY);
            return ix + iy;
        }

        private List<TileAttribute> GetFinalPath(TileAttribute start, TileAttribute end)
        {
        
            
            var finalPath = new List<TileAttribute>();
            var currentTile = end;

            while (currentTile != start)
            {
                finalPath.Add(currentTile);
                currentTile = currentTile.Parent;
            }
            finalPath.Reverse();

            return finalPath;
        }
    }
}
