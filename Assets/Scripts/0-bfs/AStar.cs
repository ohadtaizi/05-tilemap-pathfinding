using System;
using System.Collections.Generic;

/**
 * A generic implementation of the A* algorithm.
 * @author Erel Segal-Halevi
 * @since 2020-02
 */
public class AStar
{
    public static void FindPath<NodeType>(
            IGraph<NodeType> graph,
            NodeType startNode, NodeType endNode,
            List<NodeType> outputPath,
            Func<NodeType, NodeType, double> heuristic,
            int maxIterations = 1000)
    {
        PriorityQueue<NodeType> openQueue = new PriorityQueue<NodeType>();
        HashSet<NodeType> closedSet = new HashSet<NodeType>();
        Dictionary<NodeType, double> gScore = new Dictionary<NodeType, double>();
        Dictionary<NodeType, NodeType> cameFrom = new Dictionary<NodeType, NodeType>();

        gScore[startNode] = 0;
        double fScore = gScore[startNode] + heuristic(startNode, endNode);
        openQueue.Enqueue(startNode, fScore);

        int iterations = 0;
        while (openQueue.Count > 0 && iterations < maxIterations)
        {
            iterations++;
            NodeType current = openQueue.Dequeue();

            if (current.Equals(endNode))
            {
                ReconstructPath(cameFrom, outputPath, endNode);
                return;
            }

            closedSet.Add(current);

            foreach (var neighbor in graph.Neighbors(current))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                double tentativeGScore = gScore[current] + graph.Cost(current, neighbor);
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    double fScoreNeighbor = gScore[neighbor] + heuristic(neighbor, endNode);
                    if (!openQueue.Contains(neighbor))
                        openQueue.Enqueue(neighbor, fScoreNeighbor);
                }
            }
        }
    }

    private static void ReconstructPath<NodeType>(Dictionary<NodeType, NodeType> cameFrom, List<NodeType> outputPath, NodeType current)
    {
        outputPath.Add(current);
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            outputPath.Insert(0, current); // Insert at the beginning for correct order
        }
    }

    public static List<NodeType> GetPath<NodeType>(
        IGraph<NodeType> graph,
        NodeType startNode, NodeType endNode,
        Func<NodeType, NodeType, double> heuristic,
        int maxIterations = 1000)
    {
        List<NodeType> path = new List<NodeType>();
        FindPath(graph, startNode, endNode, path, heuristic, maxIterations);
        return path;
    }
    public interface IGraph<NodeType>
    {
        IEnumerable<NodeType> Neighbors(NodeType node);
        double Cost(NodeType fromNode, NodeType toNode); // Add this method
    }

}