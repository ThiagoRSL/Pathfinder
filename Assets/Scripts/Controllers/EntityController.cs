using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    protected GridController AtGrid;
    protected TileController AtTile;
    protected List<TileController> path;
    protected TileController TargetTile;

    protected bool isMoving;
    protected int moveSpeed;

    protected bool IsMoving() { return this.isMoving; }
    public void SetAtGrid(GridController grid) { AtGrid = grid; }
    public void SetAtTile(TileController tile) { AtTile = tile; }

    protected void SetTargetTile(Vector3 position)
    {
        if (IsMoving()) return;
        if (!(TargetTile is null))
        {
            TargetTile.UnselectTile();
        }
        TargetTile = AtGrid.GetTileAtPosition(position);
        TargetTile.SelectTile();

        Dijkstra dm = new Dijkstra();
        SetPath(dm.FindPath(AtTile.GetPosition(), TargetTile.GetPosition()));
        dm = null;
    }

    protected void MoveTo(Vector3 targetPosition)
    {
        TargetTile = Grid.GetTileAtPosition(targetPosition);
        Move();
    }

    protected void SetPath(List<TileController> path)
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

    protected IEnumerator Move()
    {
        this.isMoving = true;
        Tile target = this.path[0];
        float unevenness = Tile.Unevenness(atTile, target);
        Vector3 targetPosition = target.GetPosition() + new Vector3(0, 0, 1);
        while ((targetPosition - ((Vector3)transform.position)).sqrMagnitude > Mathf.Epsilon)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed / unevenness * Time.deltaTime);
            yield return null;
        }
        atTile = path[0];
        atTile.SetAtop(this);
        transform.position = targetPosition;
        path[0].UnsetPath();
        path.RemoveAt(0);
        isMoving = false;

        if (path.Count > 0) StartCoroutine(Move());
    }


}
