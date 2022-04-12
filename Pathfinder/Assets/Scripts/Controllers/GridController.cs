using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Dictionary<Vector3, Tile> Tiles;
    public List<EntityController> Entities;
    public int Complexity { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Grid(int width, int height, int complexity)
    {
        Width = width;
        Height = height;
        Complexity = complexity;

        Tiles = new Dictionary<Vector3, Tile>();
        Entities = new List<EntityController>();
    }
}

public class GridController : MonoBehaviour
{
    private Grid Grid;

    public void Init(int width, int height, int complexity, Tile tilePreFab)
    {
        Grid = new Grid(width, height, complexity); 

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 position = new Vector2(i, j);
                var newTile = Instantiate(tilePreFab, position, Quaternion.identity);
                newTile.transform.SetParent(transform, true);
                newTile.transform.position = position;
                newTile.name = $"Tile {i} {j}";
                newTile.Init(UnityEngine.Random.Range(1, 100) > 100 - complexity, i, j);
                Grid.SetTileAtPosition(position, newTile);
            }
        }
    }
    public Graph MakeGraph()
    {
       return new Graph(Grid);
    }
    public void PutEntity(EntityController entity, Tile tile)
    {
        entity.SetAtGrid(Grid);
        entity.SetAtTile(tile);
        tile.SetAtop(entity);
        AddEntity(entity);
    }
    public void AddEntity(EntityController entity)
    {
        Grid.Entities.Add(entity);
    }
    
    public void SetTileAtPosition(Vector2 position, Tile tile) { Grid.SetTileAtPosition(position, tile); }
    public Tile GetTileAtPosition(Vector2 position) { Grid.GetTileAtPosition(position); }
}
