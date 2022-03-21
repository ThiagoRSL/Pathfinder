using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Vertex
{
    Tile* tile;
    float elevation;

    Arris north;
    Arris east;
    Arris west;
    Arris south;

    public Vertex(Tile tile)
    {
        this.tile = tile;
        this.elevation = tile.GetElevation();

        this.north = null;
        this.east = null;
        this.west = null;
        this.south = null;
    }
}

struct Arris
{
    Vertex* from;
    Vertex* to;
    float cost;

    public Arris(Vertex from, Vertex to)
    {
        this.from = from;
        this.to = to;
        this.cost = Mathf.Abs(from.elevation - to.elevation);
    }
}

struct DijkstraItem
{
    Vertex* vertex;
    Vertex* previousVertex;
    float totalCost;
    bool open;

    public DijkstraItem(Vertex vertex)
    {
        this.vertex = vertex;
        this.previousVertex = null;
        this.totalCost = Mathf.infinity;
        this.open = true;
    }
}

public class DijkstraManager
{
    private GridManager grid;
    private Vertex[][] vertexes;
    private DijkstraItem[] AuxiliarList;

    unsafe public void DijkstraManager(GridManager grid)
    {
        this.grid = grid;
        this.MapVertexes();
        this.MapArrises();
        this.InitializeList();
    }

    unsafe public void MapVertexes()
    {
        this.vertexes = new Vertex[width][height];

        //Mapping the Vertexes
        for (int i = 0; i < this.grid.getWidth(); i++)
        {
            for (int j = 0; j < this.grid.GetHeight(); j++)
            {
                this.vertexes[i][j] = new Vertex(grid.GetTileAtPosition(new Vector2(i, j)));
            }
        }
    }
    unsafe public void MapArrises()
    {
        float width = this.grid.getWidth();
        float height = this.grid.GetHeight();
        
        //Mapping the Arrises
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i > 0) this.vertexes[i][j].west = new Arris(this.vertexes[i][j], this.vertexes[i - 1][j]);
                if (i < (width - 1)) this.vertexes[i][j].east = new Arris(this.vertexes[i][j], this.vertexes[i + 1][j]);
                if (j > 0) this.vertexes[i][j].north = new Arris(this.vertexes[i][j], this.vertexes[i][j - 1]);
                if (j < (height - 1)) this.vertexes[i][j].south = new Arris(this.vertexes[i][j], this.vertexes[i][j + 1]);
            }
        }
    }

    unsafe public void InitializeList()
    {
        float width = this.grid.getWidth();
        float height = this.grid.GetHeight();
        this.AuxiliarList = new DijkstraItem[width*height];

        for (int i = 0; i < this.grid.getWidth(); i++)
        {
            for (int j = 0; j < this.grid.GetHeight(); j++)
            {
                this.AuxiliarList[(i*25) + j] = new DijkstraItem(this.vertexes[i][j]);
            }
        }
    }

    unsafe public void Dijkstra(Vertex origin, Vertex target)
    {
        this.InitializeList();
        DijkstraItem item = null;
        while (true)
        {
            int menor = Mathf.infinity;
            for(int i = 0; i < this.AuxiliarList.Length; i++)
            {
                if (!item.open)
                {
                    break;
                }
                if (menor > this.AuxiliarList[i].cost)
                {
                    item = this.AuxiliarList[i];
                }
            }
            if(item == null) break;


            if (item.vertex.north)
            {
                Vector3 toPosition = item.vertex.north.to.tile.GetPosition();
                DijkstraItem toItem = this.AuxiliarList[(toPosition.x * 25) + (toPosition.y)];
                if ((item.vertex.north.cost + item.totalCost) < (toItem.totalCost))
                {
                    toItem.totalCost = (item.vertex.north.cost + item.totalCost);
                    toItem.previousVertex = item.vertex;
                }
            }
            if (item.vertex.east)
            {
                Vector3 toPosition = item.vertex.east.to.tile.GetPosition();
                DijkstraItem toItem = this.AuxiliarList[(toPosition.x * 25) + (toPosition.y)];
                if ((item.vertex.east.cost + item.totalCost) < (toItem.totalCost))
                {
                    toItem.totalCost = (item.vertex.east.cost + item.totalCost);
                    toItem.previousVertex = item.vertex;
                }
            }
            if (item.vertex.west)
            {
                Vector3 toPosition = item.vertex.west.to.tile.GetPosition();
                DijkstraItem toItem = this.AuxiliarList[(toPosition.x * 25) + (toPosition.y)];
                if ((item.vertex.west.cost + item.totalCost) < (toItem.totalCost))
                {
                    toItem.totalCost = (item.vertex.west.cost + item.totalCost);
                    toItem.previousVertex = item.vertex;
                }
            }
            if (item.vertex.south)
            {
                Vector3 toPosition = item.vertex.south.to.tile.GetPosition();
                DijkstraItem toItem = this.AuxiliarList[(toPosition.x * 25) + (toPosition.y)];
                if ((item.vertex.south.cost + item.totalCost) < (toItem.totalCost))
                {
                    toItem.totalCost = (item.vertex.south.cost + item.totalCost);
                    toItem.previousVertex = item.vertex;
                }
            }
            item.open = false;
        }
    }
}


/*
struct Graph
{
    Tile  tile;
    Graph* north;
    Graph* east;
    Graph* south;
    Graph* west;

    float elevation;
    bool open;
    float cost;

    public Graph(Grid grid, float x, float y)
    {
        this.tile = grid.GetTileAtPosition(new Vector2(x, y));
        this.elevation = tile.GetElevation();
        this.cost = Mathf.infinity;
        Graph* north;
        Graph* east;
        Graph* south;
        Graph* west;
    }
}*/
