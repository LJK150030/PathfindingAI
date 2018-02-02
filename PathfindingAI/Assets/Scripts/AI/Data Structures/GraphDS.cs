using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Data_Structures
{
    public class GraphDS
    {
        public Dictionary<Vector2Int, SearchNode> AdjacencyList;

        //constructor with a root coordinate
        public GraphDS(Vector2Int rootCoord)
        {
            AdjacencyList = new Dictionary<Vector2Int, SearchNode>
            {
                {rootCoord, new SearchNode(rootCoord)}
            };
        }

        public bool AddChild(Vector2Int from, Vector2Int to, int amount)
        {
            //Does the from nodes exits?
            if (!AdjacencyList.ContainsKey(from) || AdjacencyList.ContainsKey(to)) return false;

            //If so, then we need to add a vertex for to and an edge from from to to
            AddVertex(to);
            AddEdge(from, to, amount);
            return true;
        }

        public bool RemoveChild(Vector2Int from, Vector2Int to)
        {
            //Are the nodes adjacent?
            if (!Adjacent(from, to)) return false;

            //If so, then remove the edge then the vertex
            RemoveEdge(from, to);
            RemoveVertex(to);
            return true;
        }

        public void Clear()
        {
            AdjacencyList.Clear();
        }

        public int GetEdgeValue(Vector2Int from, Vector2Int to)
        {
            //if from to to is not adjacent
            if (!Adjacent(from, to)) return -1;

            //then return the pathcost
            return AdjacencyList[to].GetPathCost();
        }

        //add a vertex to the adjacency list
        private void AddVertex(Vector2Int coord)
        {
            AdjacencyList.Add(coord, new SearchNode(coord));
        }

        private void AddEdge(Vector2Int @from, Vector2Int to, int amount)
        {
            AdjacencyList[to].SetParent(AdjacencyList[from]);
            AdjacencyList[to].SetPathCost(AdjacencyList[from].GetPathCost() + amount);
        }

        private void RemoveVertex(Vector2Int coord)
        {
            AdjacencyList.Remove(coord);
        }

        private void RemoveEdge(Vector2Int @from, Vector2Int to)
        {
            AdjacencyList[to].SetParent(null);
            AdjacencyList[to].SetPathCost(0);
        }

        private bool Adjacent(Vector2Int from, Vector2Int to)
        {
            //are the nodes in the list?
            if (!AdjacencyList.ContainsKey(from) || !AdjacencyList.ContainsKey(to)) return false;

            //is there a refrence from TO to FROM
            if (from != AdjacencyList[to].GetParent().GetState()) return false;
            
            //else
            return true;
        }
    }
}
