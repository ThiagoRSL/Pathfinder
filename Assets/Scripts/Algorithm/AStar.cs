using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarItem
{
    public Vertex Vertex { get; set; }
    public Vertex PreviousVertex { get; set; }
    public float TotalCost { get; set; }
    public float HeuristicCost { get; set; }
    public bool Open { get; set; }

    public AstarItem(Vertex vertex)
    {
        Vertex = vertex;
        PreviousVertex = null;
        TotalCost = float.MaxValue;
        Open = true;
    }
    public void SetInitial()
    {
        TotalCost = 0 + HeuristicCost;
    }
}

public class Astar : IPathFinder
{
    private AstarItem[] AuxiliarList;
    public Graph Graph { get; private set; }

    private int width;
    private int height;

    public Astar(Graph Graph)
    {
        this.Graph = Graph;
        this.width = Graph.Grid.Width;
        this.height = Graph.Grid.Height;
    }


    public void InitializeList(Vector2 targetCord)
    {
        AuxiliarList = new AstarItem[(width * height)];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                AuxiliarList[(i * height) + j] = new AstarItem(Graph.GetVertex(i, j));
                AuxiliarList[(i * height) + j].HeuristicCost = Astar.EuclidieanHeuristic(new Vector3(i, j), targetCord) * (float) 0.1;
            }
        }        
    }

    public List<TileController> FindPath(Vector3 originCord, Vector3 targetCord)
    {
        Vertex target = Graph.GetVertex((int)targetCord.x, (int)targetCord.y);
        InitializeList(targetCord);

        AuxiliarList[(int)((originCord.x * height) + originCord.y)].SetInitial();
        while (true)
        {
            float menor = float.MaxValue;
            AstarItem item = null;
            for (int i = 0; i < AuxiliarList.Length; i++)
            {
                if (menor > AuxiliarList[i].TotalCost && AuxiliarList[i].Open)
                {
                    item = AuxiliarList[i];
                    menor = item.TotalCost;
                }
            }
            if (item == null) return null;
            if (item.Vertex == target) return this.GetShortestPath(originCord, targetCord);

            if (item.Vertex.Up != null)
            {
                Vector2 nextItemPosition = item.Vertex.Up.GetTo(item.Vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].Open)
                {
                    AstarItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
                    if ((item.Vertex.Up.cost + item.TotalCost) < (nextItem.TotalCost - nextItem.HeuristicCost))
                    {
                        nextItem.TotalCost = item.Vertex.Up.cost + item.TotalCost + nextItem.HeuristicCost;
                        nextItem.PreviousVertex = item.Vertex;
                    }
                }
            }
            if (item.Vertex.Right != null)
            {
                Vector2 nextItemPosition = item.Vertex.Right.GetTo(item.Vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].Open)
                {
                    AstarItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
                    if ((item.Vertex.Right.cost + item.TotalCost) < (nextItem.TotalCost - nextItem.HeuristicCost))
                    {
                        nextItem.TotalCost = item.Vertex.Right.cost + item.TotalCost + nextItem.HeuristicCost;
                        nextItem.PreviousVertex = item.Vertex;
                    }
                }
            }
            if (item.Vertex.Left != null)
            {
                Vector2 nextItemPosition = item.Vertex.Left.GetTo(item.Vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].Open)
                {
                    AstarItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
                    if ((item.Vertex.Left.cost + item.TotalCost) < (nextItem.TotalCost - nextItem.HeuristicCost))
                    {
                        nextItem.TotalCost = item.Vertex.Left.cost + item.TotalCost + nextItem.HeuristicCost;
                        nextItem.PreviousVertex = item.Vertex;
                    }
                }
            }
            if (item.Vertex.Down != null)
            {
                Vector2 nextItemPosition = item.Vertex.Down.GetTo(item.Vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].Open)
                {
                    AstarItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
                    if ((item.Vertex.Down.cost + item.TotalCost) < (nextItem.TotalCost - nextItem.HeuristicCost))
                    {
                        nextItem.TotalCost = item.Vertex.Down.cost + item.TotalCost + nextItem.HeuristicCost;
                        nextItem.PreviousVertex = item.Vertex;
                    }
                }
            }
            item.Open = false;
        }
    }

    public List<TileController> GetShortestPath(Vector2 originCord, Vector2 targetCord)
    {;
        AstarItem originItem = AuxiliarList[(int)((originCord.x * height) + originCord.y)];
        AstarItem item = AuxiliarList[(int)((targetCord.x * height) + targetCord.y)];

        List<TileController> path = new List<TileController>();
        Vertex v = AuxiliarList[(int)((targetCord.x * height) + targetCord.y)].Vertex;
        Vector2 position;

        while (item != originItem && item != null)
        {
            position = item.PreviousVertex.tile.GetPosition();
            path.Add(item.Vertex.tile);
            item = AuxiliarList[(int)((position.x * height) + position.y)];
        }
        path.Reverse();
        return path;
    }

    public int CountClosed()
    {
        int count = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(!AuxiliarList[(i * height) + j].Open) count++;
            }
        }
        return count;
    }

    static public float EuclidieanHeuristic(Vector2 actualCord, Vector2 targetCord)
    {
        return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(actualCord.x - targetCord.x), 2) + Mathf.Pow(Mathf.Abs(actualCord.y - targetCord.y), 2));
    }
}

