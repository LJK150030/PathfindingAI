using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public FloorTile[] Neighbors;

    private void Awake()
    {
        Neighbors = new FloorTile[4];
    }
}
