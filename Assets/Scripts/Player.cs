using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Tile _targetTile;
    [SerializeField] public Tile atTile;
    private bool isMoving;
    private int moveSpeed;
    private List<Tile> path;
    private GridManager _grid;

    public bool IsMoving()
    {
        return this.isMoving;
    }
    public void Init(Tile tileInit, GridManager grid)
    {
        this.moveSpeed = 5;
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
        StartCoroutine(this.Move());
    }
    IEnumerator Move()
    {
        this.isMoving = true;
        Tile target = this.path[0]; 
        float unevenness = Tile.Unevenness(this.atTile, target);
        Vector2 targetPosition = target.GetPosition();
        while((targetPosition - ((Vector2) this.transform.position)).sqrMagnitude > Mathf.Epsilon)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, this.moveSpeed/unevenness * Time.deltaTime);
            yield return null;
        }
        this.atTile = this.path[0];
        this.transform.position = targetPosition;
        this.path[0].UnsetPath();
        this.path.RemoveAt(0);
        this.isMoving = false;

        if(this.path.Count > 0) StartCoroutine(this.Move());
    }
}
