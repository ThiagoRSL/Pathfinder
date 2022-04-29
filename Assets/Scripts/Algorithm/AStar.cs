using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : IPathFinder
{
    //Debug
    private List<Vertex> VisitedList;
    //
    private List<Vertex> ClosedList;
    private List<Vertex> OpenList;

    private double[] F; // Total cost
    private double[] G; // Move cost
    private double[] H; // Heuristic
    public Vertex[] PreviousVertex;

    public Graph MapGraph { get; private set; }
    private Vector2 PositionStart;
    private List<Vector2> ClosedOrder;
    private int Width;


    public Astar(Graph Graph, int width)
    {
        MapGraph = Graph;
        Width = width;

        VisitedList = new List<Vertex>();
        OpenList = new List<Vertex>();
        ClosedList = new List<Vertex>();

        F = new double[Graph.Size];
        G = new double[Graph.Size];
        H = new double[Graph.Size];
        PreviousVertex = new Vertex[Graph.Size];
    }
    public List<int> FindPath(int start, int target)
    {
        Debug.Log(target);
        ResetLists(start, target);

        PositionStart = IdToPosition(start);

        Vertex vertexTarget = MapGraph.GetVertex(target);
        Vertex CurrentVertex = null;

        double menor;
        while (true)
        {
            menor = double.MaxValue;
            int current = int.MaxValue;
            OpenList.ForEach(delegate (Vertex vertex)
            {
                //Debug.Log("Vertex: " + vertex.Id.ToString() + " Heuristic " + H[vertex.Id].ToString() + " Function: " + ManhattanHeuristic(vertex.Id, target).ToString());
                if (menor > F[vertex.Id])
                {
                    current = vertex.Id;
                    menor = F[vertex.Id];
                    CurrentVertex = vertex;
                }
            });

            if (CurrentVertex == null) return null;

            //Debug.Log("Escolhido V�rtice: " + CurrentVertex.Id.ToString());
            //Debug.Log(" -------------------------------------------------- ");
            OpenList.Remove(CurrentVertex);
            ClosedList.Add(CurrentVertex);

            if (CurrentVertex == vertexTarget) return GetShortestPath(target);

            string[] directions = { "U", "R", "D", "L" };

            foreach (string d in directions)
            {
                if (CurrentVertex.HasEdge(d))
                {
                    Vertex NextVertex = CurrentVertex.GetAdjacent(d);

                    double newCost = G[CurrentVertex.Id] + CurrentVertex.GetEdge(d).Cost;

                    if (G[NextVertex.Id] > newCost)
                    {
                        G[NextVertex.Id] = newCost;

                        //Works with ManhattanHeuristic(NextVertex.Id, target), but not with H[NextVertex.Id]. Why?
                        F[NextVertex.Id] = G[NextVertex.Id] + ManhattanHeuristic(NextVertex.Id, target); //H[NextVertex.Id];
                        PreviousVertex[NextVertex.Id] = CurrentVertex;

                        if (OpenList.Find(v => v.Id == NextVertex.Id) is null) OpenList.Add(NextVertex);
                    }
                    if (VisitedList.Find(v => v.Id == NextVertex.Id) is null) VisitedList.Add(NextVertex);

                }
            }
        }
    }

    //
    public double ManhattanHeuristic(int current, int target)
    {

        Vector2 positionCurrent = IdToPosition(current);
        Vector2 positionTarget = IdToPosition(target);

        double dx = Math.Abs(positionCurrent.x - positionTarget.x);
        double dy = Math.Abs(positionCurrent.y - positionTarget.y);
        double heuristic = (dx + dy);

        //Tie-breaker
        double dxs = Math.Abs(PositionStart.x - positionTarget.x);
        double dys = Math.Abs(PositionStart.y - positionTarget.y);
        double cross = Math.Abs(dx * dys - dy * dxs);
        heuristic += cross * 0.01;

        return heuristic;
    }

    //Debug
    public List<Vector2> GetVisited()
    {
        List<Vector2> list = new List<Vector2>();
        VisitedList.ForEach(delegate (Vertex v) {
            list.Add(IdToPosition(v.Id));
        });
        return list;
    }
    public List<Vector2> GetOpened()
    {
        List<Vector2> list = new List<Vector2>();
        OpenList.ForEach(delegate (Vertex v) {
            list.Add(IdToPosition(v.Id));
        });
        return list;
    }
    public List<Vector2> GetClosed()
    {
        List<Vector2> list = new List<Vector2>();
        ClosedList.ForEach(delegate (Vertex v) {
            list.Add(IdToPosition(v.Id));
        });
        return list;
    }
    //

    //Auxiliar
    private void ResetLists(int start, int target)
    {
        VisitedList.Clear();
        OpenList.Clear();
        ClosedList.Clear();

        for (int i = 0; i < MapGraph.Size; i++)
        {
            F[i] = double.MaxValue;
            G[i] = double.MaxValue;
            H[i] = ManhattanHeuristic(i, target);
            PreviousVertex[i] = null;
        }

        G[start] = 0;
        F[start] = G[start] + H[start];
        OpenList.Add(MapGraph.GetVertex(start));

    }

    private List<int> GetShortestPath(int target)
    {
        List<int> path = new List<int>();
        int vertexId = MapGraph.GetVertex(target).Id;

        while (vertexId >= 0 && PreviousVertex.Length > vertexId && PreviousVertex[vertexId] != null)
        {
            path.Add(vertexId);
            vertexId = PreviousVertex[vertexId].Id;
        }

        path.Reverse();
        return path;
    }

    private Vector2 IdToPosition(int val)
    {
        return new Vector2((val % Width), Mathf.Floor(val / Width));
    }

}

