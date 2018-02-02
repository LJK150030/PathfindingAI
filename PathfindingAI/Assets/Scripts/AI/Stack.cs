using System.Collections.Generic;
using Assets.Scripts.AI.Data_Structures;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [CreateAssetMenu(menuName = "AI/Frontier/Stack")]
    public class Stack : Frontier {
        private Stack<SearchNode> _openList;

        private void OnEnable()
        {
            _openList = new Stack<SearchNode>();
        }

        public override bool IsEmpty()
        {
            return _openList.Count <= 0;
        }

        public override SearchNode Pop()
        {
            return _openList.Pop();
        }

        public override void Insert(SearchNode n)
        {
            _openList.Push(n);
        }

    }
}
