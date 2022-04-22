using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Grid
{
    public Dictionary<Vector3, TileController> Tiles;
    public List<EntityController> Entities;
    public int Complexity { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Grid(int width, int height, int complexity)
    {
        Width = width;
        Height = height;
        Complexity = complexity;

        Tiles = new Dictionary<Vector3, TileController>();
        Entities = new List<EntityController>();
    }

    public TileController GetTileAtPosition(Vector2 position)
    {
        if (Tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }
        return null;
    }
}

public class GridController : MonoBehaviour
{
    public Dijkstra PFDijkstra;
    public Astar PFAstar;
    //public HPAstar PFHPAstar;

    private Graph Graph;
    private Grid Grid;

    public int IndexFrom2D(int i, int j) { return (j * Grid.Width) + i; }
    public int IndexFrom2D(Vector2 v) { return ((int)v.y * Grid.Width) + (int)v.x; }
    public Vector2 Vector2FromIndex(int index) 
    {
        int x = (index % Grid.Width);
        int y = (int) Mathf.Floor(index / Grid.Width);
        return new Vector2(x, y);
    }

    private void SetGraph()
    { 
        Graph = GridToGraph.Convert(Grid);
    }

    public void Init(int width, int height, int complexity, TileController tilePreFab)
    {
        Grid = new Grid(width, height, complexity); 

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 position = new Vector2(i, j);
                var newTile = Instantiate(tilePreFab, position, Quaternion.identity);
                newTile.transform.SetParent(this.transform, true);
                newTile.transform.position = position;
                newTile.name = $"Tile {i} {j}";
                newTile.Init(UnityEngine.Random.Range(1, 100) > 100 - complexity, i, j);
                SetTileAtPosition(position, newTile);
            }
        }

        SetGraph();
        PFDijkstra = new Dijkstra(Graph);
        PFAstar = new Astar(Graph, width);
        //PFHPAstar = new HPAstar(Graph);
    }
    public void PutEntity(EntityController entity, TileController tile)
    {
        entity.SetAtGrid(this);
        entity.SetAtTile(tile);
        tile.SetAtop(entity);
        AddEntity(entity);
    }
    public void AddEntity(EntityController entity)
    {
        Grid.Entities.Add(entity);
    }
    public void SetTileAtPosition(Vector2 position, TileController tile) 
    {
        Grid.Tiles[position] = tile;
    }
    public TileController GetTileAtPosition(Vector2 position) { return Grid.GetTileAtPosition(position); }

    public List<TileController> FindPath(Vector2 start, Vector2 target)
    {
        //Comparing Nodes//

        List<int> DijkstraIDs = PFDijkstra.FindPath(IndexFrom2D(start), IndexFrom2D(target));
        Debug.Log("Dijkstra closed nodes: ");
        Debug.Log(PFDijkstra.CountClosed());

        //List<TileController> hpastar = PFAstar.FindPath(start, target);
        //Debug.Log("HPAstar closed nodes: ");
        //Debug.Log(PFHPAstar.CountClosed());

        List<TileController> Path = new List<TileController>();
        List<int> PathIds = PFAstar.FindPath(IndexFrom2D(start), IndexFrom2D(target));
        while(PathIds.Count > 0)
        {
            Path.Add(GetTileAtPosition(Vector2FromIndex(PathIds[0])));
            PathIds.RemoveAt(0);
        }
        Debug.Log("Astar closed nodes: " + PFAstar.CountClosed().ToString());

        return Path;
    }

}
