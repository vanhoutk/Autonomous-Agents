﻿using System;
using System.Collections.Generic;
using System.Linq;

public struct Coordinates
{
    public int x, y;

    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public struct AttenuationData
{
    public double movementCost;
    public double sightAttenuation;
    public double hearingAttenuation;
    public double smellAttenuation;

    public AttenuationData(double moveCost, double sightA, double hearingA, double smellA)
    {
        movementCost = moveCost;
        sightAttenuation = sightA;
        hearingAttenuation = hearingA;
        smellAttenuation = smellA;
    }
}

public struct Node
{
    public readonly Coordinates coordinates;
    public AttenuationData attenuationData;

    public Node(int x, int y)
    {
        coordinates = new Coordinates(x, y);
        attenuationData = new AttenuationData(0, 0, 0, 0);
    }

    public Node(int x, int y, double moveCost, double sightA, double hearingA, double smellA)
    {
        coordinates = new Coordinates(x, y);
        attenuationData = new AttenuationData(moveCost, sightA, hearingA, smellA);
    }

    public Node(Coordinates coordinates, AttenuationData attenuationData)
    {
        this.coordinates = coordinates;
        this.attenuationData = attenuationData;
    }
}

public interface WeightedGraph<T>
{
    double MovementCost(T a, T b);
    double SenseCost(T a, T b, SenseTypes type);
    IEnumerable<T> Neighbours(T id);
}

public class SquareGrid : WeightedGraph<Node>
{
    // Variables
    private static readonly Coordinates[] DIRECTIONS = new[]
        {
            new Coordinates(1, 0),
            new Coordinates(0, -1),
            new Coordinates(-1, 0),
            new Coordinates(0, 1)
        };

    public int width, height;
    public Dictionary<Coordinates, Node> nodeSet = new Dictionary<Coordinates, Node>();

    // Functions
    /*
     * public bool InBounds(Coordinates coords)
     * public double MovementCost(Node a, Node b)
     * public double SenseCost(Node a, Node b, SenseTypes senseType)
     * public IEnumerable<Node> Neighbours(Node node)
     * public SquareGrid(int width, int height)
     */

    public bool InBounds(Coordinates coords)
    {
        return 0 <= coords.x && coords.x < width && 0 <= coords.y && coords.y < height;
    }

    public double MovementCost(Node a, Node b)
    {
        return b.attenuationData.movementCost;
    }

    public double SenseCost(Node a, Node b, SenseTypes senseType)
    {
        if (senseType == SenseTypes.Hearing)
            return b.attenuationData.hearingAttenuation;
        if (senseType == SenseTypes.Sight)
            return b.attenuationData.sightAttenuation;
        if (senseType == SenseTypes.Smell)
            return b.attenuationData.smellAttenuation;
        return 0;
    }

    public IEnumerable<Node> Neighbours(Node node)
    {
        foreach (var dir in DIRECTIONS)
        {
            Coordinates next = new Coordinates(node.coordinates.x + dir.x, node.coordinates.y + dir.y);
            if (InBounds(next))
            {
                if (nodeSet.ContainsKey(next))
                    yield return nodeSet[next];
            }
        }
    }

    public SquareGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
}

public class AStarSearch
{
    // Variables
    private static float straightMovementCost = 1.0f;
    private static float diagonalMovementCost = 1.0f;

    public Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    public Dictionary<Node, double> gScore = new Dictionary<Node, double>();

    // Functions
    /*
     * public double ComplexDiagonalHeuristic(Node a, Node b)
     * public double ManhattanHeuristic(Node a, Node b)
     * public double SimpleDiagonalHeuristic(Node a, Node b)
     * private Node SmallestNode(Dictionary<Node, double> set)
     * 
     * public AStarSearch(WeightedGraph<Node> graph, Node start, Node goal)
     * public AStarSearch(WeightedGraph<Node> graph, Node start, Node goal, SenseTypes senseType)
     */

    public double ComplexDiagonalHeuristic(Node a, Node b)
    {
        double h_diagonal = Math.Min(Math.Abs(a.coordinates.x - b.coordinates.x), Math.Abs(a.coordinates.y - b.coordinates.y));
        double h_straight = ManhattanHeuristic(a, b);
        return (diagonalMovementCost * h_diagonal) + (straightMovementCost * (h_straight - 2 * h_diagonal));
    }

    public double ManhattanHeuristic(Node a, Node b)
    {
        return straightMovementCost * Math.Abs(a.coordinates.x - b.coordinates.x) + Math.Abs(a.coordinates.y - b.coordinates.y);
    }

    public double SimpleDiagonalHeuristic(Node a, Node b)
    {
        return straightMovementCost * Math.Max(Math.Abs(a.coordinates.x - b.coordinates.x), Math.Abs(a.coordinates.y - b.coordinates.y));
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

        open_set.Add(start, 0);

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

            foreach (var next in graph.Neighbours(current))
            {
                double new_cost = gScore[current] + graph.MovementCost(current, next);

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

    public AStarSearch(WeightedGraph<Node> graph, Node start, Node goal, SenseTypes senseType)
    {
        Dictionary<Node, double> open_set = new Dictionary<Node, double>();
        Dictionary<Node, double> closed_set = new Dictionary<Node, double>();

        open_set.Add(start, 0);

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

            foreach (var next in graph.Neighbours(current))
            {
                double new_cost = gScore[current] + graph.SenseCost(current, next, senseType);

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
                    
                    // TODO: Possibly change heuristic based on the sense in question
                    double f_score = new_cost + ComplexDiagonalHeuristic(next, goal);
                    open_set.Add(next, f_score);
                    cameFrom[next] = current;
                }
            }
        }
    }
}