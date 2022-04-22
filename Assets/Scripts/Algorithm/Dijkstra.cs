using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraItem
{
    public Vertex Vertex { get; set; }
    public Vertex PreviousVertex { get; set; }
    public float TotalCost { get; set; }
    public bool Open { get; set; }


    public DijkstraItem(Vertex vertex)
    {
        Vertex = vertex;
        PreviousVertex = null;
        TotalCost = (float) int.MaxValue;
        Open = true;
    }
}

public class Dijkstra : IPathFinder
{
    private DijkstraItem[] AuxiliarList;
    public Graph Graph { get; private set; }

    private int width;
    private int height;

    public Dijkstra(Graph Graph)
    {
        this.Graph = Graph;
        this.width = Graph.Grid.Width;
        this.height = Graph.Grid.Height;
    }


    public void InitializeList()
    {
        AuxiliarList = new DijkstraItem[(width * height)];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                AuxiliarList[(i * height) + j] = new DijkstraItem(Graph.GetVertex(i, j));
            }
        }        
    }

    public List<TileController> FindPath(Vector3 originCord, Vector3 targetCord)
    {
        Vertex target = Graph.GetVertex((int)targetCord.x, (int)targetCord.y);
        this.InitializeList();

        this.AuxiliarList[(int)((originCord.x * height) + originCord.y)].TotalCost = 0;
        while (true)
        {
            int menor = int.MaxValue;
            DijkstraItem item = null;
            for (int i = 0; i < this.AuxiliarList.Length; i++)
            {
                if (menor > this.AuxiliarList[i].TotalCost && this.AuxiliarList[i].Open)
                {
                    item = this.AuxiliarList[i];
                    menor = (int) item.TotalCost;
                }
            }
            if(item == null) return null;
            item.Open = false;
            if (item.Vertex == target) return this.GetShortestPath(originCord, targetCord);

            string[] directions = { "U", "R", "D", "L" };

            foreach (string d in directions)
            {
                if (item.Vertex.Edges.ContainsKey(d))
                {
                    Vector2 nextItemPosition = item.Vertex.Edges[d].GetTo(item.Vertex).tile.GetPosition();
                    if (AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].Open)
                    {
                        DijkstraItem nextItem = AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
                        if ((item.Vertex.Edges[d].cost + item.TotalCost) < nextItem.TotalCost)
                        {
                            nextItem.TotalCost = item.Vertex.Edges[d].cost + item.TotalCost;
                            nextItem.PreviousVertex = item.Vertex;
                        }
                    }
                }
            }
        }
    }

    public List<TileController> GetShortestPath(Vector2 originCord, Vector2 targetCord)
    {
        DijkstraItem originItem = AuxiliarList[(int)((originCord.x * height) + originCord.y)];
        DijkstraItem item = AuxiliarList[(int)((targetCord.x * height) + targetCord.y)];

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
                if (!AuxiliarList[(i * height) + j].Open) count++;
            }
        }
        return count;
    }

}

