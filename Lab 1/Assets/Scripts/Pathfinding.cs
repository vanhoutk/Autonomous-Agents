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
        Debug.Log("Pathfinding: Inside Neighbors");
        foreach (var dir in DIRECTIONS)
        {
            Debug.Log("Pathfinding: Inside Neighbors foreach");
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
        this.Item1 = item1;
        this.Item2 = item2;
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

    public AStarSearch(WeightedGraph<Node> graph, Node start, Node goal)
    {
        var openSet = new PriorityQueue<Node>();
        var closedSet = new PriorityQueue<Node>();
        openSet.Enqueue(start, 0);

        cameFrom[start] = start;
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current.Equals(goal))
            {
                break;
            }

            //closedSet.Enqueue(current, 0);

            Debug.Log("Pathfinding: Before foreach");
            Debug.Log("Pathfinding: Start - " + start.x + ", " + start.y);
            Debug.Log("Pathfinding: Goal - " + goal.x + ", " + goal.y);
            Debug.Log("Pathfinding: Current - " + current.x + ", " + current.y);
            double testCost = graph.Cost(current, new Node(current.x, current.y + 1));
            Debug.Log("Pathfinding: testCost = " + testCost);

            foreach (var next in graph.Neighbors(current))
            {
                Debug.Log("Pathfinding: Inside foreach");

                double newCost = gScore[current] + graph.Cost(current, next);

                
                /*if(openSet.Contains(next) && newCost < gScore[next])
                {
                    openSet.Remove(next);
                }
                else if(closedSet.Contains(next) && newCost < gScore[next])
                {
                    closedSet.Remove(next);
                }
                else
                {
                    gScore[next] = newCost;
                    double fScore = newCost + ManhattanHeuristic(next, goal);
                    openSet.Enqueue(next, fScore);
                    cameFrom[next] = current;
                }*/
                


                if (!gScore.ContainsKey(next) || newCost < gScore[next])
                {
                    gScore[next] = newCost;
                    double fScore = newCost + ManhattanHeuristic(next, goal);
                    openSet.Enqueue(next, fScore);
                    cameFrom[next] = current;
                }
            }
        }
    }
}




//public class Pathfinding
//{
//    List<Vector2> A_Star(Vector2 start, Vector2 goal, Vector2 mapSize)
//    {
//        // The set of nodes already evaluated.
//        List<Vector2> closedSet = new List<Vector2>(); // = { };
//        // The set of currently discovered nodes that are not evaluated yet.
//        // Initially, only the start node is known.
//        List<Vector2> openSet = new List<Vector2>(); // := {start}
//        openSet.Add(start);

//        // For each node, which node it can most efficiently be reached from.
//        // If a node can be reached from many nodes, cameFrom will eventually contain the
//        // most efficient previous step.
//        int[,] cameFrom;
//        //cameFrom := the empty map

//        // For each node, the cost of getting from the start node to that node.
//        float[,] gScore = new float[(int)mapSize.x, (int)mapSize.y];
//        for(int i = 0; i < mapSize.y; i++)
//        {
//            for(int j = 0; j < mapSize.x; j++)
//            {
//                gScore[j, i] = 10000.0f; //Infinity
//            }
//        }
//        //gScore := map with default value of Infinity

//        // The cost of going from start to start is zero.
//        gScore[(int)start.x, (int)start.y] = 0.0f;


//        // For each node, the total cost of getting from the start node to the goal
//        // by passing by that node. That value is partly known, partly heuristic.
//        float[,] fScore = new float[(int)mapSize.x, (int)mapSize.y];
//        for (int i = 0; i < mapSize.y; i++)
//        {
//            for (int j = 0; j < mapSize.x; j++)
//            {
//                fScore[j, i] = 10000.0f; //Infinity
//            }
//        }
//        //fScore := map with default value of Infinity

//        // For the first node, that value is completely heuristic.
//        fScore[(int)start.x, (int)start.y] = heuristic_cost_estimate(start, goal);

//        while (openSet.Count() != 0) // is not empty
//        {
//            current:= the node in openSet having the lowest fScore[] value
//            if current = goal
//                return reconstruct_path(cameFrom, current)

//            openSet.Remove(current)
//            closedSet.Add(current)
//            for each neighbor of current
//                if neighbor in closedSet
//                    continue		// Ignore the neighbor which is already evaluated.
//                // The distance from start to a neighbor
//                tentative_gScore:= gScore[current] + dist_between(current, neighbor)
//                if neighbor not in openSet	// Discover a new node
//                    openSet.Add(neighbor)
//                else if tentative_gScore >= gScore[neighbor]
//                    continue		// This is not a better path.

//                // This path is the best until now. Record it!
//                cameFrom[neighbor] := current
//                gScore[neighbor] := tentative_gScore
//                fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal)
//        }

//        return failure
//    }

//    float heuristic_cost_estimate(Vector2 a, Vector2 b)
//    {
//        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
//    }

//    List<Vector2> reconstruct_path(List<Vector2> cameFrom, Vector2 current)
//    {
//        List<Vector2> total_path = new List<Vector2>();
//        total_path.Add(current);
//        while (cameFrom.Contains(current))
//        {
//            current = cameFrom[current];
//            total_path.Add(current);
//        }
//        return total_path;
//    }
//}
