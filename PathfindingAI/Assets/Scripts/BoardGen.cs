using System.Collections.Generic;
using UnityEngine;

public class BoardGen : MonoBehaviour
{
    public Camera Camera;
    public FloorTile FloorTileGameObject;
    public int BoardWidth;
    public int BoardHeight;

    private Dictionary<int, FloorTile> _adjacencyList;
    private FloorTile[,] _board;
    private RaycastHit _hit;
    private FloorTile _startTile;
    private FloorTile _goalTile;
    private bool _goalToStart;

    //setting up the board and the connections
    private void Awake()
    {
        _adjacencyList = new Dictionary<int, FloorTile>();
        _board = new FloorTile[BoardWidth, BoardHeight];

        GenerateBoard();
        GenerateCardinalEdges();
        _goalToStart = false;
        _startTile = null;
        _goalTile = null;
    }

    private void Update()
    {
        //left click changes a tile from active to deactive
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 16.0f);

            if (hit.collider != null)
            {
                var currentTile = hit.collider.gameObject.GetComponent<FloorTile>();

                if (_board[currentTile.Xlocation, currentTile.Ylocation].Equals(_startTile))
                    _startTile = null;

                if (_board[currentTile.Xlocation, currentTile.Ylocation].Equals(_goalTile))
                    _goalTile = null;

                //changing the type and updating adjacency list
                if (_board[currentTile.Xlocation, currentTile.Ylocation].type != 0)
                {
                    _board[currentTile.Xlocation, currentTile.Ylocation].type = 0;
                    RemoveCardinalEdges(currentTile.Xlocation, currentTile.Ylocation);
                }
                else
                {
                    _board[currentTile.Xlocation, currentTile.Ylocation].type = 1;
                    AddCardinalEdges(currentTile.Xlocation, currentTile.Ylocation);
                }

                _board[currentTile.Xlocation, currentTile.Ylocation].UpdateSprite();
            }
        }

        //right click changes the start and goal tiles
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 16.0f);

            if (hit.collider != null)
            {
                var currentTile = hit.collider.gameObject.GetComponent<FloorTile>();

                if (_startTile != null && _goalTile != null)
                {
                    if (!_goalToStart)
                    {
                        _startTile.type = 1;
                        _startTile.UpdateSprite();
                        _board[currentTile.Xlocation, currentTile.Ylocation].type = 2;
                        _startTile = _board[currentTile.Xlocation, currentTile.Ylocation];
                        _startTile.UpdateSprite();
                        _goalToStart = true;
                    }
                    else
                    {
                        _goalTile.type = 1;
                        _goalTile.UpdateSprite();
                        _board[currentTile.Xlocation, currentTile.Ylocation].type = 3;
                        _goalTile = _board[currentTile.Xlocation, currentTile.Ylocation];
                        _goalTile.UpdateSprite();
                        _goalToStart = false;
                    }
                }

                if (_startTile == null)
                {
                    _board[currentTile.Xlocation, currentTile.Ylocation].type = 2;
                    _startTile = _board[currentTile.Xlocation, currentTile.Ylocation];
                    _startTile.UpdateSprite();
                    _goalToStart = true;
                    return;
                }

                if (_goalTile == null)
                {
                    _board[currentTile.Xlocation, currentTile.Ylocation].type = 3;
                    _goalTile = _board[currentTile.Xlocation, currentTile.Ylocation];
                    _goalTile.UpdateSprite();
                    _goalToStart = false;
                }
            }
        }
    }

    //Generates the board and populates the adjacency list with vertices
    private void GenerateBoard()
    {
        Vector2 boardLocation = gameObject.transform.position;
        var currentLocation = boardLocation + new Vector2(-1.0f * (BoardWidth / 2.0f) + 0.5f, BoardHeight / 2.0f - 0.5f);
        var numNodes = 0;

        for (var x = 0; x < BoardWidth; x++)
        {
            for (var y = 0; y < BoardHeight; y++)
            {
                _board[x,y] = Instantiate(FloorTileGameObject, currentLocation, Quaternion.identity, transform);
                currentLocation = new Vector2(currentLocation.x, currentLocation.y - 1.0f);
                _board[x, y].ActivateTile(numNodes, x, y);
                AddVertex(numNodes, _board[x, y]);
                numNodes++;
            }

            currentLocation = new Vector2(currentLocation.x + 1, BoardHeight / 2.0f - 0.5f);
        }
    }

    //sets up adjacency list with North, East, South, West
    private void GenerateCardinalEdges()
    {
        //x
        for (int i = 0; i < BoardWidth; i++)
        {
            //y
            for (int j = 0; j < BoardHeight; j++)
            {
                AddCardinalEdges(i, j);
            }
        }
    }
    
    //adds an instance of id to the graph
    private void AddVertex(int id, FloorTile tile)
    {
        if (!_adjacencyList.ContainsKey(id))
        {
            _adjacencyList.Add(id, tile);
            Debug.Log("G(" + id + ")");
        }
        else
            Debug.Log("Vertex " + id + " is already in graph");
    }

    //Removes an instance of id to the graph
    private void RemoveVertex(int id)
    {
        if (_adjacencyList.ContainsKey(id))
        {
            _adjacencyList.Remove(id);
            Debug.Log(" Removed G(" + id + ")");
        }
        else
            Debug.Log("Vertex " + id + " is not in graph");
    }

    //Adds a new, directed edge to the graph that connects two vertices.
    private void AddEdge(int fromVert, int toVert)
    {
        if (_adjacencyList.ContainsKey(fromVert) && _adjacencyList.ContainsKey(toVert))
        {
            _adjacencyList[fromVert].Adj.Add(toVert, 1.0f);
            Debug.Log("G(" + fromVert + "," + toVert + ")");
        }
        else
            Debug.Log("Vertices " + fromVert + " to " + toVert + " does not exits");
    }

    /*
    //Adds a new, weighted, directed edge to the graph that connects two vertices.
    private void AddEdge(int fromVert, int toVert, float weight)
    {
        if (_adjacencyList.ContainsKey(fromVert) && _adjacencyList.ContainsKey(toVert))
        {
            _adjacencyList[fromVert].Adj.Add(toVert, weight);
            Debug.Log("G(" + fromVert + "," + toVert + "," + weight + ")");
        }
        else
            Debug.Log("Vertices " + fromVert + " to " + toVert + " does not exits");
    }
    */


    private void RemoveEdge(int fromVert, int toVert)
    {
        if (_adjacencyList.ContainsKey(fromVert) && _adjacencyList.ContainsKey(toVert))
        {
            _adjacencyList[fromVert].Adj.Remove(toVert);
            Debug.Log("Removed G(" + fromVert + "," + toVert + ")");
        }
        else
            Debug.Log("Cannot remove vertices " + fromVert + " to " + toVert);
    }

    //finds the vertex in the graph named vertKey
    private List<float> Neighbors(int vertKey)
    {
        if (_adjacencyList.ContainsKey(vertKey))
        {
            return new List<float>(_adjacencyList[vertKey].Adj.Values);
        }
        else
        {
            Debug.Log("Vertex " + vertKey + " was not found");
            return new List<float>();
        }
    }

    private float GetEdgeValue(int fromVert, int toVert)
    {
        if (_adjacencyList.ContainsKey(fromVert) && _adjacencyList.ContainsKey(toVert))
        {
            return _adjacencyList[fromVert].Adj[toVert];
        }
        else
        {
            Debug.Log("Vertices " + fromVert + " to " + toVert + " does not exits");
            return -1.0f;
        }
    }

    private void AddCardinalEdges(int x, int y)
    {
        //12 o'clock
        if ((y - 1) >= 0)
            if (_board[x, y - 1].type != 0)
                AddEdge(_board[x, y].Id, _board[x, y - 1].Id);

        //3 o'clock
        if ((x + 1) < BoardWidth)
            if (_board[x + 1, y].type != 0)
                AddEdge(_board[x, y].Id, _board[x + 1, y].Id);

        //6 o'clock
        if ((y + 1) < BoardHeight)
            if (_board[x, y + 1].type != 0)
                AddEdge(_board[x, y].Id, _board[x, y + 1].Id);

        //9 o'clock
        if ((x - 1) >= 0)
            if (_board[x - 1, y].type != 0)
                AddEdge(_board[x, y].Id, _board[x - 1, y].Id);
    }

    private void RemoveCardinalEdges(int x, int y)
    {
        //12 o'clock
        if ((y - 1) >= 0)
        {
            RemoveEdge(_board[x, y].Id, _board[x, y - 1].Id);
            RemoveEdge(_board[x, y - 1].Id, _board[x, y].Id);
        }

        //3 o'clock
        if ((x + 1) < BoardWidth)
        {
            RemoveEdge(_board[x, y].Id, _board[x + 1, y].Id);
            RemoveEdge(_board[x + 1, y].Id, _board[x, y].Id);
        }

        //6 o'clock
        if ((y + 1) < BoardHeight)
        {
            RemoveEdge(_board[x, y].Id, _board[x, y + 1].Id);
            RemoveEdge(_board[x, y + 1].Id, _board[x, y].Id);
        }

        //9 o'clock
        if ((x - 1) >= 0)
        {
            RemoveEdge(_board[x, y].Id, _board[x - 1, y].Id);
            RemoveEdge(_board[x - 1, y].Id, _board[x, y].Id);
        }
    }
}
