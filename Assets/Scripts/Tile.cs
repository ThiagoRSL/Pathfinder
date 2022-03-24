using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private GridManager _grid;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private bool _blocked;
    [SerializeField] private bool _selected;
    [SerializeField] private GameObject _highlight;

    public void Init(GridManager grid, bool hasElevation)
    {
        if(hasElevation)
        {
            this.SetElevation(Random.Range(-3,3));
        }
        else
        {
            this.SetElevation(0);
        }
        this._grid = grid;
    }
    public void UnselectTile()
    {
        this._selected = false;
        this.SetColor();
    }
    public void SelectTile()
    {
        this._selected = true;
        this._renderer.color = this._selectedColor;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
    public float GetElevation()
    {
        return this.transform.position.z;
    }
    public void SetElevation(int elevation)
    {
        this.transform.position = this.transform.position + (new Vector3(0, 0, elevation));
        this.SetColor();
    }

    private void SetColor(){
        Color32 color = this.DefColor(this.transform.position);
        this._renderer.color = color;
    }
    void OnMouseEnter()
    {
        if (!this._blocked && !this._selected) this.Highlight(true); ;
    }
    void OnMouseExit()
    {
        if (!this._blocked) this.Highlight(false);
    }
    void OnMouseDown()
    {
        this.Highlight(false);
        if (!this._blocked) this._grid.SetSelectedTile(this.transform.position);
    }
    public void Highlight(bool active)
    {
        this._highlight.SetActive(active);
    }
    private Color32 DefColor(Vector3 position) => position.z switch
    {
        -3 => new Color32(0, 0, 0, 255),
        -2 => new Color32(40, 40, 40, 255),
        -1 => new Color32(80, 80, 80, 255),
        0 => new Color32(120, 120, 120, 255),
        1 => new Color32(160, 160, 160, 255),
        2 => new Color32(200, 200, 200, 255),
        3 => new Color32(240, 240, 240, 255),
    };

}

