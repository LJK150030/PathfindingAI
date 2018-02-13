using UnityEngine;

namespace Assets.Scripts.Hexagons
{
    public class HexagonBoard : MonoBehaviour {
        public HexagonTile Tile;
        public float TileRadius;
        public int BoardWidth;
        public int BoardHeight;
        public bool Rest;

        private HexagonTile[,] _board;
        private int _mouseClick;
        private Vector2Int _startCoord;
        private Vector2Int _endCoord;
        private bool _doneReseting;
        private bool _inUI;

        // Use this for initialization
        private void Awake()
        {
            GenerateHexBoard();
            //StartAndEndTiles();
            Rest = false;
            _doneReseting = true;
            _inUI = false;
        }

        private void GenerateHexBoard()
        {
            _board = new HexagonTile[BoardWidth, BoardHeight];
            float tileWidth = TileRadius * 2.0f;
            float tileHeight = Mathf.Sqrt(3.0f) * 0.5f * tileWidth;
            float horizSpacing = tileWidth * 0.75f;
            float vertSpacing = tileHeight;

            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    
                }
            }
        }

        //cube to oddq(cube coord)
        private Vector2 CubeToAxis(Vector3 cube)
        {
            float col = cube.x;
            float row = cube.z + ((cube.x - (cube.x % 2.0f)) / 2.0f);
            return new Vector2(col, row);
        }

        //oddq to cube(hex coord)
        private Vector3 AxisToCube(Vector2 coord)
        {
            float x = coord.x;
            float z = coord.y - (coord.x - (coord.x % 2)) / 2.0f;
            float y = -x - z;
            return new Vector3(x, y, z);
        }

    }
}
