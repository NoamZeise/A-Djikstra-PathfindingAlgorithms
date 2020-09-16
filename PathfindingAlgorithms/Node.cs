using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingAlgorithms
{
    class Node
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Visited = false;
        public int Weight = 10000000;
        public int Manhatten = 10000000;
        public Node previous = default;
        public Dictionary<Node, int> Neighbours;
        public Node() { }
        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Reset()
        {
            Weight = 10000000;
            Visited = false;
            Manhatten = 10000000;
            previous = default;
        }

        public void setCoords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}