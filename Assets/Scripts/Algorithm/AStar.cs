using System;
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
    private int Width;
    
    public Astar(Graph Graph, int width)
    {
        Width = width;
        Graph = Graph;
    }
    public void InitializeList(int target)
    {
        Debug.Log(Graph.Size);
        AuxiliarList = new AstarItem[Graph.Size];
        for (int i = 0; i < Graph.Size; i++)
        {
            Debug.Log(i);
            Vertex v = Graph.GetVertex(i);
            AuxiliarList[v.Id] = new AstarItem(v);
            AuxiliarList[v.Id].HeuristicCost = ManhattanHeuristic(v.Id, target);
        }
    }
    public List<int> FindPath(int start, int target)
    {
        InitializeList(target);
        Vertex vertexTarget = Graph.GetVertex(target);

        AuxiliarList[start].SetInitial();
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
                        AstarItem nextItem = AuxiliarList[nextVertex.Id];
                        if ((item.Vertex.GetEdge(d).Cost + item.TotalCost) < (nextItem.TotalCost - nextItem.HeuristicCost))
                        {
                            nextItem.TotalCost = item.Vertex.GetEdge(d).Cost + item.TotalCost + nextItem.HeuristicCost;
                            nextItem.PreviousVertex = item.Vertex;
                        }
                    }
                }
            }
           
        }
    }

    public List<int> GetShortestPath(int start, int target)
    {
        AstarItem originItem = AuxiliarList[start];
        AstarItem item = AuxiliarList[target];

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
    public float ManhattanHeuristic(int actual, int target)
    {
        int actualX = actual % Width;
        int actualY = (actual - actualX) / Width;

        int targetX = target % Width;
        int targetY = (target - targetX) / Width;

        return Math.Abs(actualX - targetX) + Math.Abs(actualY - targetY);
    }
}

