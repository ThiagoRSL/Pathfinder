using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    protected GridController AtGrid;
    protected TileController AtTile;
    protected List<TileController> Path;
    protected TileController TargetTile;
    protected IPathFinder PathFinderAlgorithm;

    protected bool isMoving;
    protected int moveSpeed;

    protected bool IsMoving() { return this.isMoving; }
    public void SetAtTile(TileController tile) { AtTile = tile; }
    public void SetAtGrid(GridController grid){ AtGrid = grid; }

    protected void SetTargetTile(Vector3 position)
    {
        if (IsMoving()) return;
        if (!(TargetTile is null))
        {
            TargetTile.UnselectTile();
        }
        TargetTile = AtGrid.GetTileAtPosition(position);
        TargetTile.SelectTile();

        SetPath(PathFinderAlgorithm.FindPath(AtTile.GetPosition(), TargetTile.GetPosition()));
    }

    protected void MoveTo(Vector3 targetPosition)
    {
        TargetTile = AtGrid.GetTileAtPosition(targetPosition);
        Move();
    }

    protected void SetPath(List<TileController> Path)
    {
        if (this.Path != null)
        {
            foreach (var tile in this.Path)
            {
                tile.UnsetPath();
            }
        }
        this.Path = Path;
        foreach (var tile in this.Path)
        {
            tile.SetPath();
        }
        StartCoroutine(this.Move());
    }

    protected IEnumerator Move()
    {
        isMoving = true;
        TileController target = Path[0];
        float unevenness = TileController.Unevenness(AtTile, target);
        Vector3 targetPosition = target.GetPosition() + new Vector3(0, 0, 1);
        while ((targetPosition - ((Vector3)transform.position)).sqrMagnitude > Mathf.Epsilon)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed / unevenness * Time.deltaTime);
            yield return null;
        }
        AtTile = Path[0];
        AtTile.SetAtop(this);
        transform.position = targetPosition;
        Path[0].UnsetPath();
        Path.RemoveAt(0);
        isMoving = false;

        if (Path.Count > 0) StartCoroutine(Move());
    }

    public void Init(GridController Grid, IPathFinder PFAlgorithm)
    {
        this.AtGrid = Grid;
        this.PathFinderAlgorithm = PFAlgorithm;
    }
}
