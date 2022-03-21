using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height, _level;

    [SerializeField] private Tile _tilePreFab, _selectedTile;

    [SerializeField] private Player _player;

    [SerializeField] private Transform _cam;

    [SerializeField] private Dictionary<Vector3, Tile> _tiles;

    private void Start()
    {
        this.CreateGrid();
        this.SetPlayer();
    }

    public void CreateGrid()
    {
        this._tiles = new Dictionary<Vector3, Tile>();
        GameObject gameObject = new GameObject("GridObject", typeof(Grid));
        Transform transform = gameObject.transform;
        transform.SetParent(null, false);
        transform.localPosition = new Vector3(0, 0, 0);

        for(int i = 0; i < this._width; i++)
        {
            for (int j = 0; j < this._height; j++)
            {
                var newTile = Instantiate(this._tilePreFab, new Vector3(i, j, 0), Quaternion.identity);
                newTile.name = $"Tile {i} {j}";
                newTile.Init(this, Random.Range(1, 100) > 100 - this._level);
                this._tiles[new Vector3(i, j)] = newTile;
            }
        }

        this._cam.transform.position = new Vector3((float)this._width / 2 - 0.5f, (float)this._height / 2 - 0.5f, - 10);
    }

    public void SetPlayer()
    {
        var player = Instantiate(this._player, (this.GetTileAtPosition(new Vector3(Random.Range(0, this._width), Random.Range(0, this._height))).GetPosition() + new Vector3(0,0,1)), Quaternion.identity);
       // player.Init(this.GetTileAtPosition(new Vector2(Random.Range(0, this._width), Random.Range(0, this._height))), this);
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if(_tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }
        return null;
    }

    public void SetSelectedTile(Vector2 position)
    {
        if(this._selectedTile) this._selectedTile.UnselectTile();
        this._selectedTile = this.GetTileAtPosition(position);
        this._selectedTile.SelectTile();
    }
    public float GetHeight()
    {
        return this._height;
    }
    public float GetWidth()
    {
        return this._width;
    }
}
