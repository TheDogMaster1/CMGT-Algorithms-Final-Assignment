using System.Collections.Generic;

public class BFS<T>
{
    public bool GraphSearch(DungeonGraph<T> graph, HashSet<T> visited)
    {
        T firstroom = graph.ReturnRooms()[0];
        if (visited.Contains(firstroom))
        {
            firstroom = graph.ReturnRooms()[1];
        }
        Queue<T> queue = new();
        queue.Enqueue(firstroom);

        visited.Add(firstroom);
        while (queue.Count > 0)
        {
            T currentRoom = queue.Dequeue();
            //Debug.Log(currentRoom);
            foreach (T neighbor in graph.ReturnRoomAdjacents(currentRoom))
            {
                if (visited.Contains(neighbor)) continue;
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
        bool everythingIsVisitable = graph.ReturnGraphLength() == visited.Count;
        return everythingIsVisitable;
    }
}
