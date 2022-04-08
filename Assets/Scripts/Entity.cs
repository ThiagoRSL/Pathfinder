using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Tile atTile;
    protected Tile targetTile;
    protected Grid grid;
    protected bool isMoving;
    protected int moveSpeed;
    protected List<Tile> path;

    protected void SetTargetTile(Vector3 position)
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

    protected void MoveTo(Vector3 targetPosition)
    {
        this.targetTile = grid.GetTileAtPosition(targetPosition);
        this.Move();
    }

    protected void SetPath(List<Tile> path)
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
        transform.position = targetPosition;
        path[0].UnsetPath();
        path.RemoveAt(0);
        isMoving = false;

        if (path.Count > 0) StartCoroutine(Move());
    }

    protected bool IsMoving()
    {
        return this.isMoving;
    }

}
