using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void SpawnEntity(EntityController entity)
    {
        return new Graph(Grid);
    }
    public void SetTileAtPosition(Vector2 position, Tile tile) { Grid.SetTileAtPosition(position, tile); }
    public Tile GetTileAtPosition(Vector2 position) { Grid.GetTileAtPosition(position); }
}
