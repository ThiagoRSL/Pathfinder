using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Tile _targetTile;
    [SerializeField] private Tile _atTile;
    private GridManager _grid;

    public void Init(Tile tileInit, GridManager grid)
    {
        this.SetColor(new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)));
        this._grid = grid;
        this._atTile = tileInit;
        Debug.Log(this._atTile);
        this.transform.position = this._atTile.GetPosition() + new Vector3(0,0,1);
    }
    public void SetColor(Color color)
    {
        this._renderer.color = color;
        Debug.Log(color);
    }
    public void MoveTo(Vector3 targetPosition)
    {
        this._targetTile = this._grid.GetTileAtPosition(targetPosition);
        this.Move();
    }
    public void Move()
    {

    }

    public Tile AtTile()
    {
        return this._atTile;
    }
}
