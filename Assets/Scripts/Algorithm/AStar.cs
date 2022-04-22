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
    
    public Astar(Graph Graph)
    {
        this.Graph = Graph;
    }
    public void InitializeList()
    {
        AuxiliarList = new AstarItem[Graph.Size];

        for (int i = 0; i < Graph.Size; i++)
        {
            Vertex v = Graph.GetVertex(i);
            AuxiliarList[v.Id] = new AstarItem(v);
            //AuxiliarList[v.Id].HeuristicCost = EuclidieanHeuristic();
        }
    }
    public List<int> FindPath(int start, int target)
    {
        Vertex vertexTarget = Graph.GetVertex(target);
        InitializeList();

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
    /*
    static public float EuclidieanHeuristic(int actual, int target)
    {
        //return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(actualCord.x - targetCord.x), 2) + Mathf.Pow(Mathf.Abs(actualCord.y - targetCord.y), 2));//FIX
    }
    */
}

