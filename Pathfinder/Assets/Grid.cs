using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Dictionary<Vector3, Tile> tiles;
    private int width = 10;
    private int height = 10;
    private int complexity = 10;
    public int Complexity { get { return complexity; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }

    public Grid(int width, int height, int complexity)
    {
        this.width = width;
        this.height = height;
        this.complexity = complexity;

        GameObject gameObject = new GameObject("Grid", typeof(Grid));
        Transform transform = gameObject.transform;
        transform.SetParent(null, false);
        transform.localPosition = new Vector3(0, 0, 0);

        tiles = new Dictionary<Vector3, Tile>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 position = new Vector3(i, j, 0);
                var newTile = Instantiate(GameManager.Instance.tilePreFab, position, Quaternion.identity);
                newTile.transform.SetParent(transform, true);
                newTile.transform.position = position;
                newTile.name = $"Tile {i} {j}";
                newTile.Init(UnityEngine.Random.Range(1, 100) > 100 - complexity, i, j);
                tiles[position] = newTile;
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