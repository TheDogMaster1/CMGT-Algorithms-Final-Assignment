using System.Collections.Generic;

public class DungeonGraph<T>
{
    private Dictionary<T, List<T>> nodeList;

    public DungeonGraph()
    {
        nodeList = new();
    }

    public void ClearGraph()
    {
        nodeList.Clear();
    }

    public void AddNode(T node)
    {
        if (!nodeList.ContainsKey(node))
        {
            nodeList[node] = new List<T>();
        }
    }

    public void RemoveNode(T node)
    {
        foreach (T child in ReturnRoomAdjacents(node))
        {
            RemoveEdge(node, child);
        }
        nodeList.Remove(node);
    }

    public void AddEdge(T fromNode, T toNode)
    {
        if (!nodeList.ContainsKey(fromNode))
        {
            AddNode(fromNode);
        }
        if (!nodeList.ContainsKey(toNode))
        {
            AddNode(toNode);
        }

        nodeList[fromNode].Add(toNode);
        nodeList[toNode].Add(fromNode);
    }

    public void RemoveEdge(T fromNode, T toNode)
    {
        nodeList[fromNode].Remove(toNode);
        nodeList[toNode].Remove(fromNode);
    }

    public List<T> ReturnRooms()
    {
        return new List<T>(nodeList.Keys);
    }

    public List<T> ReturnRoomAdjacents(T node)
    {
        return new List<T>(nodeList[node]);
    }

    public int ReturnGraphLength()
    {
        return nodeList.Count;
    }
}
