using System.Collections.Generic;
using UnityEngine;

public class DFSPathFindingAlgorithm : AbstractPathFinderAlgorithm
{
    public override List<Vector3> FindPath(Vector3 start, Vector3 end, Graph<Vector3> graph)
    {
        //Use this "discovered" list to see the nodes in the visual debugging used on OnDrawGizmos()
        discovered.Clear();
        Vector3 current = start;
        Dictionary<Vector3, Vector3> parentlist = new();
        Stack<Vector3> stack = new();
        stack.Push(current);
        discovered.Add(current);
        while (stack.Count > 0)
        {
            current = stack.Pop();
            if (current == end)
            {
                return ReconstructPath(parentlist, start, end);
            }
            foreach (Vector3 neighbor in graph.ReturnAdjacents(current))
            {
                if (discovered.Contains(neighbor)) continue;
                stack.Push(neighbor);
                discovered.Add(neighbor);
                parentlist[neighbor] = current;
            }
        }

        return new List<Vector3>(); // No path found
    }
}
