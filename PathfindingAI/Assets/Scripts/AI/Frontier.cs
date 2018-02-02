using Assets.Scripts.AI.Data_Structures;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public abstract class Frontier : ScriptableObject
    {
        public abstract bool IsEmpty();
        public abstract SearchNode Pop();
        public abstract void Insert(SearchNode n);
    }
}
