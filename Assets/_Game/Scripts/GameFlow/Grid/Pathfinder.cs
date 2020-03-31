using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            foreach (var t in _grid.GetCubes())
            {
                if (t.Contains(start))
                {
                    startX = _grid.GetCubes().IndexOf(t);
                    startY = t.IndexOf(start);
                    startBool = true;
                }

                if (t.Contains(end))
                {
                    endX = _grid.GetCubes().IndexOf(t);
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
            
            openList.Add(_grid.GetNeighbours()[startX, startY]);
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
                    return GetFinalPath(_grid.GetNeighbours()[startX, startY], _grid.GetNeighbours()[endX, endY]);
                }

                var tilesInRange = _grid.GetNeighboursTiles(currentTile.Node);
                tilesInRange = tilesInRange.Where(c => c != null);
                
                foreach (var tiles in tilesInRange)
                {
                    if (tiles.node.layer != 9<<LayerMask.GetMask("Walkable") || closedList.Contains(tiles))
                    {
                        continue;
                    }

                    var moveCost = currentTile.G + GetManhattenDistance(currentTile, tiles);

                    if (moveCost >= tiles.G &&
                        openList.Contains(tiles)) continue;
                    tiles.G = moveCost;
                    tiles.H = GetManhattenDistance(tiles, _grid.GetNeighbours()[endX, endY]);
                    tiles.parent = currentTile;
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
                currentTile = currentTile.parent;
            }
            finalPath.Reverse();

            return finalPath;
        }
    }
}
