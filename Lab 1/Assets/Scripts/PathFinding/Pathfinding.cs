using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// A* needs only a WeightedGraph and a Node type L, and does *not*
public interface WeightedGraph<T>
{
    double Cost(Node a, Node b);
    IEnumerable<Node> Neighbors(Node id);
}

public struct Node
{
    // Implementation notes: I am using the default Equals but it can
    // be slow. You'll probably want to override both Equals and
    // GetHashCode in a real project.

    public readonly int x, y;
 
    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}


public class SquareGrid : WeightedGraph<Node>
{
    // Implementation notes: I made the fields public for convenience,
    // but in a real project you'll probably want to follow standard
    // style and make them private.

    public static readonly Node[] DIRECTIONS = new[]
        {
            new Node(1, 0),
            new Node(0, -1),
            new Node(-1, 0),
            new Node(0, 1)
        };

    public int width, height;
    //public HashSet<Node> walls = new HashSet<Node>();
    //public HashSet<Node> forests = new HashSet<Node>();
    public HashSet<Node> mountains = new HashSet<Node>();

    public SquareGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public bool InBounds(Node id)
    {
        return 0 <= id.x && id.x < width
            && 0 <= id.y && id.y < height;
    }

    /*public bool Passable(Node id)
    {
        return !walls.Contains(id);
    }*/

    public double Cost(Node a, Node b)
    {
        return mountains.Contains(b) ? 5 : 1;
    }

    public IEnumerable<Node> Neighbors(Node id)
    {
        foreach (var dir in DIRECTIONS)
        {
            Node next = new Node(id.x + dir.x, id.y + dir.y);
            if (InBounds(next)/* && Passable(next)*/)
            {
                yield return next;
            }
        }
    }
}

public class Tuple<T1, T2>
{
    public T1 Item1 { get; private set; }
    public T2 Item2 { get; private set; }

    internal Tuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}

public static class Tuple
{
    public static Tuple<T1, T2> New<T1, T2>(T1 item1, T2 item2)
    {
        var tuple = new Tuple<T1, T2>(item1, item2);
        return tuple;
    }
}

public class PriorityQueue<T>
{
    // I'm using an unsorted array for this example, but ideally this
    // would be a binary heap. There's an open issue for adding a binary
    // heap to the standard C# library: https://github.com/dotnet/corefx/issues/574
    //
    // Until then, find a binary heap class:
    // * https://bitbucket.org/BlueRaja/high-speed-priority-queue-for-c/wiki/Home
    // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
    // * http://xfleury.github.io/graphsearch.html
    // * http://stackoverflow.com/questions/102398/priority-queue-in-net

    private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

    //private HashSet<Tuple<T, double>> elements = new HashSet<Tuple<T, double>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, double priority)
    {
        elements.Add(new Tuple<T, double>(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    // TODO: Check that this works correctly
    public bool Contains(T item)
    {
        bool contains = false;
        for(int i = 0; i < elements.Count; i++)
        {
            if(elements[i].Item1.Equals(item))
            {
                contains = true;
            }
        }
        return contains;
    }

    public void Remove (T item)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item1.Equals(item))
            {
                elements.RemoveAt(i);
                return;
            }
        }
    }
}


/* NOTE about types: in the main article, in the Python code I just
 * use numbers for costs, heuristics, and priorities. In the C++ code
 * I use a typedef for this, because you might want int or double or
 * another type. In this C# code I use double for costs, heuristics,
 * and priorities. You can use an int if you know your values are
 * always integers, and you can use a smaller size number if you know
 * the values are always small. */

public class AStarSearch
{
    public Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    public Dictionary<Node, double> gScore = new Dictionary<Node, double>();

    static float straightMovementCost = 1.0f;
    static float diagonalMovementCost = 1.0f;

    // Note: a generic version of A* would abstract over Node and also Heuristic
    static public double ManhattanHeuristic(Node a, Node b)
    {
        return straightMovementCost * Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    static public double SimpleDiagonalHeuristic(Node a, Node b)
    {
        return straightMovementCost * Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
    }

    static public double ComplexDiagonalHeuristic(Node a, Node b)
    {
        double h_diagonal = Math.Min(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
        double h_straight = ManhattanHeuristic(a, b);
        return (diagonalMovementCost * h_diagonal) + (straightMovementCost * (h_straight - 2 * h_diagonal));
    }

    private Node SmallestNode(Dictionary<Node, double> set)
    {
        KeyValuePair<Node, double> smallest = set.ElementAt(0);
        foreach (KeyValuePair<Node, double> item in set)
        {
            if (item.Value < smallest.Value)
                smallest = item;
        }
        return smallest.Key;
    }

    public AStarSearch(WeightedGraph<Node> graph, Node start, Node goal)
    {
        Dictionary<Node, double> openSet = new Dictionary<Node, double>(); //open 
        Dictionary<Node, double> closedSet = new Dictionary<Node, double>(); //closed 

        openSet.Add(start,0);

        cameFrom[start] = start;
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            var current = SmallestNode(openSet);
            if (current.Equals(goal))
            {
                break;
            }

            openSet.Remove(current);
            closedSet.Add(current, 0);
            
            foreach (var next in graph.Neighbors(current))
            {
                double newCost = gScore[current] + graph.Cost(current, next);
    
                if (openSet.ContainsKey(next) && newCost < gScore[next])
                {
                    openSet.Remove(next);
                }
                else if (closedSet.ContainsKey(next) && newCost < gScore[next])
                {
                    closedSet.Remove(next);
                }
                else if (!openSet.ContainsKey(next) && !closedSet.ContainsKey(next))
                {
                    gScore[next] = newCost;
                    double fScore = newCost + ManhattanHeuristic(next, goal);
                    openSet.Add(next, fScore);
                    cameFrom[next] = current;
                }

            }
        }
    }
}