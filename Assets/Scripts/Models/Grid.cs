using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private Dictionary<Vector3, Tile> tiles;
    private int width;
    private int height;
    private int complexity;
    public int Complexity { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Grid(int width, int height, int complexity)
    {
        this.width = width;
        this.height = height;
        this.complexity = complexity;

        tiles = new Dictionary<Vector3, Tile>();
    }
    public void SetTileAtPosition(Vector2 position, Tile tile)
    {
        tiles[position] = tile;
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
