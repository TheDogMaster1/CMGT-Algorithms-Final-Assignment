using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPathFinderAlgorithm : MonoBehaviour
{

    protected HashSet<Vector3> discovered = new HashSet<Vector3>();
    public abstract List<Vector3> FindPath(Vector3 start, Vector3 end, Graph<Vector3> graph);

    protected List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> parentMap, Vector3 start, Vector3 end)
    {
        List<Vector3> path = new List<Vector3>();
        Vector3 currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = parentMap[currentNode];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    protected float Cost(Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }

    protected float Heuristic(Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }
}
