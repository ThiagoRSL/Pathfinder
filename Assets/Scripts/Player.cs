using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Tile _targetTile;
    [SerializeField] public Tile atTile;
    private List<Tile> path;
    private GridManager _grid;

    public void Init(Tile tileInit, GridManager grid)
    {
        this.path = null;
        this._grid = grid;
        this.atTile = tileInit;
        this.transform.position = this.atTile.GetPosition() + new Vector3(0,0,1);
        this.SetColor(new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)));
    }
    public void SetColor(Color color)
    {
        this._renderer.color = color;
    }
    public void MoveTo(Vector3 targetPosition)
    {
        this._targetTile = this._grid.GetTileAtPosition(targetPosition);
        this.Move();
    }
    public void SetPath(List<Tile> path)
    {
        if (this.path != null)
        {
            foreach (var tile in this.path)
            {
                tile.UnsetPath();
            }
        }
        this.path = path;
        foreach (var tile in this.path)
        {
            tile.SetPath();
        }
    }
    public void Move()
    {

    }
}
