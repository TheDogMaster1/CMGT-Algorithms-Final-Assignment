using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathFindingalgorithm : AbstractPathFinderAlgorithm
{
    public override List<Vector3> FindPath(Vector3 start, Vector3 end, Graph<Vector3> graph)
    {
        //Use this "discovered" list to see the nodes in the visual debugging used on OnDrawGizmos()
        discovered.Clear();
        Dictionary<Vector3, Vector3> path = new();
        Dictionary<Vector3, float> cost = new();
        List<(Vector3 node, float priority)> todo = new();
        Vector3 current = start;
        cost[current] = 0;
        todo.Add((current, 0));

        discovered.Add(current);

        while (todo.Count > 0)
        {
            todo = todo.OrderByDescending(node => node.priority).ToList();
            current = todo[todo.Count - 1].node;
            todo.RemoveAt(todo.Count - 1);
            if (current == end)
            {
                return ReconstructPath(path, start, end);
            }
            foreach (Vector3 neighbor in graph.ReturnAdjacents(current))
            {
                float newcost = cost[current] + Cost(current, neighbor);
                if (!cost.ContainsKey(neighbor) || newcost < cost[neighbor])
                {
                    cost[neighbor] = newcost;
                    path[neighbor] = current;
                    todo.Add((neighbor, newcost + Heuristic(neighbor, end)));
                    discovered.Add(neighbor);
                }
            }
        }
        /* */
        return new List<Vector3>(); // No path found
    }
}
