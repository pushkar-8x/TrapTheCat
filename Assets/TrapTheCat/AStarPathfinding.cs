using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat
{
    public class AStarPathfinding
    {
        public static List<HexTile> FindPath(HexTile start, HexTile target, GridManager gridManager)
        {
            List<HexTile> openSet = new List<HexTile>();
            HashSet<HexTile> closedSet = new HashSet<HexTile>();
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                HexTile current = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < current.FCost || openSet[i].FCost == current.FCost && openSet[i].HCost < current.HCost)
                    {
                        current = openSet[i];
                    }
                }

                openSet.Remove(current);
                closedSet.Add(current);


                if (current == target)
                {
                    return RetracePath(start, target);
                }

                foreach (HexTile neighbor in GetNeighbors(current, gridManager))
                {
                    if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newCostToNeighbor = current.GCost + GetDistance(current, neighbor);
                    if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, target);
                        neighbor.Parent = current;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
            return null;
        }

        static List<HexTile> RetracePath(HexTile start, HexTile end)
        {
            List<HexTile> path = new List<HexTile>();
            HexTile current = end;

            while (current != start)
            {
                path.Add(current);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }

        public static int GetDistance(HexTile a, HexTile b)
        {
            int dx = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
            int dy = Mathf.Abs(a.gridPosition.y - b.gridPosition.y);
            return dx + dy;
        }

        public static List<HexTile> GetNeighbors(HexTile tile, GridManager gridManager)
        {
            List<HexTile> neighbors = new List<HexTile>();
            Vector2Int[] directions = new Vector2Int[]
            {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            //new Vector2Int(1, 1),
            //new Vector2Int(-1, -1),
        
            };

            foreach (var direction in directions)
            {
                HexTile neighbor = gridManager.GetTileAtPosition(tile.gridPosition + direction);
                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }
    }

}

