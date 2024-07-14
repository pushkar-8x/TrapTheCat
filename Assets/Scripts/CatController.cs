using System.Collections.Generic;
using UnityEngine;

namespace TrapTheCat
{
    public class CatController : MonoBehaviour
    {
        public GridManager gridManager;
        private HexTile currentTile;
        private List<HexTile> currentPath;
        private int ATTEMPTS = 30;
        void Start()
        {
            // Find a random walkable tile to start on
            currentTile = GetRandomWalkableTile();
            if (currentTile != null)
            {
                currentTile.hasCat = true;
                transform.position = currentTile.transform.position;
            }
        }

        public void MoveCat()
        {
            if (currentTile.isCornerTile)
            {
                Debug.Log("Escaped !");
                GameManager.Instance.GameOver("Oops ! The Cat has escaped !!");
                currentTile.SetColor(Color.green);
                return;
            }
            HexTile targetTile = FindClosestEdgeTile();
            List<HexTile> path = AStarPathfinding.FindPath(currentTile, targetTile, gridManager);
            currentTile.hasCat = false;
            //ClearPathHighlights();
            

            if (path != null && path.Count > 0)
            {
                HexTile nextTile = path[0];
                currentTile = nextTile;
                transform.position = nextTile.transform.position;
                nextTile.hasCat = true;
                
                //HighlightPath(path);
            }
            else
            {
                // If no valid path, move to a random walkable neighbor
                List<HexTile> neighbors = AStarPathfinding.GetNeighbors(currentTile, gridManager);
                neighbors.RemoveAll(tile => !tile.isWalkable);

                if (neighbors.Count > 0)
                {
                    HexTile nextTile = neighbors[Random.Range(0, neighbors.Count)];
                    currentTile = nextTile;
                    transform.position = nextTile.transform.position;
                }
                else
                {
                    currentTile.SetColor(Color.red);
                    GameManager.Instance.GameOver("Cat got trapped !!");
                }
            }
        }

        HexTile FindClosestEdgeTile()
        {
            HexTile closestEdgeTile = null;
            int minDistance = int.MaxValue;

            foreach (var tile in gridManager.GetEdgeTiles())
            {
                if (tile.isWalkable)
                {
                    int distance = AStarPathfinding.GetDistance(currentTile, tile);
                    if (distance < minDistance)
                    {
                        closestEdgeTile = tile;
                        minDistance = distance;
                    }
                }
            }

            return closestEdgeTile;
        }

        HexTile GetRandomWalkableTile()
        {
            List<HexTile> walkableTiles = gridManager.GetWalkableTiles();

            if (walkableTiles.Count > 0)
            {
                for (int i = 0; i < ATTEMPTS; i++)
                {
                    
                    HexTile randomTile =  walkableTiles[Random.Range(0, walkableTiles.Count)];
                    if(!randomTile.isCornerTile)
                    {
                        return randomTile;
                    }
                }
            }


            return null;
        }

        void HighlightPath(List<HexTile> path)
        {
            currentPath = path;
            foreach (var tile in path)
            {
                tile.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }

        void ClearPathHighlights()
        {
            if (currentPath == null) return;

            foreach (var tile in currentPath)
            {
                if (tile.isWalkable)
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }

            currentPath = null;
        }
    }


}
