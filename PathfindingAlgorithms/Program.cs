using System;
using System.Collections.Generic;
using System.Dynamic;

namespace PathfindingAlgorithms
{
    class Program
    {
        static void printPath(Graph graph, List<Node> path)
        {
            for (int i = 0; i < graph._width; i++)
            {
                for (int j = 0; j < graph._height; j++)
                {
                    bool inPath = false;
                    foreach (var node in path)
                    {
                        if (node == graph.nodes[i, j])
                            inPath = true;
                    }
                    if (inPath)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("X ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (graph.nodes[i, j].Neighbours.Count > 0)
                    {
                        if (!graph.nodes[i, j].Visited)
                            Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("x ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.Write("  ");
                }
                Console.WriteLine();
            }
        }
        static void printWeights(Graph graph)
        {
            for (int i = 0; i < graph._width; i++)
            {
                for (int j = 0; j < graph._height; j++)
                {
                    if (graph.nodes[i, j].Visited)
                        Console.Write(graph.nodes[i, j].Weight + " |");
                    else
                       Console.Write("X |");
                }
                Console.WriteLine();
            }
        }
        static void printManhatten(Graph graph)
        {
            for (int i = 0; i < graph._width; i++)
            {
                for (int j = 0; j < graph._height; j++)
                {
                    // if (graph.nodes[i, j].Visited)
                    Console.Write(graph.nodes[i, j].Manhatten + " |");
                    //else
                    //     Console.Write("X |");
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Graph graph = new Graph(10, 10);
                Random rand = new Random();
                for (int i = 0; i < 20; i++)
                {
                    int x = rand.Next(10);
                    int y = rand.Next(10);
                    if (x == 0 && y == 0)
                        continue;
                    if (x == 9 && y == 9)
                        continue;
                    graph.popNode(x, y);
                }
                var path = graph.DijkstraShortestPath(graph.nodes[0, 0], graph.nodes[9, 9]);
                printPath(graph, path);
                Console.WriteLine();
                path = graph.AStarShortestPath(graph.nodes[0, 0], graph.nodes[9, 9]);
                printPath(graph, path);
                Console.WriteLine();
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}