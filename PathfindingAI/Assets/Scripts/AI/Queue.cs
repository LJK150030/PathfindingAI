using System.Collections.Generic;
using Assets.Scripts.AI.Data_Structures;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [CreateAssetMenu(menuName = "AI/Frontier/Queue")]
    public class Queue : Frontier
    {
        private Queue<SearchNode> _openList;

        private void OnEnable()
        {
            _openList = new Queue<SearchNode>();
        }

        public override bool IsEmpty()
        {
            return _openList.Count <= 0;
        }

        public override SearchNode Pop()
        {
            return _openList.Dequeue();
        }

        public override void Insert(SearchNode n)
        {
            _openList.Enqueue(n);
        }
    }
}
