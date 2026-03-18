using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGraphController : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;

    public DungeonGraph<RectInt> roomGraph;
    public DungeonGraph<RectInt> doorGraph;

    private RectInt checkedRoom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        roomGraph = new();
        doorGraph = new();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (RectInt room in roomGraph.ReturnRooms())
        {
            DebugExtension.DebugWireSphere(new Vector3(room.x + room.width / 2f, 0, room.y + room.height / 2f), Color.cyan);
            foreach (RectInt door in roomGraph.ReturnRoomAdjacents(room))
            {
                Debug.DrawLine(new Vector3(room.x + room.width / 2f, 0, room.y + room.height / 2f), new Vector3(door.x + door.width / 2f, 0, door.y + door.height / 2f), Color.yellow);
            }
        }
        foreach (RectInt door in doorGraph.ReturnRooms())
        {
            DebugExtension.DebugWireSphere(new Vector3(door.x + door.width / 2f, 0, door.y + door.height / 2f), Color.cyan);
        }
        if (checkedRoom.width > 0) AlgorithmsUtils.DebugRectInt(checkedRoom, Color.white, 0, false, 3);
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void GenerateGraph()
    {
        roomGraph = new();
        foreach (RectInt room in dungeonGenerator.doneRooms)
        {
            roomGraph.AddNode(room);
        }
        //foreach (RectInt door in dungeonGenerator.doors)
        //{
        //    doorGraph.AddNode(door);
        //}
        for (int i = 0; i < dungeonGenerator.doneRooms.Count; i++)
        {
            for (int j = 0; j < dungeonGenerator.doors.Count; j++)
            {
                if (AlgorithmsUtils.Intersects(dungeonGenerator.doneRooms[i], dungeonGenerator.doors[j]))
                {
                    roomGraph.AddEdge(dungeonGenerator.doneRooms[i], dungeonGenerator.doors[j]);
                }
            }
        }
    }
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator CheckGraph()
    {
        RectInt firstroom = roomGraph.ReturnRooms()[0];
        Queue<RectInt> queue = new();
        queue.Enqueue(firstroom);

        HashSet<RectInt> visited = new HashSet<RectInt>();
        visited.Add(firstroom);
        while (queue.Count > 0)
        {
            RectInt currentRoom = queue.Dequeue();
            checkedRoom = currentRoom;
            Debug.Log(currentRoom);
            if (dungeonGenerator.splitType != DungeonGenerator.SplitType.instant)
            {
                yield return dungeonGenerator.SplitWait();
            }
            foreach (RectInt neighbor in roomGraph.ReturnRoomAdjacents(currentRoom))
            {
                if (visited.Contains(neighbor)) continue;
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
        Debug.Log("There are " + roomGraph.ReturnRooms().Count + "Nodes");
        Debug.Log(visited.Count + "Has been checked");
        checkedRoom = new();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void RemoveRoom()
    {
        List<RectInt> test = dungeonGenerator.doneRooms;


        //List<RectInt> savedNodes = new List<RectInt>();
        RectInt testroom = test[0];

        for (int i = 0; i < test.Count; i++)
        {
            if (test[i].width * test[i].height < testroom.width * testroom.height)
            {
                testroom = test[i];
            }
            else
            {
                continue;
            }
        }
        //savedNodes.Add(roomGraph.ReturnRooms()[roomToDeleteNum]);
        //foreach(RectInt door in roomGraph.ReturnRoomAdjacents())
        //{

        //}
        //Debug.Log(test.Min<RectInt>());
        foreach (RectInt room in test)
        {
            Debug.Log(room.height * room.width);
        }

        dungeonGenerator.doneRooms.Remove(testroom);
        foreach (RectInt door in roomGraph.ReturnRoomAdjacents(testroom))
        {
            dungeonGenerator.doors.Remove(door);
            roomGraph.RemoveNode(door);
        }
        roomGraph.RemoveNode(testroom);

        Debug.Log("room: " + testroom + "Size: " + testroom.width * testroom.height);
        //Debug.Log("chose " + roomToDelete);
    }
}
