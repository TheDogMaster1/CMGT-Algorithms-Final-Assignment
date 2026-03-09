using NaughtyAttributes;
using System;
using UnityEngine;

public class DungeonGraphController : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;

    public DungeonGraph<RectInt> roomGraph;
    public DungeonGraph<RectInt> doorGraph;


    [InfoBox("Graph", EInfoBoxType.Normal)]
    [SerializeField]
    [TextArea(10, 10)]
    private string contents = "";

    private void Awake()
    {
        // Receive debug.log callbacks
        Application.logMessageReceived += logHandler;
    }

    private void logHandler(string info, string stackTrace, LogType type)
    {
        contents += info + Environment.NewLine;
    }

    private void OnApplicationQuit()
    {
        Application.logMessageReceived -= logHandler;
    }

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
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void GenerateGraph()
    {
        contents = "";
        roomGraph = new();
        foreach (RectInt room in dungeonGenerator.doneRooms)
        {
            roomGraph.AddNode(room);
        }
        foreach (RectInt door in dungeonGenerator.doors)
        {
            doorGraph.AddNode(door);
        }
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

        //foreach (RectInt roomNode in roomGraph.ReturnRooms())
        //{
        //    foreach (RectInt doorNode in doorGraph.ReturnRooms())
        //    {
        //        if (!AlgorithmsUtils.Intersects(roomNode, doorNode)) continue;

        //    }
        //}
    }
}
