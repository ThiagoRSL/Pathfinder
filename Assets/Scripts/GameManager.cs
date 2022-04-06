using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public sealed class GameManager : MonoBehaviour
{

    //Lazy Singleton Implementation
    private static readonly Lazy<GameManager> lazy = new Lazy<GameManager>(() => new GameManager());

    public static GameManager Instance { get { return lazy.Value; } }

    private GameManager() { }
    //
    [SerializeField, Range(10, 100)]
    public int gridWidth = 10;
    [SerializeField, Range(10, 100)]
    public int gridHeight = 10;
    [SerializeField, Range(10, 100)]
    public int gridComplexity = 10;
    [SerializeField] 
    private Player playerPreFab;
    [SerializeField] 
    private Transform cam;

    public Player Player { get; set; }
    public Grid Grid { get; set; }
    public Graph Graph { get; set; }

    private void Start()
    {
        Grid = new Grid(gridWidth, gridHeight, gridComplexity);
        RenderGrid();
        Graph = new Graph(Grid);
        SetPlayer();
        cam.transform.position = new Vector3((float)Grid.Width / 2 - 0.5f, (float) Grid.Height / 2 - 0.5f, -10); ;
    }


    public void RenderGrid()
    {
        GameObject gameObject = new GameObject("GridObject", typeof(Grid));
        Transform transform = gameObject.transform;
        transform.SetParent(null, false);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void SetPlayer()
    {
        Tile playerTile = Grid.GetTileAtPosition(new Vector3(Random.Range(0, Grid.Width), Random.Range(0, Grid.Height)));
        this.Player = Instantiate(this.playerPreFab, playerTile.GetPosition() + new Vector3(0, 0, 1), Quaternion.identity);
        this.Player.Init(playerTile);
    }
}
