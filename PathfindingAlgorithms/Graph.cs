using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingAlgorithms
{
    class Graph
    {
        static Random rand = new Random();
        public Node[,] nodes { get; private set; }
        public int _width { get; private set; }
        public int _height { get; private set; }

        public Graph(int width, int height) //builds a square grid of given W and H
        {
            _width = width;
            _height = height;
            nodes = new Node[_width, _height];
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    nodes[i, j] = new Node(i, j);
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                {
                    Dictionary<Node, int> neighDict = new Dictionary<Node, int>();
                    //add relavent neighbours, skip if on either limit of grid
                    if (i > 0)
                        neighDict.Add(nodes[i - 1, j], 1);
                    if (i < _width - 1)
                        neighDict.Add(nodes[i + 1, j], 1);
                    if (j > 0)
                        neighDict.Add(nodes[i, j - 1], 1);
                    if (j < _height - 1)
                        neighDict.Add(nodes[i, j + 1], 1);
                    nodes[i, j].Neighbours = neighDict;
                }
        }

        public void popNode(int x, int y)
        {
            foreach (var node in nodes) //remove all references of node in neighbour lists
            {
                if (node.Neighbours.ContainsKey(nodes[x, y]))
                    node.Neighbours.Remove(nodes[x, y]);
            }
            nodes[x, y].Neighbours.Clear(); //clear specified node's neighbours list
        }

        public List<Node> DijkstraShortestPath(Node start, Node finish)
        {
            List<Node> nodeQ = new List<Node>();
            foreach (var node in nodes)
            {
                if (node.Neighbours.Count == 0)
                    continue;
                node.Reset();
                nodeQ.Add(node);
            }
            start.Visited = true;
            start.Weight = 0;
            Node shortest = new Node();
            bool breakLoop = false;
            while (true)
            {
                //get shortest weight node that hasn't visited all it's neighbours
                shortest = new Node();
                bool foundUnvisited = false;
                foreach (var node in nodeQ)
                {
                    if (node.Weight < shortest.Weight)
                        foreach (var link in node.Neighbours)
                            if (!link.Key.Visited)
                            {
                                foundUnvisited = true;
                                shortest = node;
                            }
                    if(node.Weight == shortest.Weight)
                    if (rand.Next() % 2 == 0)
                        foreach (var link in node.Neighbours)
                            if (!link.Key.Visited)
                            {
                                shortest = node;
                                break;
                            }
                }
                if (!foundUnvisited)
                    break;
                //calculate weight of links
                foreach (var link in shortest.Neighbours)
                {
                    if (link.Key.Visited)
                        continue;
                    link.Key.Visited = true;
                    link.Key.Weight = shortest.Weight + link.Value;
                    link.Key.previous = shortest;
                    if (link.Key == finish)
                    {
                        shortest = finish;
                        breakLoop = true;
                        break;
                    }
                }
                if (breakLoop)
                    break;
            }
            List<Node> path = new List<Node>();
            while (shortest.previous != default)
            {
                path.Add(shortest);
                shortest = shortest.previous;
            }
            if (path.Count == 0)
                Console.WriteLine("NO PATH FOUND");
            path.Add(start);
            path.Reverse();
            return path;
        }

        public List<Node> AStarShortestPath(Node start, Node finish)
        {
            List<Node> nodeQ = new List<Node>();
            //add all nodes to node queue and 
            //calculate their manhatten distance to the goal
            foreach (var node in nodes)
            { 
                if (node.Neighbours.Count == 0)
                    continue;
                node.Reset();
                node.Manhatten = calcManhatten(node, finish);
                nodeQ.Add(node);
            } 
            start.Weight = 0; //set start weight to zero, so it is explored first
            start.Visited = true;
            Node shortest;
            while(true)
            {
                shortest = new Node();
                foreach(var node in nodes)
                { //assign lowest value node to shortest
                    if (node.Weight + node.Manhatten < shortest.Weight + shortest.Manhatten)
                        foreach (var link in node.Neighbours)
                            if (!link.Key.Visited)
                            {
                                shortest = node;
                                break;
                            }
                    if (node.Weight + node.Manhatten == shortest.Weight + shortest.Manhatten)
                    { //randomly select if there are multiple lowest value paths
                        if(rand.Next() % 2 == 0)
                        foreach (var link in node.Neighbours)
                            if (!link.Key.Visited)
                            {
                                shortest = node;
                                break;
                            }
                    }
                }
                //if the shortest node is an uncalculated node there must be no vaild paths
                if (shortest.Weight >= new Node().Weight) 
                    break;
                bool finishfound = false;
                foreach(var link in shortest.Neighbours)
                {
                    if (link.Key.Visited)
                        continue;
                    link.Key.Visited = true;
                    link.Key.Weight = shortest.Weight + link.Value;
                    link.Key.previous = shortest;
                    if(link.Key == finish) //if one of the neigbours are the end node then a path has been found
                    {
                        shortest = finish;
                        finishfound = true;
                    }
                }
                if(finishfound) //end node found
                    break;
            }
            List<Node> path = new List<Node>();
            //go through shortest node to previously visted node to get path from finish->start
            while (shortest.previous != default)
            { //start value will have default previous node, so means start has been traced back to 
                path.Add(shortest);
                shortest = shortest.previous;
                if (shortest.Weight >= new Node().Weight)
                { //if node has default value then there is no available path
                    path.Clear();
                    break;
                }
            }
            if (path.Count == 0)
                Console.WriteLine("NO PATH FOUND");
            path.Add(start);
            path.Reverse(); //so is ordered from start->finish
            return path;
        }

        int calcManhatten(Node node1, Node node2) =>
             Math.Abs(node1.X - node2.X) + Math.Abs(node1.Y - node2.Y);

       
    }
}