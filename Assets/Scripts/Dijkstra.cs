using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public Tile tile;
    public float elevation;

    public Arris north;
    public Arris east;
    public Arris west;
    public Arris south;

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

public class Arris
{
    public Vertex vertexA;
    public Vertex vertexB;
    public float cost;

    public Arris(Vertex a, Vertex b)
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

public class DijkstraItem
{
    public Vertex vertex;
    public Vertex previousVertex;
    public float totalCost;
    public bool open;

    public DijkstraItem(Vertex vertex)
    {
        this.vertex = vertex;
        this.previousVertex = null;
        this.totalCost = (float) int.MaxValue;
        this.open = true;
    }
}

public class DijkstraManager
{
    private GridManager grid;
    private Vertex[,] vertexes;
    private DijkstraItem[] AuxiliarList;

    public DijkstraManager(GridManager grid)
    {
        this.grid = grid;
        this.MapVertexes();
        this.MapArrises();
        this.InitializeList();
        Vector2 playerPosition = grid.player.transform.position;
        Debug.Log("AAA");
        Vertex playerVertex = this.AuxiliarList[(int)((playerPosition.x * grid._width) + playerPosition.y)].vertex;
        Debug.Log(playerVertex.north.cost);
        Debug.Log(playerVertex.south.cost);
        Debug.Log(playerVertex.east.cost);
        Debug.Log(playerVertex.west.cost);
    }

    public void MapVertexes()
    {
        //Mapping the Vertexes, 
        this.vertexes = new Vertex[(int)this.grid.GetWidth(), (int)this.grid.GetHeight()];
        for (int i = 0; i < this.grid.GetWidth(); i++)
        {
            for (int j = 0; j < this.grid.GetHeight(); j++)
            {
                this.vertexes[i,j] = new Vertex(this.grid.GetTileAtPosition(new Vector2(i, j)));
            }
        }
    }
     public void MapArrises()
    {
        float width = (int)this.grid.GetWidth();
        float height = (int)this.grid.GetHeight();

        //Mapping the Arrises
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (this.vertexes[i, j].west == null &&  i > 0)
                {
                    Arris arris = new Arris(this.vertexes[i, j], this.vertexes[i - 1, j]);
                    this.vertexes[i, j].west = arris;
                    this.vertexes[i - 1, j].east = arris;
                }
                if (this.vertexes[i, j].east == null && i < (width - 1))
                {
                    Arris arris = new Arris(this.vertexes[i, j], this.vertexes[i + 1, j]);
                    this.vertexes[i, j].east = arris;
                    this.vertexes[i + 1, j].west = arris;
                }
                if (this.vertexes[i, j].north == null && j < (height - 1))
                {
                    Arris arris = new Arris(this.vertexes[i, j], this.vertexes[i,j + 1]);
                    this.vertexes[i, j].north = arris;
                    this.vertexes[i, j + 1].south = arris;
                }
                if (this.vertexes[i, j].south == null && j > 0)
                {
                    Arris arris = new Arris(this.vertexes[i, j], this.vertexes[i,j - 1]);
                    this.vertexes[i, j].south = arris;
                    this.vertexes[i,j - 1].north = arris;
                }
            }
        }
    }

    public void InitializeList()
    {
        int width = (int) this.grid.GetWidth();
        int height = (int) this.grid.GetHeight();
        this.AuxiliarList = new DijkstraItem[(width*height)];

        for (int i = 0; i < (this.grid.GetWidth()); i++)
        {
            for (int j = 0; j < (this.grid.GetHeight()); j++)
            {
                this.AuxiliarList[(i*width) + j] = new DijkstraItem(this.vertexes[i, j]);
            }
        }
    }

    public List<Tile> Dijkstra(Vector3 originCord, Vector3 targetCord)
    {
        float width = (int)this.grid.GetWidth();
        Vertex target = this.vertexes[(int)targetCord.x,(int)targetCord.y];
        this.InitializeList();

        this.AuxiliarList[(int)((originCord.x * width) + originCord.y)].totalCost = 0;
        while (true)
        {
            int menor = int.MaxValue;
            DijkstraItem item = null;
            for (int i = 0; i < this.AuxiliarList.Length; i++)
            {
                if (menor > this.AuxiliarList[i].totalCost && this.AuxiliarList[i].open)
                {
                    item = this.AuxiliarList[i];
                    menor = (int) item.totalCost;
                }
            }
            if(item == null) break;
            //Debug.Log(item.vertex.tile.GetPosition().x);
            //Debug.Log(item.vertex.tile.GetPosition().y);


            if (item.vertex.north != null)
            {
                Vector2 nextItemPosition = item.vertex.north.GetTo(item.vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x* width) + nextItemPosition.y)].open)
                {
                    DijkstraItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * width) + nextItemPosition.y)];
                    if ((item.vertex.north.cost + item.totalCost) < (nextItem.totalCost))
                    {
                        nextItem.totalCost = (item.vertex.north.cost + item.totalCost);
                        nextItem.previousVertex = item.vertex;
                    }
                }
            }
            if (item.vertex.east != null)
            {
                Vector2 nextItemPosition = item.vertex.east.GetTo(item.vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x * width) + nextItemPosition.y)].open)
                {
                    DijkstraItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * width) + nextItemPosition.y)];
                    if ((item.vertex.east.cost + item.totalCost) < (nextItem.totalCost))
                    {
                        nextItem.totalCost = (item.vertex.east.cost + item.totalCost);
                        nextItem.previousVertex = item.vertex;
                    }
                }
            }
            if (item.vertex.west != null)
            {
                Vector2 nextItemPosition = item.vertex.west.GetTo(item.vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x * width) + nextItemPosition.y)].open)
                {
                    DijkstraItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * width) + nextItemPosition.y)];
                    if ((item.vertex.west.cost + item.totalCost) < (nextItem.totalCost))
                    {
                        nextItem.totalCost = (item.vertex.west.cost + item.totalCost);
                        nextItem.previousVertex = item.vertex;
                    }
                }
            }
            if (item.vertex.south != null)
            {
                Vector2 nextItemPosition = item.vertex.south.GetTo(item.vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x * width) + nextItemPosition.y)].open)
                {
                    DijkstraItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * width) + nextItemPosition.y)];
                    if ((item.vertex.south.cost + item.totalCost) < (nextItem.totalCost))
                    {
                        nextItem.totalCost = (item.vertex.south.cost + item.totalCost);
                        nextItem.previousVertex = item.vertex;
                    }
                }
            }
            item.open = false;
        }
        return this.GetShortestPath(originCord, targetCord);
    }

    public List<Tile> GetShortestPath(Vector2 originCord, Vector2 targetCord)
    {
        float width = (int) this.grid.GetWidth();
        DijkstraItem originItem = this.AuxiliarList[(int)((originCord.x * width) + originCord.y)];
        DijkstraItem item = this.AuxiliarList[(int)((targetCord.x * width) + targetCord.y)];

        List<Tile> path = new List<Tile>();
        Vertex v = this.AuxiliarList[(int)((targetCord.x * width) + targetCord.y)].vertex;
        Vector2 position;

        while (item != originItem && item != null)
        {
            //Debug.Log(item.previousVertex);
            position = item.previousVertex.tile.GetPosition();
            path.Add(item.vertex.tile);
            item = this.AuxiliarList[(int)((position.x * width) + position.y)];
        }
        path.Reverse();
        return path;
    }
}

