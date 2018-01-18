using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public int Id;
    public Dictionary<int, float> Adj;
    public int type; // 0 (deactive), 1 (active), 2 (start), 3 (goal)
    public int Xlocation;
    public int Ylocation;

    private SpriteRenderer _render;

    public void ActivateTile(int id, int x, int y)
    {
        Id = id;
        type = 1;
        Xlocation = x;
        Ylocation = y;
        Adj = new Dictionary<int, float>();
        _render = GetComponent<SpriteRenderer>();
    }

    public void UpdateSprite()
    {
        switch (type)
        {
            case 0:
                _render.color = Color.black;
                break;
            case 1:
                _render.color = Color.white;
                break;
            case 2:
                _render.color = Color.green;
                break;
            case 3:
                _render.color = Color.red;
                break;
        }
    }
}
