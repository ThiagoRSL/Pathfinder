using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Dictionary<Vector3, TileController> Tiles;
    public List<EntityController> Entities;
    public int Complexity { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

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
    public Astar PFAStar;
    public Dijkstra PFDijkstra;
    private Graph Graph;
    private Grid Grid;

    private void SetGraph(){ Graph = new Graph(Grid);}

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
        PFAStar = new Astar(Graph);
        PFDijkstra = new Dijkstra(Graph);
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
        List<TileController> dijkstra = PFDijkstra.FindPath(start, target);
        List<TileController> astar = PFAStar.FindPath(start, target);
        Debug.Log("Dijkstra closed nodes: ");
        Debug.Log(PFDijkstra.CountClosed());
        Debug.Log("Astar closed nodes: ");
        Debug.Log(PFAStar.CountClosed());
        return astar;
    }
}
