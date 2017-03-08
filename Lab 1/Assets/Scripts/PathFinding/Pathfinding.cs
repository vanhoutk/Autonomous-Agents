using System;
using System.Collections.Generic;
using System.Linq;

public interface WeightedGraph<T>
{
    double Cost(Node a, Node b);
    IEnumerable<Node> Neighbors(Node id);
}

public struct Node
{
    public readonly int x, y;
 
    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class SquareGrid : WeightedGraph<Node>
{
    private static readonly Node[] DIRECTIONS = new[]
        {
            new Node(1, 0),
            new Node(0, -1),
            new Node(-1, 0),
            new Node(0, 1)
        };

    public int width, height;

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

public class AStarSearch
{
    public Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    public Dictionary<Node, double> gScore = new Dictionary<Node, double>();

    static float straightMovementCost = 1.0f;
    static float diagonalMovementCost = 1.0f;

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
        Dictionary<Node, double> open_set = new Dictionary<Node, double>(); 
        Dictionary<Node, double> closed_set = new Dictionary<Node, double>(); 

        open_set.Add(start,0);

        cameFrom[start] = start;
        gScore[start] = 0;

        while (open_set.Count > 0)
        {
            var current = SmallestNode(open_set);
            if (current.Equals(goal))
            {
                break;
            }

            open_set.Remove(current);
            closed_set.Add(current, 0);
            
            foreach (var next in graph.Neighbors(current))
            {
                double new_cost = gScore[current] + graph.Cost(current, next);
    
                if (open_set.ContainsKey(next) && new_cost < gScore[next])
                {
                    open_set.Remove(next);
                }
                else if (closed_set.ContainsKey(next) && new_cost < gScore[next])
                {
                    closed_set.Remove(next);
                }
                else if (!open_set.ContainsKey(next) && !closed_set.ContainsKey(next))
                {
                    gScore[next] = new_cost;
                    double f_score = new_cost + ManhattanHeuristic(next, goal);
                    open_set.Add(next, f_score);
                    cameFrom[next] = current;
                }

            }
        }
    }
}