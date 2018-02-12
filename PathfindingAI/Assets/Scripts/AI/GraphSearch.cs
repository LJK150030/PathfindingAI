using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Square;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.AI
{
    public class GraphSearch : MonoBehaviour
    {
        public Frontier UnexploredList;
        public float WaitTime;
        public bool StartSearch;
        public bool UseLimit;
        public int Limit;
        public bool IterativeDeepening;
        public bool Heuristic;
        public bool Manhattan;
        public bool Euclidean;
        public bool AStar;

        private Dictionary<Vector2Int, SearchNode> _exploredList;
        private bool _foundGoal;
        private SquareBoard _board;
        private bool _searchRunning;
        private int _iterations;
        

        private void Awake()
        {
            _exploredList = new Dictionary<Vector2Int, SearchNode>();
            _board = GetComponent<SquareBoard>();
            _foundGoal = false;
            StartSearch = false;
            _searchRunning = false;
            UseLimit = false;
            Limit = 0;
            WaitTime = 0.0f;
            IterativeDeepening = false;
            Heuristic = false;
            Manhattan = false;
            Euclidean = false;
            AStar = false;
            _iterations = -1;
        }

        private void Update()
        {
            if (!_searchRunning && StartSearch)
            {
                _searchRunning = true;
                if (IterativeDeepening)
                {
                    if (_iterations < Limit)
                    {
                        StartCoroutine(Search());
                    }
                }
                else
                {
                    _iterations = Limit;
                    StartCoroutine(Search());
                }
            }
        }

        //Using a grpah search to find the goal node
        public IEnumerator Search()
        {
            _iterations++;
            _foundGoal = false;
            //initialize the frontier using the initial state of problem
            //UnexploredList.OnEnable()
            SearchNode node = new SearchNode(_board.GetStartCoord());
            UnexploredList.Insert(node);

            //initialize the explored set to be empty
            _exploredList.Clear();

            while (!_foundGoal)
            {
                //if the frontier is empty then return failure
                if (UnexploredList.IsEmpty())
                {
                    Debug.Log("Open List is empty");
                    _foundGoal = false;
                    break;
                }

                //choose a leaf node and remove it from the frontier
                node = UnexploredList.Pop();

                //if the node contains a goal state then return the corresponding solution
                if (_board.IsGoal(node.GetState()))
                {
                    Debug.Log("Found the goal");

                    while (!_board.IsStart(node.GetParent().GetState()))
                    {
                        node = node.GetParent();
                        _board.UpdateKind(node.GetState(), 6);
                    }

                    _foundGoal = true;
                    break;
                }

                //add the node to the explored set
                _exploredList.Add(node.GetState(), node);
                if(_board.IsStart(node.GetState()))
                    _board.UpdateKind(node.GetState(), 2);
                if (_board.IsGoal(node.GetState()))
                    _board.UpdateKind(node.GetState(), 3);
                if (!_board.IsGoal(node.GetState()) && !_board.IsStart(node.GetState()))
                    _board.UpdateKind(node.GetState(), 5);

                //if we are using a stack and we have a limit
                if (UseLimit && node.GetPathCost() >= _iterations)
                {
                    continue;
                }

                //expand the chosen node, adding the resulting nodes to the frontier only if not in the frontier or explored set
                List<Vector2Int> nextTiles = _board.Neighbors(node.GetState());

                foreach (Vector2Int tile in nextTiles)
                {
                    SearchNode temp = new SearchNode(tile, node, node.GetPathCost() + 1);

                    if (Heuristic && UnexploredList is PriorityQueue)
                    {
                        if(Manhattan)
                            temp = new SearchNode(tile, node, Mathf.Abs(_board.GetGoalCoord().x - tile.x) + Mathf.Abs(_board.GetGoalCoord().y - tile.y));

                        if(Euclidean)
                            temp = new SearchNode(tile, node, Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(_board.GetGoalCoord().x - tile.x, 2.0f) + Mathf.Pow(_board.GetGoalCoord().y - tile.y, 2.0f))));

                        if (AStar && Manhattan)
                            temp = new SearchNode(tile, node, node.GetPathCost() + Mathf.Abs(_board.GetGoalCoord().x - tile.x) + Mathf.Abs(_board.GetGoalCoord().y - tile.y));

                        if (AStar && Euclidean)
                            temp = new SearchNode(tile, node, node.GetPathCost() + Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(_board.GetGoalCoord().x - tile.x, 2.0f) + Mathf.Pow(_board.GetGoalCoord().y - tile.y, 2.0f))));
                    }

                    if (_exploredList.ContainsKey(tile) || UnexploredList.Contains(tile))
                    {
                        //Check to see if the neighbor node is cheaper to travel than a previously
                        if (_exploredList.ContainsKey(temp.GetState()) &&
                            temp.GetPathCost() < _exploredList[tile].GetPathCost())
                        {
                            _exploredList.Remove(tile);
                        }
                        else
                        {
                            continue;
                        }
                    }


                    UnexploredList.Insert(temp);
                    _board.UpdateKind(tile, _board.IsGoal(tile) ? 3 : 4);
                    yield return new WaitForSeconds(WaitTime);
                }
            }

            if(IterativeDeepening && (_iterations < Limit))
            {
                StartSearch = true;
                _searchRunning = false;

                if (_foundGoal)
                {
                    StartSearch = false;
                    _searchRunning = false;
                    Debug.Log("On Limit " + _iterations);
                    _iterations = -1;
                }
            }
            else
            {
                StartSearch = false;
                _searchRunning = false;
                _iterations = -1;
            }
            UnexploredList.Clear();
        }

        public void SetFrontier(Frontier frontList)
        {
            UnexploredList = frontList;
        }

        public void Search(bool start)
        {
            StartSearch = start;
        }

        public void SetWaitTime(Slider bar)
        {
            WaitTime = bar.value;
        }

        public void SetLimitBool(Toggle toggle)
        {
            UseLimit = toggle.isOn;
        }

        public void SetLimitAmount(InputField stringField)
        {
            foreach (char c in stringField.text)
            {
                if (c < '0' || c > '9')
                {
                    Limit = 0;
                    return;
                }
            }

            Limit = Int32.Parse(stringField.text);
        }

        public void SetID(Toggle tog)
        {
            IterativeDeepening = tog.isOn;
        }

        public void SetHeuristic(Toggle tog)
        {
            Heuristic = tog.isOn;
        }

        public void SetToManhattan()
        {
            Manhattan = true;
            Euclidean = false;
        }

        public void SetToEuclidean()
        {
            Manhattan = false;
            Euclidean = true;
        }

        public void SetAStar(Toggle tog)
        {
            AStar = tog.isOn;
        }

    }

}
