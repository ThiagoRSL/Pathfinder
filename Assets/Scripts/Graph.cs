using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public int Id { get; private set; }
    private Dictionary<string, Edge> Edges;

    public Vertex(int id)
    {
        Id = id;
        Edges = new Dictionary<string, Edge>();
    }
    public bool HasEdge(string key)
    {
        return Edges.ContainsKey(key);
    }
    public void SetEdge(string key, Edge edge)
    {
        Edges.Add(key, edge);
    }
    public Edge GetEdge(string key)
    {
        return Edges[key];
    }
    public Vertex GetAdjacent(string key)
    {
        if (Edges.TryGetValue(key, out Edge edge))
        {
            return edge.GetOtherVertex(this);
        }
        return null;
    }
}

public class Edge
{
    public Vertex VertexA;
    public Vertex VertexB;
    public float Cost { get; private set; }

    public Edge(Vertex a, Vertex b, float cost)
    {
        VertexA = a;
        VertexB = b;
        Cost = cost;
    }
    public Vertex GetOtherVertex(Vertex from)
    {
        if(from == this.VertexA) return this.VertexB;
        else return this.VertexA;
    }
}

public class Graph
{
    private Vertex[] Vertexes;
    public int Size { get; private set; }

    public Graph(int size)
    {
        Debug.Log(size);
        Size = size;
        Vertexes = new Vertex[size];
    }

    public Vertex GetVertex(int x)
    {
        return Vertexes[x];
    }
    public void SetVertex(int x, Vertex vertex)
    {
        Vertexes[x] = vertex;
    }
}