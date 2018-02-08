using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public Camera Camera;
    public VertexTile VertexTileGameObject;
    public int BoardWidth;
    public int BoardHeight;
    public bool UpToDate;
    [HideInInspector] public bool Ready;

    private Dictionary<Vector2Int, VertexTile> _adjacencyList;

    // Use this for initialization
    private void Awake () {
        _adjacencyList = new Dictionary<Vector2Int, VertexTile>();
        GenerateVertices();
        //GenerateCardinalEdges();
        Camera.orthographicSize = Mathf.Max(BoardWidth / 3.5f, BoardHeight / 2.0f);
        UpToDate = false;
        Ready = false;
    }

    private void Update()
    {
        if (UpToDate)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    var fromCoord = new Vector2Int(x, y);

                    //12 o'clock
                    if ((y - 1) >= 0)
                    {
                        var toCoord = new Vector2Int(x, y - 1);
                        if(_adjacencyList[toCoord].Type != 1)
                            AddEdge(fromCoord, toCoord);
                    }

                    //3 o'clock
                    if ((x + 1) < BoardWidth)
                    {
                        var toCoord = new Vector2Int(x + 1, y);
                        if (_adjacencyList[toCoord].Type != 1)
                            AddEdge(fromCoord, toCoord);
                    }

                    //6 o'clock
                    if ((y + 1) < BoardHeight)
                    {
                        var toCoord = new Vector2Int(x, y + 1);
                        if (_adjacencyList[toCoord].Type != 1)
                            AddEdge(fromCoord, toCoord);
                    }

                    //9 o'clock
                    if ((x - 1) >= 0)
                    {
                        var toCoord = new Vector2Int(x - 1, y);
                        if (_adjacencyList[toCoord].Type != 1)
                            AddEdge(fromCoord, toCoord);
                    }
                }
            }
        }
    }

    //Generates the board and populates the adjacency list with vertices
    private void GenerateVertices()
    {
        Vector2 boardLocation = gameObject.transform.position;
        var currentLocation = boardLocation + new Vector2(-1.0f * BoardWidth * 0.5f + 0.5f, BoardHeight * 0.5f - 0.5f);
        var numNodes = 0;

        for (var x = 0; x < BoardWidth; x++)
        {
            for (var y = 0; y < BoardHeight; y++)
            {
                var coord = new Vector2Int(x, y);
                AddVertex(coord, Instantiate(VertexTileGameObject, currentLocation, Quaternion.identity, transform), numNodes);
                currentLocation = new Vector2(currentLocation.x, currentLocation.y - 1.0f);
                numNodes++;
            }
            currentLocation = new Vector2(currentLocation.x + 1.0f, BoardHeight * 0.5f - 0.5f);
        }
    }

    //adds an instance of id to the graph
    private void AddVertex(Vector2Int position, VertexTile tile, int id)
    {
        if (!_adjacencyList.ContainsKey(position))
        {
            _adjacencyList.Add(position, tile);
            _adjacencyList[position].ActivateTile(id, position.x, position.y);
            Debug.Log("G(" + id + ")");
        }
        else
            Debug.Log("Vertex " + id + " is already in graph");
    }

    //sets up adjacency list with North, East, South, West
    private void GenerateCardinalEdges()
    {
        //var actualWidth = BoardWidth * 2 - 1;
        //var actualHeight = BoardHeight * 2 - 1;
        //Vector2 boardLocation = gameObject.transform.position;
        //var currentLocation = boardLocation + new Vector2(-1.0f * actualWidth * 0.5f + 0.5f, actualHeight * 0.5f - 0.5f);

        for (int x = 0; x < BoardWidth; x++)
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                var fromCoord = new Vector2Int(x, y);

                //12 o'clock
                if ((y - 1) >= 0)
                {
                    var toCoord = new Vector2Int(x, y - 1);
                    AddEdge(fromCoord, toCoord);
                }

                //3 o'clock
                if ((x + 1) < BoardWidth)
                {
                    var toCoord = new Vector2Int(x + 1, y);
                    AddEdge(fromCoord, toCoord);
                }

                //6 o'clock
                if ((y + 1) < BoardHeight)
                {
                    var toCoord = new Vector2Int(x, y + 1);
                    AddEdge(fromCoord, toCoord);
                }

                //9 o'clock
                if ((x - 1) >= 0)
                {
                    var toCoord = new Vector2Int(x - 1, y);
                    AddEdge(fromCoord, toCoord);
                }
            }
        }
    }

    //Adds a new, directed edge to the graph that connects two vertices.
    private void AddEdge(Vector2Int fromVert, Vector2Int toVert)
    {
        if (_adjacencyList.ContainsKey(fromVert) && _adjacencyList.ContainsKey(toVert))
        {
            _adjacencyList[fromVert].Adj.Add(toVert, 1.0f);
            Debug.Log("G(" + _adjacencyList[fromVert].Id + "," + _adjacencyList[toVert].Id + ")");
        }
        else
            Debug.Log("Vertices " + _adjacencyList[fromVert].Id + " to " + _adjacencyList[toVert].Id + " does not exits");
    }

    
    
    //Removes an instance of id to the graph
    private void RemoveVertex(Vector2Int position)
    {
        if (_adjacencyList.ContainsKey(position))
        {
            _adjacencyList.Remove(position);
            Debug.Log(" Removed G(" + position + ")");
        }
        else
            Debug.Log("Vertex " + position + " is not in graph");
    }
    
    //Adds a new, weighted, directed edge to the graph that connects two vertices.
    private void AddEdge(Vector2Int fromVert, Vector2Int toVert, float weight)
    {
        if (_adjacencyList.ContainsKey(fromVert) && _adjacencyList.ContainsKey(toVert))
        {
            _adjacencyList[fromVert].Adj.Add(toVert, weight);
            Debug.Log("G(" + fromVert + "," + toVert + "," + weight + ")");
        }
        else
            Debug.Log("Vertices " + fromVert + " to " + toVert + " does not exits");
    }
    
    private void RemoveEdge(Vector2Int fromVert, Vector2Int toVert)
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
    private List<float> Neighbors(Vector2Int position)
    {
        if (_adjacencyList.ContainsKey(position))
        {
            return new List<float>(_adjacencyList[position].Adj.Values);
        }
        else
        {
            Debug.Log("Vertex " + position + " was not found");
            return new List<float>();
        }
    }

    private float GetEdgeValue(Vector2Int fromVert, Vector2Int toVert)
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

}
