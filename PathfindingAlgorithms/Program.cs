using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Diagnostics;
namespace PathfindingAlgorithms
{
    class Program
    {
        
        static void printPath(Graph graph, List<Node> path, int startX, int startY)
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
                        if (i == startX && j == startY)
                            Console.ForegroundColor = ConsoleColor.Blue;
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
            Random rand = new Random();
            int totalD = 0;
            int totalA = 0;
            for (int j = 0; j < 1000; j++)
            {
                Graph graph = new Graph(10, 10);


                int x1, y1, x2, y2; // set random start and finish points
                x1 = rand.Next(10);
                x2 = rand.Next(10);
                y1 = rand.Next(10);
                y2 = rand.Next(10);

                for (int i = 0; i < 20; i++) //remove 20 nodes from graph that arent the start or finish
                {
                    int x = rand.Next(10);
                    int y = rand.Next(10);
                    if ((x == x1 && y == y1) || (x == x2 && y == y2))
                        continue;
                    graph.popNode(x, y);
                }

                //print path found via Dijkstra and A* highlighting in red the unvisited nodes
                Stopwatch timer = new Stopwatch(); //time each algorithm in ticks

                timer.Start();
                var path = graph.DijkstraShortestPath(graph.nodes[x1, y1], graph.nodes[x2, y2]);
                Console.WriteLine("Ticks: " + timer.ElapsedTicks);
                totalD += Convert.ToInt32(timer.ElapsedTicks);
                printPath(graph, path, x1, y1);
                Console.WriteLine("Count: " + path.Count);
                Console.WriteLine();

                timer.Restart();
                path = graph.AStarShortestPath(graph.nodes[x1, y1], graph.nodes[x2, y2]);
                Console.WriteLine("Ticks: " + timer.ElapsedTicks);
                totalA += Convert.ToInt32(timer.ElapsedTicks);
                timer.Reset();
                printPath(graph, path, x1, y1);
                Console.WriteLine("Count: " + path.Count);

                Console.WriteLine();
                Console.ReadKey();
                Console.Clear();
            }
            Console.WriteLine("average Dijkstra: " + totalD / 100);
            Console.WriteLine("average AStar: " + totalA / 100);
        }
    }
}