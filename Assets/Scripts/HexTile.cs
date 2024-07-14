using Unity.VisualScripting;
using UnityEngine;

namespace TrapTheCat
{
    public class HexTile : MonoBehaviour
    {
        public Vector2Int gridPosition;
        public bool isWalkable = true;
        public bool isCornerTile = false;
        public bool hasCat { get; set; }
        public int GCost { get; set; } // Cost from start node
        public int HCost { get; set; } // Heuristic cost to end node
        public int FCost => GCost + HCost; // Total cost
        public HexTile Parent { get; set; } // For path retracing
        private SpriteRenderer _renderer;
        public void SetColor(Color color)=> _renderer.color = color;


        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
        void OnMouseDown()
        {
            if (isWalkable && !GameManager.Instance.isGameOver && !hasCat)
            {
                isWalkable = false;
                GetComponent<SpriteRenderer>().color = Color.black;
                GameManager.Instance.UpdateCatPath();
            }
        }
    }


}

