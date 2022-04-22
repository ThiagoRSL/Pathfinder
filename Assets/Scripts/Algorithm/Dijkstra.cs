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
        TotalCost = (float)int.MaxValue;
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
    }

    public void InitializeList()
    {
        AuxiliarList = new DijkstraItem[Graph.Size];

        for (int i = 0; i < Graph.Size; i++)
        {
            Vertex v = Graph.GetVertex(i);
            AuxiliarList[v.Id] = new DijkstraItem(v);
        }
    }

    public List<int> FindPath(int start, int target)
    {
        Vertex vertexTarget = Graph.GetVertex(target);
        this.InitializeList();

        this.AuxiliarList[start].TotalCost = 0;
        while (true)
        {
            int menor = int.MaxValue;
            DijkstraItem item = null;
            for (int i = 0; i < AuxiliarList.Length; i++)
            {
                if (menor > AuxiliarList[i].TotalCost && AuxiliarList[i].Open)
                {
                    item = AuxiliarList[i];
                    menor = (int)item.TotalCost;
                }
            }
            if (item == null) return null;
            item.Open = false;
            if (item.Vertex == vertexTarget) return this.GetShortestPath(start, target);

            string[] directions = { "U", "R", "D", "L" };

            foreach (string d in directions)
            {
                if (item.Vertex.HasEdge(d))
                {
                    Vertex nextVertex = item.Vertex.GetAdjacent(d);
                    if (AuxiliarList[nextVertex.Id].Open)
                    {
                        DijkstraItem nextItem = AuxiliarList[nextVertex.Id];
                        if ((item.Vertex.GetEdge(d).Cost + item.TotalCost) < nextItem.TotalCost)
                        {
                            nextItem.TotalCost = item.Vertex.GetEdge(d).Cost + item.TotalCost;
                            nextItem.PreviousVertex = item.Vertex;
                        }
                    }
                }
            }
        }
    }

    public List<int> GetShortestPath(int start, int target)
    {
        DijkstraItem originItem = AuxiliarList[start];
        DijkstraItem item = AuxiliarList[target];

        List<int> path = new List<int>();
        Vertex v = AuxiliarList[target].Vertex;

        while (item != originItem && item != null)
        {
            int PreviousId = item.PreviousVertex.Id;
            path.Add(item.Vertex.Id);
            item = AuxiliarList[PreviousId];
        }
        path.Reverse();
        return path;
    }
    public int CountClosed()
    {
        int count = 0;
        for (int i = 0; i < Graph.Size; i++)
        {
            if (!AuxiliarList[i].Open) count++;
        }
        return count;
    }

}

