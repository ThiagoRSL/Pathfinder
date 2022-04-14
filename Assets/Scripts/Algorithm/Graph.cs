using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public TileController tile;
    public float elevation;

    public Edge north;
    public Edge east;
    public Edge west;
    public Edge south;

    public Vertex(TileController tile)
    {
        this.tile = tile;
        this.elevation = tile.GetElevation();

        this.north = null;
        this.east = null;
        this.west = null;
        this.south = null;
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
                if (this.vertexes[i, j].west == null && i > 0)
                {
                    Edge arris = new Edge(this.vertexes[i, j], this.vertexes[i - 1, j]);
                    this.vertexes[i, j].west = arris;
                    this.vertexes[i - 1, j].east = arris;
                }
                if (this.vertexes[i, j].east == null && i < (width - 1))
                {
                    Edge arris = new Edge(this.vertexes[i, j], this.vertexes[i + 1, j]);
                    this.vertexes[i, j].east = arris;
                    this.vertexes[i + 1, j].west = arris;
                }
                if (this.vertexes[i, j].north == null && j < (height - 1))
                {
                    Edge arris = new Edge(this.vertexes[i, j], this.vertexes[i, j + 1]);
                    this.vertexes[i, j].north = arris;
                    this.vertexes[i, j + 1].south = arris;
                }
                if (this.vertexes[i, j].south == null && j > 0)
                {
                    Edge arris = new Edge(this.vertexes[i, j], this.vertexes[i, j - 1]);
                    this.vertexes[i, j].south = arris;
                    this.vertexes[i, j - 1].north = arris;
                }
            }
        }
    }
    public Vertex GetVertex(int i, int j)
    {
        return this.vertexes[i, j];
    }
}
