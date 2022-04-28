using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GridToGraph
{
    private static Grid Grid;
    private static int width;
    private static int height;

    static public Graph Convert(Grid grid)
    {
        Grid = grid;
        width = grid.Width;
        height = grid.Height;
        int size = width * height;

        Graph graph = new Graph(size);
        TilesToVertexes(graph);
        MapEdges(graph);

        return graph;
    }

    static public void TilesToVertexes(Graph graph)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int id = IndexFrom2D(x, y);
                graph.SetVertex(id, new Vertex(id));
            }
        }
    }

    static public void MapEdges(Graph graph)
    {
        //Mapping the edges
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!graph.GetVertex(IndexFrom2D(x, y)).HasEdge("L") && x > 0)
                {
                    Vertex vertA = graph.GetVertex(IndexFrom2D(x, y));
                    Vertex vertB = graph.GetVertex(IndexFrom2D(x - 1, y));

                    Edge arris = new Edge(vertA, vertB, Unevenness(x, y, x - 1, y));
                    vertA.SetEdge("L", arris);
                    vertB.SetEdge("R", arris);
                }
                if (!graph.GetVertex(IndexFrom2D(x, y)).HasEdge("R") && x < (width - 1))
                {
                    Vertex vertA = graph.GetVertex(IndexFrom2D(x, y));
                    Vertex vertB = graph.GetVertex(IndexFrom2D(x + 1, y));

                    Edge arris = new Edge(vertA, vertB, Unevenness(x, y, x + 1, y));
                    vertA.SetEdge("R", arris);
                    vertB.SetEdge("L", arris);
                }
                if (!graph.GetVertex(IndexFrom2D(x, y)).HasEdge("U") && y < (height - 1))
                {
                    Vertex vertA = graph.GetVertex(IndexFrom2D(x, y));
                    Vertex vertB = graph.GetVertex(IndexFrom2D(x, y + 1));

                    Edge arris = new Edge(vertA, vertB, Unevenness(x, y, x, y + 1));

                    vertA.SetEdge("U", arris);
                    vertB.SetEdge("D", arris);
                }
                if (!graph.GetVertex(IndexFrom2D(x, y)).HasEdge("D") && y > 0)
                {
                    Vertex vertA = graph.GetVertex(IndexFrom2D(x, y));
                    Vertex vertB = graph.GetVertex(IndexFrom2D(x, y - 1));

                    Edge arris = new Edge(vertA, vertB, Unevenness(x, y, x, y - 1));

                    vertA.SetEdge("D", arris);
                    vertB.SetEdge("U", arris);
                }
            }
        }
    }
    static private float Unevenness(float x1, float y1, float x2, float y2)
    {
        return TileController.Unevenness(Grid.GetTileAtPosition(new Vector2(x1, y1)), Grid.GetTileAtPosition(new Vector2(x2, y2)));
    }
    static public int IndexFrom2D(int x, int y) { return x + (y*Grid.Height); }
}