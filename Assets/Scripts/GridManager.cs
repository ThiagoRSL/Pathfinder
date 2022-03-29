using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile tilePreFab, selectedTile;
    [SerializeField] private Player playerPreFab;
    [SerializeField] private Transform cam;
    [SerializeField] private Dictionary<Vector3, Tile> tiles;
    public int width, height, level;
    public Player player;

    private void Start()
    {
        /*this.width = PlayerPrefs.GetInt("gridWidth");
        this.height = PlayerPrefs.GetInt("gridHeight");
        this.level = PlayerPrefs.GetInt("gridHardness");*/
        this.CreateGrid();
        this.SetPlayer();
    }

    public void CreateGrid()
    {
        this.tiles = new Dictionary<Vector3, Tile>();
        GameObject gameObject = new GameObject("GridObject", typeof(Grid));
        Transform transform = gameObject.transform;
        transform.SetParent(null, false);
        transform.localPosition = new Vector3(0, 0, 0);

        for(int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                var newTile = Instantiate(this.tilePreFab, new Vector3(i, j, 0), Quaternion.identity);
                newTile.name = $"Tile {i} {j}";
                newTile.Init(this, Random.Range(1, 100) > 100 - this.level);
                this.tiles[new Vector3(i, j)] = newTile;
            }
        }

        this.cam.transform.position = new Vector3((float)this.width / 2 - 0.5f, (float)this.height / 2 - 0.5f, - 10);
    }

    public void SetPlayer()
    {
        Tile playerTile = this.GetTileAtPosition(new Vector3(Random.Range(0, this.width), Random.Range(0, this.height)));
        this.player = Instantiate(this.playerPreFab, playerTile.GetPosition() + new Vector3(0, 0, 1), Quaternion.identity);
        this.player.Init(playerTile, this);
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if(tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }
        return null;
    }

    public void SetSelectedTile(Vector2 position)
    {
        if (this.selectedTile) this.selectedTile.UnselectTile();
        this.selectedTile = this.GetTileAtPosition(position);
        this.selectedTile.SelectTile();
        DijkstraManager dm = new DijkstraManager(this);
        this.player.SetPath(dm.Dijkstra(this.player.atTile.GetPosition(), this.selectedTile.GetPosition()));
        dm = null;

    }
    public float GetHeight()
    {
        return this.height;
    }
    public float GetWidth()
    {
        return this.width;
    }
}
