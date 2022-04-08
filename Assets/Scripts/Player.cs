using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    private Tile atTile;
    private Tile targetTile;
    private Grid grid;
    private bool isMoving;
    private int moveSpeed;
    private List<Tile> path;

    public bool IsMoving()
    {
        return this.isMoving;
    }
    public void Init(Tile tileInit)
    {
        moveSpeed = 5;
        this.grid = GameManager.Instance.Grid;
        path = null;
        atTile = tileInit;
        transform.position = this.atTile.GetPosition() + new Vector3(0,0,1);
        SetColor(new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)));
    }
    public void SetColor(Color color)
    {
        this._renderer.color = color;
    }
    public void MoveTo(Vector3 targetPosition)
    {
        this.targetTile = grid.GetTileAtPosition(targetPosition);
        this.Move();
    }
    public void SetTargetTile(Vector2 position)
    {
        if (this.IsMoving()) return;
        if (!(this.targetTile is null))
        {
            this.targetTile.UnselectTile();
        }
        this.targetTile = grid.GetTileAtPosition(position);
        this.targetTile.SelectTile();
        Dijkstra dm = new Dijkstra();
        SetPath(dm.FindPath(atTile.GetPosition(), targetTile.GetPosition()));
        dm = null;
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
