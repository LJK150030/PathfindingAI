using System.Collections.Generic;
using UnityEngine;

public class VertexTile : MonoBehaviour
{
    public int Id;
    public Dictionary<Vector2Int, float> Adj;
    public int type; // 0 (active), 1 (deactivated), 2 (start), 3 (goal)
    public int Xlocation;
    public int Ylocation;

    private SpriteRenderer _render;

    public void ActivateTile(int id, int x, int y)
    {
        Id = id;
        type = 0;
        Xlocation = x;
        Ylocation = y;
        Adj = new Dictionary<Vector2Int, float>();
        _render = GetComponent<SpriteRenderer>();
    }

    private void UpdateSprite()
    {
        switch (type)
        {
            case 0:
                _render.color = Color.white;
                break;
            case 1:
                _render.color = Color.black;
                break;
            case 2:
                _render.color = Color.green;
                break;
            case 3:
                _render.color = Color.red;
                break;
        }
    }

    private void OnMouseDown()
    {
        type = (type + 1) % 4;
        UpdateSprite();
    }
}
