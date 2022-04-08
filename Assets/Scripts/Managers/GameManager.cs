using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[DisallowMultipleComponent]
public sealed class GameManager : MonoBehaviour
{

    //Lazy Singleton Implementation
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        if(instance is null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
        SetGame();
    }
    //

    [SerializeField]
    public Tile tilePreFab;
    [SerializeField] 
    private Player playerPreFab;
    [SerializeField] 
    private Transform cam;

    public Player Player { get; private set; }
    public Grid Grid { get; private set; }
    public Graph Graph { get; private set; }

    private int gridWidth;
    private int gridHeight;
    private int gridComplexity;

    public int GridWidth { get { return gridWidth; } }
    public int GridHeight { get { return gridHeight; } }
    public int GridComplexity { get { return gridComplexity; }  }

    public void SetGridWidth(string val) { int.TryParse(val, out gridWidth); }
    public void SetGridHeight(string val) { int.TryParse(val, out gridHeight); }
    public void SetGridComplexity(string val) { int.TryParse(val, out gridComplexity); }

    public void SetGame()
    {
        Grid = new Grid(10, 10, 10);// Grid = new Grid(GridWidth, GridHeight, GridComplexity);
        // RenderGrid();
        Graph = new Graph(Grid);
        RenderPlayer();
        cam.transform.position = new Vector3( (float) Grid.Width / 2 - 0.5f, (float) Grid.Height / 2 - 0.5f, -10); ;
    }

    //public void RenderGrid()   {    }

    public void RenderPlayer()
    {
        Tile playerTile = Grid.GetTileAtPosition(new Vector3(Random.Range(0, Grid.Width), Random.Range(0, Grid.Height)));
        Player = Instantiate(playerPreFab, playerTile.GetPosition() + new Vector3(0, 0, 1), Quaternion.identity);
        Player.Init(playerTile);
    }
}
