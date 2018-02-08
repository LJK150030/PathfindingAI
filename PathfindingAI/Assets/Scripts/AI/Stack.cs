using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [CreateAssetMenu(menuName = "AI/Frontier/Stack")]
    public class Stack : Frontier {

        private Stack<SearchNode> _openList;
        private Dictionary<Vector2Int, SearchNode> _checkSet;

        private void OnEnable()
        {
            _openList = new Stack<SearchNode>();
            _checkSet = new Dictionary<Vector2Int, SearchNode>();
        }

        public override bool IsEmpty()
        {
            return _openList.Count <= 0;
        }

        public override SearchNode Pop()
        {
            _checkSet.Remove(_openList.Peek().GetState());
            return IsEmpty() ? null : _openList.Pop();
        }

        public override void Insert(SearchNode n)
        {
            _openList.Push(n);
            _checkSet.Add(n.GetState(), n);
        }

        public override bool Contains(Vector2Int coord)
        {
            return _checkSet.ContainsKey(coord);
        }

        public override void Clear()
        {
            _openList.Clear();
            _checkSet.Clear();
        }
    }
}
