using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public int Id;
    public Dictionary<int, float> Adj;
    public bool Active;

    public void ActivateTile(int id)
    {
        Id = id;
        Active = true;
        Adj = new Dictionary<int, float>();
    }
}
