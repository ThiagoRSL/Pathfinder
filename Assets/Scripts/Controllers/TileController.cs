using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileController : MonoBehaviour
{
    [SerializeField] 
    private Color selectedColor;
    [SerializeField] 
    private SpriteRenderer renderer;
    [SerializeField] 
    private GameObject highlight, pathHighlight;
    private EntityController Atop;
    private bool selected;
    private bool path;

    public void SetAtop(EntityController entity) { Atop = entity; }

    public void Init(bool hasElevation, int i, int j)
    {
        if (hasElevation)
        {
            SetElevation(Random.Range(-3, 4));
        }
        else
        {
            SetElevation(0);
        }
    }
    public void UnselectTile()
    {
        this.selected = false;
        this.SetColor();
    }
    public void SelectTile()
    {
        this.selected = true;
        this.renderer.color = this.selectedColor;
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

    private void SetColor()
    {
        Color32 color = this.DefColor(this.transform.position);
        this.renderer.color = color;
    }
    public void Highlight(bool active)
    {
        this.highlight.SetActive(active);
    }

    public void SetPath()
    {
        this.path = true;
        this.pathHighlight.SetActive(true);
    }
    public void UnsetPath()
    {
        this.pathHighlight.SetActive(false);
        this.path = false;
    }


    void OnMouseEnter()
    {
        this.Highlight(true);
    }
    void OnMouseExit()
    {
        this.Highlight(false);
    }
    void OnMouseDown()
    {
        this.Highlight(false);
        GameManager.Instance.Player.DefinePath(this.transform.position);
    }

    private Color32 DefColor(Vector3 position) => position.z switch
    {
        -3 => new Color32(0, 60, 80, 255),
        -2 => new Color32(0, 120, 160, 255),
        -1 => new Color32(0, 180, 240, 255),
        0 => new Color32(125, 125, 125, 255),
        1 => new Color32(240, 0, 0, 255),
        2 => new Color32(160, 0, 0, 255),
        3 => new Color32(80, 0, 0, 255),
    };

    public static float Unevenness(TileController a, TileController b)
    {
        return 1+Mathf.Abs(a.GetElevation() - b.GetElevation());
    }
}

