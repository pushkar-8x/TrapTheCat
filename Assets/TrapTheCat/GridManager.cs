using UnityEngine;
using System.Collections.Generic;

namespace Cat
{

    public class GridManager : MonoBehaviour
    {
        public GameObject hexTilePrefab;
        public int gridWidth;
        public int gridHeight;
        public float hexTileSize;
        public float spacing; // New variable for spacing between tiles
        public int nonWalkableTileCount;
        private Dictionary<Vector2Int, HexTile> grid;

        void Awake()
        {
            grid = new Dictionary<Vector2Int, HexTile>();
            GenerateGrid();
            SetRandomNonWalkableTiles();
        }

        void GenerateGrid()
        {
            float xOffset = hexTileSize * 0.75f + spacing;
            float yOffset = hexTileSize * Mathf.Sqrt(3) / 2 + spacing;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    float xPos = x * xOffset;
                    float yPos = y * yOffset;
                    if (x % 2 == 1)
                    {
                        yPos += yOffset / 2;
                    }

                    Vector2 position = new Vector2(xPos, yPos);
                    GameObject tile = Instantiate(hexTilePrefab, position, Quaternion.identity);
                    tile.name = $"Tile_{x}_{y}";
                    tile.transform.SetParent(transform);

                    HexTile hexTile = tile.GetComponent<HexTile>();
                    hexTile.gridPosition = new Vector2Int(x, y);
                    hexTile.isCornerTile = IsCornerTile(hexTile);
                    grid.Add(new Vector2Int(x, y), hexTile);
                }
            }
        }

        void SetRandomNonWalkableTiles()
        {
            List<HexTile> allTiles = new List<HexTile>(grid.Values);
            for (int i = 0; i < nonWalkableTileCount; i++)
            {
                if (allTiles.Count == 0)
                    break;

                int randomIndex = Random.Range(0, allTiles.Count);
                HexTile tile = allTiles[randomIndex];
                tile.isWalkable = false;
                tile.GetComponent<SpriteRenderer>().color = Color.black;
                allTiles.RemoveAt(randomIndex);
            }
        }

        public Vector2 HexToWorld(Vector2Int hex)
        {
            float x = hex.x * (hexTileSize * 0.75f + spacing);
            float y = hex.y * (hexTileSize * Mathf.Sqrt(3) / 2 + spacing);
            if (hex.x % 2 != 0)
            {
                y += (hexTileSize * Mathf.Sqrt(3) / 4 + spacing / 2);
            }
            return new Vector2(x, y);
        }

        public HexTile GetTileAtPosition(Vector2Int position)
        {
            if (grid.ContainsKey(position))
            {
                return grid[position];
            }
            return null;
        }

        public List<HexTile> GetEdgeTiles()
        {
            List<HexTile> edgeTiles = new List<HexTile>();
            foreach (var tile in grid.Values)
            {
                if (tile.gridPosition.x == 0 || tile.gridPosition.y == 0 || tile.gridPosition.x == gridWidth - 1 || tile.gridPosition.y == gridHeight - 1)
                {
                    edgeTiles.Add(tile);
                }
            }
            return edgeTiles;
        }

        public bool IsCornerTile(HexTile tile)
        {
            if (tile.gridPosition.x == 0 || tile.gridPosition.y == 0 || tile.gridPosition.x == gridWidth - 1 || tile.gridPosition.y == gridHeight - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<HexTile> GetAllTiles()
        {
            return new List<HexTile>(grid.Values);
        }

        public List<HexTile> GetWalkableTiles()
        {
            List<HexTile> walkableTiles = new List<HexTile>();
            foreach (var tile in grid.Values)
            {
                if (tile.isWalkable)
                {
                    walkableTiles.Add(tile);
                }
            }
            return walkableTiles;
        }

        private void OnDestroy()
        {
            grid.Clear();
            
        }
    }


}

