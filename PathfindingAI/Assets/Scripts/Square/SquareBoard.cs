using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Square
{
    public class SquareBoard : MonoBehaviour
    {
        public SquareTile Tile;
        public int Width;
        public int Height;
        public bool Rest;

        private SquareTile[,] _board;
        private int _mouseClick;
        private Vector2Int _startCoord;
        private Vector2Int _endCoord;
        private bool _doneReseting;

        // Use this for initialization
        private void Awake ()
        {
            GenerateSquareBoard();
            StartAndEndTiles();
            Rest = false;
            _doneReseting = true;
        }

        private void Update()
        {
            // if mouse clicked 
            if (Input.GetMouseButtonDown(0))
            {
                // Get the tile the mouse is currently above
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    Transform objectHit = hit.transform;
                    Vector2Int coord = new Vector2Int((int) (objectHit.position.x + (Width * 0.5f - 0.5f)),
                        (int) (Height - (objectHit.position.y + (Height * 0.5f + 0.5f))));

                    // Based on tile selection, edit mode will know which tiles to change
                    switch (_board[coord.x, coord.y].GetKind())
                    {
                        //wall to empty
                        case 0:
                            _mouseClick = 1;
                            _board[coord.x, coord.y].SetKind(1);
                            break;
                        //empty to wall
                        case 1:
                            _mouseClick = 0;
                            _board[coord.x, coord.y].SetKind(0);
                            break;
                        //Start to potentialy move
                        case 2:
                            _mouseClick = 2;
                            break;
                        //Goal to potentialy move
                        case 3:
                            _mouseClick = 3;
                            break;
                        //In case the tile was not set
                        default:
                            Debug.Log("Tile not set");
                            break;
                    }

                }
                //In case the tile is not there
                else
                {
                    Debug.Log("Missed");
                }

                //return because we do not want to ossolate on the next if statment
                return;
            }
            
            //If mouse is held down
            if (Input.GetMouseButton(0))
            {
                // Get the tile the mouse is currently above
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                //When the mouse is held down, we are in edit mode
                if (hit.collider != null)
                {
                    Transform objectHit = hit.transform;
                    Vector2Int coord = new Vector2Int((int)(objectHit.position.x + (Width * 0.5f - 0.5f)),
                        (int)(Height - (objectHit.position.y + (Height * 0.5f + 0.5f))));

                    //based on the tile type when the mouse was clicked
                    switch (_mouseClick)
                    {
                        //wall painting
                        case 0:
                            if (_board[coord.x, coord.y].GetKind() == 2 || _board[coord.x, coord.y].GetKind() == 3)
                                return;
                            _board[coord.x, coord.y].SetKind(0);
                            break;
                        //empty
                        case 1:
                            if (_board[coord.x, coord.y].GetKind() == 2 || _board[coord.x, coord.y].GetKind() == 3)
                                return;
                            _board[coord.x, coord.y].SetKind(1);
                            break;
                        //start
                        case 2:
                            if (_board[coord.x, coord.y].GetKind() == 0 || _board[coord.x, coord.y].GetKind() == 3)
                                return;
                            _board[_startCoord.x, _startCoord.y].SetKind(1);
                            _board[coord.x, coord.y].SetKind(2);
                            _startCoord = new Vector2Int(coord.x, coord.y);
                            break;
                        //goal
                        case 3:
                            if (_board[coord.x, coord.y].GetKind() == 0 || _board[coord.x, coord.y].GetKind() == 2)
                                return;
                            _board[_endCoord.x, _endCoord.y].SetKind(1);
                            _board[coord.x, coord.y].SetKind(3);
                            _endCoord = new Vector2Int(coord.x, coord.y);
                            break;
                        //In case the tile was not set
                        default:
                            Debug.Log("Edit mode not set");
                            break;
                    }
                }
                //In case the tile is not there
                else
                {
                    Debug.Log("Missed");
                }
                
            }
           
            if (Rest && _doneReseting)
            {
                _doneReseting = false;
                ResetBoard();
                _doneReseting = true;
                Rest = false;
            }
        }

        private void GenerateSquareBoard()
        {
            _board = new SquareTile[Width, Height];

            Vector2 boardLocation = gameObject.transform.position;
            Vector2 currentLocation = boardLocation + new Vector2(-1.0f * Width * 0.5f + 0.5f, Height * 0.5f - 0.5f);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _board[x, y] = Instantiate(Tile, currentLocation, Quaternion.identity, transform);
                    currentLocation = new Vector2(currentLocation.x, currentLocation.y - 1.0f);
                }
                currentLocation = new Vector2(currentLocation.x + 1.0f, Height * 0.5f - 0.5f);
            }
        }

        private void StartAndEndTiles()
        {
            if (_board.Length <= 0)
                return;

            _board[Mathf.RoundToInt(Width * 0.5f - 3), Mathf.RoundToInt(Height * 0.5f)].SetKind(2);
            _startCoord = new Vector2Int(Mathf.RoundToInt(Width * 0.5f - 3), Mathf.RoundToInt(Height * 0.5f));
            _board[Mathf.RoundToInt(Width * 0.5f + 3), Mathf.RoundToInt(Height * 0.5f)].SetKind(3);
            _endCoord = new Vector2Int(Mathf.RoundToInt(Width * 0.5f + 3), Mathf.RoundToInt(Height * 0.5f));
        }

        private bool Adjacent(Vector2Int from, Vector2Int to)
        {
            //are the nodes inside the 2D array
            if (from.x >= 0 && from.x < Width && from.y >= 0 && from.y < Height && to.x >= 0 && to.x < Width &&
                to.y >= 0 && to.y < Height)
            {
                //if the node is itself then it is not adjacent to itself
                if (from.x == to.x && from.y == to.y)
                    return false;

                //north
                if (from.x == to.x && (from.y - 1) == to.y)
                    return true;

                //east
                if ((from.x + 1) == to.x && from.y == to.y)
                    return true;

                //south
                if (from.x == to.x && (from.y + 1) == to.y)
                    return true;

                //west
                if ((from.x - 1) == to.x && from.y == to.y)
                    return true;
            }

            return false;
        }

        private void ResetBoard()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (_board[x, y].GetKind() == 0 || _board[x, y].GetKind() == 2 || _board[x, y].GetKind() == 3)
                        continue;

                    _board[x, y].SetKind(1);
                }
            }
        }

        public Vector2Int GetStartCoord()
        {
            return _startCoord;
        }

        public Vector2Int GetGoalCoord()
        {
            return _endCoord;
        }

        public List<Vector2Int> Neighbors(Vector2Int coord)
        {
            List<Vector2Int> neighborsTiles = new List<Vector2Int>();
            //North
            if ((coord.y - 1) >= 0)
            {
                if(_board[coord.x, coord.y - 1].GetKind() != 0)
                    neighborsTiles.Add(new Vector2Int(coord.x, coord.y - 1));
            }

            //East
            if ((coord.x + 1) < Width)
            {
                if (_board[coord.x + 1, coord.y].GetKind() != 0)
                    neighborsTiles.Add(new Vector2Int(coord.x + 1, coord.y));
            }

            //South
            if ((coord.y + 1) < Height)
            {
                if (_board[coord.x, coord.y + 1].GetKind() != 0)
                    neighborsTiles.Add(new Vector2Int(coord.x, coord.y + 1));
            }

            //West
            if ((coord.x - 1) >= 0)
            {
                if (_board[coord.x - 1, coord.y].GetKind() != 0)
                    neighborsTiles.Add(new Vector2Int(coord.x - 1, coord.y));
            }

            return neighborsTiles;
        }

        public bool IsStart(Vector2Int coord)
        {
            return _board[coord.x, coord.y].GetKind() == 2;
        }

        public bool IsGoal(Vector2Int coord)
        {
            return _board[coord.x, coord.y].GetKind() == 3;
        }

        public void UpdateKind(Vector2Int coord, int num)
        {
            _board[coord.x, coord.y].SetKind(num);
        }
    }
}
