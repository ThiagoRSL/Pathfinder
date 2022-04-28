using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarItem : IEquatable<AstarItem>
{
    public Vertex Vertex { get; set; }
    public AstarItem PreviousItem { get; set; }
    public double TotalCost { get; set; }
    public double HeuristicCost { get; set; }

    public AstarItem(Vertex vertex, double heuristicCost)
    {
        Vertex = vertex;
        PreviousItem = null;
        TotalCost = double.MaxValue;
        HeuristicCost = heuristicCost;
    }
    public void SetTotalCost(double val)
    {
        TotalCost = val + HeuristicCost;
    }
    public override int GetHashCode()
    {
        return Vertex.Id;
    }
    public bool Equals(AstarItem other)
    {
        if (other == null) return false;
        return (this.Vertex.Id.Equals(other.Vertex.Id));
    }
}

public class Astar : IPathFinder
{
    private List<AstarItem> ClosedList;
    private List<AstarItem> OpenList;
    public Graph MapGraph { get; private set; }
    private int Width;
    private Vector2 positionStart;
    private List<Vector2> ClosedOrder;


    public Astar(Graph Graph, int width)
    {
        MapGraph = Graph;
        Width = width;
        ClosedOrder = new List<Vector2>();
        ClosedList = new List<AstarItem>();
        OpenList = new List<AstarItem>();
    }
    public List<int> FindPath(int start, int target)
    {
        ClosedOrder.Clear();
        ClosedList.Clear();
        OpenList.Clear();

        positionStart = IdToPosition(start);
        Vertex vertexStart = MapGraph.GetVertex(start);
        Vertex vertexTarget = MapGraph.GetVertex(target);

        AstarItem starter = new AstarItem(vertexStart, ManhattanHeuristic(vertexStart.Id, target) * 100);
        starter.SetTotalCost(0);
        OpenList.Add(starter);
        //vertexTarget.DebugVertex();

        double menor;
        while (true)
        {
            menor = double.MaxValue;
            AstarItem item = null;
            for (int i = 0; i < OpenList.Count; i++)
            {
                //Debug.Log("Menor: " + menor.ToString());
                //Debug.Log("Cursto " + OpenList[i].Vertex.Id.ToString() + " : " + OpenList[i].TotalCost.ToString());
                
                if (menor > OpenList[i].TotalCost)
                {
                    item = OpenList[i];
                    menor = item.TotalCost;
                }
            }
            if (item == null)
            {
                Debug.Log("Deu nulo");
                return null;
            }
            //Debug.Log("Escolhido: " + item.Vertex.Id);

            //Debug

            OpenList.Remove(item);
            ClosedList.Add(item);
            ClosedOrder.Add(IdToPosition(item.Vertex.Id));

            if (item.Vertex == vertexTarget) return GetShortestPath();

            string[] directions = { "U", "R", "D", "L" };

            foreach (string d in directions)
            {
                if (item.Vertex.HasEdge(d))
                {
                    //Acumulava infinitamente o custo de maneira que os vertices pertos do ponto inicial sempre tinha menor peso.
                    //Transformar em dicionarios os custos totais, custo heuristico e o item anterior
                    Vertex nextVertex = item.Vertex.GetAdjacent(d);
                    AstarItem newItem = new AstarItem(nextVertex, ManhattanHeuristic(nextVertex.Id, target));

                    if (!OpenList.Contains(newItem) && !ClosedList.Contains(newItem))
                    {
                        OpenList.Add(newItem);
                    }
                    if ((item.Vertex.GetEdge(d).Cost + item.TotalCost - item.HeuristicCost) < (newItem.TotalCost - newItem.HeuristicCost)) 
                    {
                        newItem.SetTotalCost(item.Vertex.GetEdge(d).Cost + item.TotalCost - item.HeuristicCost);
                        newItem.PreviousItem = item;
                    }
                }
            }
        }
    }

    public List<int> GetShortestPath()
    {
        AstarItem originItem = ClosedList[0];
        AstarItem item = ClosedList[(ClosedList.Count-1)];

        List<int> path = new List<int>();

        while (item != originItem && item != null)
        {
            path.Add(item.Vertex.Id);
            item = item.PreviousItem;
        }
        path.Reverse();
        Debug.Log(OpenList.Count);
        Debug.Log(ClosedList.Count);
        return path;
    }

    public List<Vector2> GetClosedOrder()
    {        
        return ClosedOrder;
    }
    public double ManhattanHeuristic(int actual, int target)
    {

        Vector2 positionActual = IdToPosition(actual);
        Vector2 positionTarget = IdToPosition(target);

        double dx = Math.Abs(positionActual.x - positionTarget.x);
        double dy = Math.Abs(positionActual.y - positionTarget.y);
        double heuristic = (dx + dy) * 0.01;

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

