using System.Collections.Generic;
using Assets.Scripts.AI.Data_Structures;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [CreateAssetMenu(menuName = "AI/Frontier/Priority Queue")]
    public class PriorityQueue : Frontier
    {
        private SortedList<int, SearchNode> _openList;
        private int _currentIndex;

        private void OnEnable()
        {
            _openList = new SortedList<int, SearchNode>();
            _currentIndex = -1;
        }

        public override bool IsEmpty()
        {
            return _openList.Count <= 0;
        }

        public override SearchNode Pop()
        {
            return _currentIndex <= 0 ? null : _openList[_currentIndex];
        }

        public override void Insert(SearchNode n)
        {
            _openList.Add(n.GetPathCost(), n);
        }
    }
}
