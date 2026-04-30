using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Algorithms
{
    BFS,
    DFS,
    Dijkstra,
    AStar
}

public class PathFinder : MonoBehaviour
{
    private TileGraphGenerator graphGenerator;

    private Vector3 startNode;
    private Vector3 endNode;

    public List<Vector3> path = new List<Vector3>();
    HashSet<Vector3> discovered = new HashSet<Vector3>();

    private Graph<Vector3> graph;

    public Algorithms algorithm = Algorithms.BFS;

    void Start()
    {
        graphGenerator = GetComponent<TileGraphGenerator>();
    }

    private Vector3 GetClosestNodeToPosition(Vector3 position)
    {
        graph = graphGenerator.GetGraph();
        Vector3 closestNode = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (var node in graph.ReturnNodesList())
        {
            if (Vector3.Distance(node, position) < closestDistance)
            {
                closestDistance = Vector3.Distance(node, position);
                closestNode = node;
            }
        }

        return closestNode;
    }

    public List<Vector3> CalculatePath(Vector3 from, Vector3 to)
    {
        Vector3 playerPosition = from;

        startNode = GetClosestNodeToPosition(playerPosition);
        endNode = GetClosestNodeToPosition(to);

        List<Vector3> shortestPath = new List<Vector3>();

        switch (algorithm)
        {
            case Algorithms.BFS:
                shortestPath = BFS(startNode, endNode);
                break;
            case Algorithms.DFS:
                shortestPath = DFS(startNode, endNode);
                break;
            case Algorithms.Dijkstra:
                shortestPath = Dijkstra(startNode, endNode);
                break;
            case Algorithms.AStar:
                shortestPath = AStar(startNode, endNode);
                break;
        }

        path = shortestPath; //Used for drawing the path

        return shortestPath;
    }

    List<Vector3> BFS(Vector3 start, Vector3 end)
    {
        //Use this "discovered" list to see the nodes in the visual debugging used on OnDrawGizmos()
        discovered.Clear();
        Vector3 current = start;
        Dictionary<Vector3, Vector3> parentlist = new();
        Queue<Vector3> queue = new();
        queue.Enqueue(current);
        discovered.Add(current);
        while (queue.Count > 0)
        {
            current = queue.Dequeue();
            if (current == end)
            {
                return ReconstructPath(parentlist, start, end);
            }
            foreach (Vector3 neighbor in graph.ReturnAdjacents(current))
            {
                if (discovered.Contains(neighbor)) continue;
                queue.Enqueue(neighbor);
                discovered.Add(neighbor);
                parentlist[neighbor] = current;
            }
        }

        return new List<Vector3>(); // No path found
    }

    List<Vector3> DFS(Vector3 start, Vector3 end)
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

    public List<Vector3> Dijkstra(Vector3 start, Vector3 end)
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
                    todo.Add((neighbor, newcost));
                    discovered.Add(neighbor);
                }
            }
        }
        /* */
        return new List<Vector3>(); // No path found
    }

    List<Vector3> AStar(Vector3 start, Vector3 end)
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

    public float Cost(Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }

    public float Heuristic(Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }

    List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> parentMap, Vector3 start, Vector3 end)
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startNode, .3f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endNode, .3f);

        if (discovered != null)
        {
            foreach (var node in discovered)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(node, .3f);
            }
        }

        if (path != null)
        {
            foreach (var node in path)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(node, .3f);
            }
        }


    }
}
