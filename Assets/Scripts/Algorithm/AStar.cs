using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarItem
{
    public Vertex Vertex { get; set; }
    public Vertex PreviousVertex { get; set; }
    public double TotalCost { get; set; }
    public double HeuristicCost { get; set; }
    public bool Open { get; set; }

    public AstarItem(Vertex vertex)
    {
        Vertex = vertex;
        PreviousVertex = null;
        TotalCost = double.MaxValue;
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
    public Graph MapGraph { get; private set; }
    private int Width;
    private Vector2 positionStart;


    public Astar(Graph Graph, int width)
    {
        MapGraph = Graph;
        Width = width;
    }
    public void InitializeList(int target)
    {
        AuxiliarList = new AstarItem[MapGraph.Size];
        for (int i = 0; i < MapGraph.Size; i++)
        {
            Vertex v = MapGraph.GetVertex(i);
            AuxiliarList[v.Id] = new AstarItem(v);
            AuxiliarList[v.Id].HeuristicCost = ManhattanHeuristic(v.Id, target);
        }
    }
    public List<int> FindPath(int start, int target)
    {
        positionStart = IdToPosition(start);
        InitializeList(target);
        Vertex vertexTarget = MapGraph.GetVertex(target);

        AuxiliarList[start].SetInitial();
        while (true)
        {
            double menor = double.MaxValue;
            AstarItem item = null;
            for (int i = 0; i < AuxiliarList.Length; i++)
            {
                if (AuxiliarList[i].Open && menor > AuxiliarList[i].TotalCost)
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
                    AstarItem nextItem = AuxiliarList[nextVertex.Id];
                    if (nextItem.Open)
                    {
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

    public List<Vector2> CountClosed()
    {
        List<Vector2> vet = new List<Vector2>();
        int count = 0;
        for (int i = 0; i < MapGraph.Size; i++)
        {
            if (!AuxiliarList[i].Open) 
            {
                count++;
                vet.Add(IdToPosition(i));
            };
            
        }
        Debug.Log("Astar closed nodes: " + count);
        return vet;
    }

    public double ManhattanHeuristic(int actual, int target)
    {

        Vector2 positionActual = IdToPosition(actual);
        Vector2 positionTarget = IdToPosition(target);

        double dx = Math.Abs(positionActual.x - positionTarget.x);
        double dy = Math.Abs(positionActual.y - positionTarget.y);
        double heuristic = (dx + dy) * 0.1;

        //Tie-breaker
        double dxs = Math.Abs(positionStart.x - positionTarget.x);
        double dys = Math.Abs(positionStart.y - positionTarget.y);
        double cross = Math.Abs((dx * dys) - (dxs * dy));
        heuristic += cross * 0.01;

        return heuristic;
    }

    private Vector2 IdToPosition(int val){
        return new Vector2((val % Width), Mathf.Floor(val / Width));
    }
}

