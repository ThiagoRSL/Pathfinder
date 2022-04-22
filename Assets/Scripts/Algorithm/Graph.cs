using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public TileController tile;
    public float elevation;
    public Dictionary<string, Edge> Edges;

    public Vertex(TileController tile)
    {
        this.tile = tile;
        this.elevation = tile.GetElevation();
        this.Edges = new Dictionary<string, Edge>();
    }
}

public class Edge
{
    public Vertex vertexA;
    public Vertex vertexB;
    public float cost;

    public Edge(Vertex a, Vertex b)
    {
        this.vertexA = a;
        this.vertexB = b;
        this.cost = 1 + Mathf.Abs(a.elevation - b.elevation);//Substituir o custo pela hipotenusa
    }
    public Vertex GetTo(Vertex from)
    {
        if(from == this.vertexA) return this.vertexB;
        else return this.vertexA;
    }
}

public class Graph
{
    public Grid Grid { get; private set; }
    private Vertex[,] vertexes;

    private int width;
    private int height;

    public Graph(Grid grid)
    {
        this.Grid = grid;
        this.width = Grid.Width;
        this.height = Grid.Height;

        MapVertexes();
        MapEdges();
    }

    public void MapVertexes()
    {
        //Mapping the Vertexes, 
        this.vertexes = new Vertex[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                this.vertexes[i, j] = new Vertex(Grid.GetTileAtPosition(new Vector2(i, j))); 
            }
        }
    }
    public void MapEdges()
    {
        //Mapping the edges
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!vertexes[i, j].Edges.ContainsKey("L") && i > 0)
                {
                    Edge arris = new Edge(this.vertexes[i, j], this.vertexes[i - 1, j]);
                    vertexes[i, j].Edges.Add("L", arris);
                    vertexes[i - 1, j].Edges.Add("R", arris);
                }
                if (!vertexes[i, j].Edges.ContainsKey("R") && i < (width - 1))
                {
                    Edge arris = new Edge(this.vertexes[i, j], this.vertexes[i + 1, j]);
                    this.vertexes[i, j].Edges.Add("R", arris);
                    this.vertexes[i + 1, j].Edges.Add("L", arris);
                }
                if (!vertexes[i, j].Edges.ContainsKey("U") && j < (height - 1))
                {
                    Edge arris = new Edge(this.vertexes[i, j], this.vertexes[i, j + 1]);
                    this.vertexes[i, j].Edges.Add("U", arris);
                    this.vertexes[i, j + 1].Edges.Add("D", arris);
                }
                if (!vertexes[i, j].Edges.ContainsKey("D") && j > 0)
                {
                    Edge arris = new Edge(this.vertexes[i, j], this.vertexes[i, j - 1]);
                    this.vertexes[i, j].Edges.Add("D", arris);
                    this.vertexes[i, j - 1].Edges.Add("U", arris);
                }
            }
        }
    }
    public Vertex GetVertex(int i, int j)
    {
        return this.vertexes[i, j];
    }
}
