using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grid
{
    [SerializeField]
    public Tile tilePreFab;

    private Dictionary<Vector3, Tile> tiles;
    private int width;
    private int height;
    private int complexity;
    public int Complexity { get { return complexity; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }

    public Grid(int width, int height, int complexity)
    {
        this.width = width;
        this.height = height;
        this.complexity = complexity;

        tiles = new Dictionary<Vector3, Tile>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = new Vector3(i, j);
                var newTile = new Tile(); //Instantiate(tilePreFab, new Vector3(i, j, 0), Quaternion.identity);
                tiles[pos] = newTile;
                //newTile.transform.position = pos;
                newTile.name = $"Tile {i} {j}";
                newTile.Init(UnityEngine.Random.Range(1, 100) > 100 - complexity, i, j);
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if (tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }
        return null;
    }
}
