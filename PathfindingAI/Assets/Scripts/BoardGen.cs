using System.Linq.Expressions;
using UnityEngine;

public class BoardGen : MonoBehaviour
{

    public GameObject FloorTileGameObject;
    public int BoardWidth;
    public int BoardHeight;

    private Vector2 _boardLocation;
    private FloorTile[] _tiles;

    private void Awake()
    {
        _boardLocation = gameObject.transform.position;
        _tiles = new FloorTile[BoardWidth * BoardHeight];

        Vector2 currentLocation = _boardLocation + new Vector2(1.0f * (BoardWidth / 2.0f), 1.0f * (BoardHeight / 2.0f));

        for (int x = 0; x < BoardWidth; x++)
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                Instantiate(FloorTileGameObject, currentLocation, Quaternion.identity, transform);
            }
        }
    }


}
