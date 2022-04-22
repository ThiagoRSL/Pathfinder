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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                graph.SetVertex(IndexFrom2D(i, j), new Vertex(IndexFrom2D(i, j)));
            }
        }
    }

    static public void MapEdges(Graph graph)
    {
        //Mapping the edges
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!graph.GetVertex(IndexFrom2D(i, j)).HasEdge("L") && i > 0)
                {
                    Vertex vertA = graph.GetVertex(IndexFrom2D(i, j));
                    Vertex vertB = graph.GetVertex(IndexFrom2D(i - 1, j));

                    Edge arris = new Edge(vertA, vertB, Unevenness((i - 1), j, i, j));
                    vertA.SetEdge("L", arris);
                    vertB.SetEdge("R", arris);
                }
                if (!graph.GetVertex(IndexFrom2D(i, j)).HasEdge("R") && i < (width - 1))
                {
                    Vertex vertA = graph.GetVertex(IndexFrom2D(i, j));
                    Vertex vertB = graph.GetVertex(IndexFrom2D(i + 1, j));

                    Edge arris = new Edge(vertA, vertB, Unevenness((i + 1), j, i, j));
                    vertA.SetEdge("R", arris);
                    vertB.SetEdge("L", arris);
                }
                if (!graph.GetVertex(IndexFrom2D(i, j)).HasEdge("U") && j < (height - 1))
                {
                    Vertex vertA = graph.GetVertex(IndexFrom2D(i, j));
                    Vertex vertB = graph.GetVertex(IndexFrom2D(i, j + 1));

                    Edge arris = new Edge(vertA, vertB, Unevenness(i, j, i, (j + 1)));

                    vertA.SetEdge("U", arris);
                    vertB.SetEdge("D", arris);
                }
                if (!graph.GetVertex(IndexFrom2D(i, j)).HasEdge("D") && j > 0)
                {
                    Vertex vertA = graph.GetVertex(IndexFrom2D(i, j));
                    Vertex vertB = graph.GetVertex(IndexFrom2D(i, j - 1));

                    Edge arris = new Edge(vertA, vertB, Unevenness(i, j, i, (j - 1)));

                    vertA.SetEdge("D", arris);
                    vertB.SetEdge("U", arris);
                }
            }
        }
    }
    static private float Unevenness(float i1, float j1, float i2, float j2)
    {
        return TileController.Unevenness(Grid.GetTileAtPosition(new Vector2(i1, j1)), Grid.GetTileAtPosition(new Vector2(i2, j2)));
    }
    static public int IndexFrom2D(int i, int j) { return (j * Grid.Width) + i; }
}