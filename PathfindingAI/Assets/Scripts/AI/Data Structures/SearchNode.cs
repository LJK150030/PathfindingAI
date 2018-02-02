using UnityEngine;

namespace Assets.Scripts.AI.Data_Structures
{
    public class SearchNode
    {

        private SearchNode _parent;
        private int _pathCost;
        private readonly Vector2Int _coord;

        public SearchNode(Vector2Int loc)
        {
            _parent = null;
            _pathCost = 0;
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
