using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;


[DisallowMultipleComponent]
public sealed class GameManager : MonoBehaviour
{

    //Lazy Singleton Implementation
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    //

    [SerializeField]
    public TileController tilePreFab;
    [SerializeField]
    private PlayerController playerPreFab;

    public PlayerController Player { get; private set; }
    public GridController Grid { get; private set; }

    private int gridWidth = 10;
    private int gridHeight = 10;
    private int gridComplexity = 25;
    private int PFAlgorithm = 0;

    public int GridWidth { get { return gridWidth; } }
    public int GridHeight { get { return gridHeight; } }
    public int GridComplexity { get { return gridComplexity; } }

    public void SetGridWidth(string val) { int.TryParse(val, out gridWidth); }
    public void SetGridHeight(string val) { int.TryParse(val, out gridHeight); }
    public void SetGridComplexity(string val) { int.TryParse(val, out gridComplexity); }
    public void SetPFAlgorithm(Dropdown change) { PFAlgorithm = change.value; }

    public void SetGame()
    {
        SetGrid();
        SetPathfinder();
        SetPlayer();
        SetCamera();
    }

    public void SetGrid()
    {
        GameObject gameObject = new GameObject("GridObject");
        Transform transform = gameObject.transform;
        transform.SetParent(null, false);
        transform.localPosition = new Vector3(0, 0, 0);
        Grid = gameObject.AddComponent<GridController>();
        Grid.Init(gridWidth, gridHeight, gridComplexity, tilePreFab);
    }

    public void SetPlayer()
    {
        TileController playerTile = Grid.GetTileAtPosition(new Vector2(Random.Range(0, gridWidth), Random.Range(0, gridHeight)));
        Player = Instantiate(playerPreFab, playerTile.GetPosition() + new Vector3(0, 0, 1), Quaternion.identity);
        Player.Init(playerTile, Grid);
        Grid.PutEntity(Player, playerTile);
    }

    public void SetPathfinder()
    {
        /*switch (PFAlgorithm) 
        {
            case 1: //"Astar"
                Grid.Pathfinder = new Astar(Grid.MakeGraph());
                break;
            case 0: //"Dijkstra"
            default:
                Grid.Pathfinder = new Dijkstra(Grid.MakeGraph());
                break;
        }*/

    }

    public void SetCamera()
    {
        Camera.main.transform.position = new Vector3((float)gridWidth / 2 - 0.5f, (float)gridHeight / 2 - 0.5f, -10);
        Camera.main.orthographicSize = 10;
    }


    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            SetGame();
        }
    }
    public void DebugPath(List<Vector2> positions)
    {
        StartCoroutine(ShowNode(positions));
    }

    IEnumerator ShowNode(List<Vector2> positions)
    {
        List<Vector2> PaintedPositions = new List<Vector2>();
        while (positions.Count > 0)
        {
            Grid.GetTileAtPosition(positions[0]).Paint(new Color32(255, 0, 0, 255));
            PaintedPositions.Add(positions[0]);
            positions.RemoveAt(0);
            yield return new WaitForSeconds(0.05f);
        }
        ClearPositions(PaintedPositions);
    }
    public void ClearPositions(List<Vector2> positions)
    {
        while (positions.Count > 0)
        {
            Grid.GetTileAtPosition(positions[0]).UnPaint();
            positions.RemoveAt(0);
        }
    }
}
