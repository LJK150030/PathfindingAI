using System.Collections.Generic;
using UnityEngine;

public class BoardGen : MonoBehaviour
{

    public FloorTile FloorTileGameObject;
    public int BoardWidth;
    public int BoardHeight;

    private Dictionary<int, FloorTile> _adjacencyList;
    private FloorTile[,] _board;

    //setting up the board and the connections
    private void Awake()
    {
        _adjacencyList = new Dictionary<int, FloorTile>();
        _board = new FloorTile[BoardWidth, BoardHeight];

        GenerateBoard();
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
                _board[x, y].ActivateTile(numNodes);
                AddVertex(numNodes, _board[x, y]);
                numNodes++;
            }

            currentLocation = new Vector2(currentLocation.x + 1, BoardHeight / 2.0f - 0.5f);
        }
    }

    /*
    private void GenerateEdges()
    {
        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                
            }
        }
    }
    */

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
        if (!_adjacencyList.ContainsKey(fromVert) && !_adjacencyList.ContainsKey(toVert))
        {
            _adjacencyList[fromVert].Adj.Add(toVert, 1.0f);
            Debug.Log("G(" + fromVert + "," + toVert + ")");
        }
        else
            Debug.Log("Vertices " + fromVert + " to " + toVert + " does not exits");
    }

    //Adds a new, weighted, directed edge to the graph that connects two vertices.
    private void AddEdge(int fromVert, int toVert, float weight)
    {
        if (!_adjacencyList.ContainsKey(fromVert) && !_adjacencyList.ContainsKey(toVert))
        {
            _adjacencyList[fromVert].Adj.Add(toVert, weight);
            Debug.Log("G(" + fromVert + "," + toVert + "," + weight + ")");
        }
        else
            Debug.Log("Vertices " + fromVert + " to " + toVert + " does not exits");
    }

    private void RemoveEdge(int fromVert, int toVert)
    {
        if (_adjacencyList.ContainsKey(fromVert) && _adjacencyList.ContainsKey(toVert))
        {
            _adjacencyList[fromVert].Adj.Remove(toVert);
            Debug.Log("Removed G(" + fromVert + "," + toVert + ")");
        }
        else
            Debug.Log("Vertices " + fromVert + " to " + toVert + " does not exits");
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
}
