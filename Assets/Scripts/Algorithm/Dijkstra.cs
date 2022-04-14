using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Dijkstra : IPathFinder
{
    private DijkstraItem[] AuxiliarList;
    public Graph Graph { get; private set; }

    private int width;
    private int height;

    public Dijkstra(Graph Graph)
    {
        this.Graph = Graph;
        this.width = Graph.Grid.Width;
        this.height = Graph.Grid.Height;
    }


    public void InitializeList()
    {
        AuxiliarList = new DijkstraItem[(width * height)];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                AuxiliarList[(i * height) + j] = new DijkstraItem(Graph.GetVertex(i, j));
            }
        }        
    }

    public List<TileController> FindPath(Vector3 originCord, Vector3 targetCord)
    {
        Vertex target = Graph.GetVertex((int)targetCord.x, (int)targetCord.y);
        this.InitializeList();

        this.AuxiliarList[(int)((originCord.x * height) + originCord.y)].totalCost = 0;
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

            if (item.vertex.north != null)
            {
                Vector2 nextItemPosition = item.vertex.north.GetTo(item.vertex).tile.GetPosition();
                if (this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].open)
                {
                    DijkstraItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
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
                if (this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].open)
                {
                    DijkstraItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
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
                if (this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].open)
                {
                    DijkstraItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
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
                if (this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)].open)
                {
                    DijkstraItem nextItem = this.AuxiliarList[(int)((nextItemPosition.x * height) + nextItemPosition.y)];
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

    public List<TileController> GetShortestPath(Vector2 originCord, Vector2 targetCord)
    {
        DijkstraItem originItem = AuxiliarList[(int)((originCord.x * height) + originCord.y)];
        DijkstraItem item = AuxiliarList[(int)((targetCord.x * height) + targetCord.y)];

        List<TileController> path = new List<TileController>();
        Vertex v = AuxiliarList[(int)((targetCord.x * height) + targetCord.y)].vertex;
        Vector2 position;

        while (item != originItem && item != null)
        {
            position = item.previousVertex.tile.GetPosition();
            path.Add(item.vertex.tile);
            item = AuxiliarList[(int)((position.x * height) + position.y)];
        }
        path.Reverse();
        return path;
    }
}

