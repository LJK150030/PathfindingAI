using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [CreateAssetMenu(menuName = "AI/Frontier/Priority Queue")]
    public class PriorityQueue : Frontier
    {

        private List<SearchNode> _pq;
        private Dictionary<Vector2Int, SearchNode> _checkSet;
        private SearchNode _ZeroNode;
        private int _index;

        private void OnEnable()
        {
            _pq = new List<SearchNode>();
            _ZeroNode = new SearchNode();
            _checkSet = new Dictionary<Vector2Int, SearchNode>();
            _index = 0;
            _pq.Add(_ZeroNode);
        }

        public override bool IsEmpty()
        {
            return _index == 0;
        }

        public int Size()
        {
            return _index;
        }

        public override void Insert(SearchNode n)
        {
            _pq.Insert(++_index, n);
            _checkSet.Add(n.GetState(), n);
            Swim(_index); //if n is bigger than parents, then change, reassess the next parent, etc.
        }

        public override SearchNode Pop()
        {
            SearchNode minNode = _pq[1];  //Gets the refrence of max key from the top
            _checkSet.Remove(_pq[1].GetState());
            Exchange(1, _index--);        //Exchange with last item of the heap.
            _pq[_index + 1] = null;       // Avoid loitering.
            Sink(1);                      //Restore heap.
            return minNode;
        }

        public override bool Contains(Vector2Int coord)
        {
            return _checkSet.ContainsKey(coord);
        }

        public override void Clear()
        {
            //derefrence all of the Searchnode from the heap
            for (int n = 0; n <= _index; n++)
            {
                _pq[n] = null;
            }

            _checkSet.Clear();
            _index = 0;
        }

        //is the element at index a more than the element at index b
        private bool More(int a, int b)
        {
            return _pq[a].GetPathCost().CompareTo(_pq[b].GetPathCost()) > 0;
        }

        //exchange the elements at index A with index B
        private void Exchange(int a, int b)
        {
            SearchNode n = _pq[a];
            _pq[a] = _pq[b];
            _pq[b] = n;
        }

        //bring the element at index k up to the top of the heap, until it's parent is larger or it is at the top
        private void Swim(int k)
        {
            //while the index is greater than 1 and is more than it's parent
            while (k > 1 && More(k / 2, k))
            {
                Exchange(k / 2, k);
                k = k / 2;
            }
        }

        //take down the element at index k down to the bottom of the heap, until a child is smaller or it is a child.
        private void Sink(int k)
        {
            while (2 * k <= _index)
            {
                int j = 2 * k;

                //if j is less than the largest index and is more than its sibling, increase j
                if (j < _index && More(j, j + 1))
                    j++;

                //if the element at index k is less than the element at j, then leave k as the parent
                if (!More(k, j))
                    break;

                //otherwise, exchange the elements in k with j.
                Exchange(k, j);
                k = j;
            }
        }
    }
}
