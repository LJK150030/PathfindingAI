using UnityEngine;

namespace Assets.Scripts.AI
{
    public class SearchNode
    {

        private SearchNode _parent;
        private int _pathCost;
        private Vector2Int _coord;

        public SearchNode()
        {
            _parent = null;
            _pathCost = -1;
            _coord = new Vector2Int(-1, -1);
        }

        public SearchNode(Vector2Int loc)
        {
            _parent = null;
            _pathCost = 0;
            _coord = loc;
        }

        public SearchNode(Vector2Int loc, SearchNode p)
        {
            _parent = p;
            _pathCost += p._pathCost;
            _coord = loc;
        }

        public SearchNode(Vector2Int loc, SearchNode p, int wieght)
        {
            _parent = p;
            _pathCost += wieght;
            _coord = loc;
        }

        public SearchNode GetParent()
        {
            return _parent;
        }

        public void SetParent(SearchNode p)
        {
            _parent = p;
        }

        public int GetPathCost()
        {
            return _pathCost;
        }

        public void SetPathCost(int amount)
        {
            _pathCost = amount;
        }

        public Vector2Int GetState()
        {
            return _coord;
        }
    }
}
