using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    [SerializeField] private SpriteRenderer _renderer;

    public void Init(TileController tileInit, GridController grid)
    {
        path = null;
        moveSpeed = 5;
        AtGrid = grid;
        AtTile = tileInit;
        transform.position = AtTile.GetPosition() + new Vector3(0,0,1);
        SetColor(new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)));
    }
    public void SetColor(Color color)
    {
        this._renderer.color = color;
    }
    public void DefinePath(Vector3 target)
    {
        SetTargetTile(target);
    }
}
