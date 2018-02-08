using UnityEngine;

namespace Assets.Scripts.AI
{
    public abstract class Frontier : ScriptableObject
    {
        public abstract bool IsEmpty();
        public abstract SearchNode Pop();
        public abstract void Insert(SearchNode n);
        public abstract bool Contains(Vector2Int coord);
        public abstract void Clear();
    }
}
