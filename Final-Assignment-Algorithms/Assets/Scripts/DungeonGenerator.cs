using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator : MonoBehaviour
{
    public List<RectInt> toDoRooms;
    public List<RectInt> doneRooms;
    public List<RectInt> doors;
    public RectInt BaseRoom;
    public RectInt maximumRoomSize;
    private RectInt CurrentRoom;
    private RectInt currentDoor;

    public int minSplitPoint;
    public int overlapAmount = 1;

    private int roomindex;
    public float secondsToWait;

    [Header("Seed options")]
    public bool seeded = false;
    public int seed = 0;

    private DungeonGraphController graphController;
    public enum SplitType { instant, withDelay, withSpacebar }

    [Space(20)]
    public SplitType splitType = SplitType.withDelay;

    private Random random = new Random();
    private void Start()
    {
        graphController = GetComponent<DungeonGraphController>();
    }
    private void Update()
    {
        for (int i = 0; i < toDoRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(toDoRooms[i], Color.red);
        }

        for (int i = 0; i < doneRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doneRooms[i], Color.green, 0);
            AlgorithmsUtils.DebugRectInt(doneRooms[i], new Color(0.8f, 0, 1, 1), 0, false, 3);
        }

        for (int i = 0; i < doors.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doors[i], Color.blue, 0, false, 3);
        }
        if (CurrentRoom.width > 0) AlgorithmsUtils.DebugRectInt(CurrentRoom, Color.cyan, 0);
        if (currentDoor.width > 0) AlgorithmsUtils.DebugRectInt(currentDoor, Color.cyan, 0, false, 3);
    }


    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator Splitrooms()
    {
        toDoRooms.Clear();
        doneRooms.Clear();
        doors.Clear();
        graphController.roomGraph.ClearGraph();

        if (seeded == true)
        {
            random = new Random(seed);
        }
        else
        {
            random = new();
        }

        toDoRooms.Add(BaseRoom);
        while (toDoRooms.Count > 0)
        {
            if (toDoRooms[roomindex].width > toDoRooms[roomindex].height)
            {
                int splitPoint = random.Next(minSplitPoint, toDoRooms[roomindex].width - minSplitPoint);
                //Debug.Log("SplitPoint is " + splitPoint);
                RectInt newroom = toDoRooms[roomindex];
                newroom.width = toDoRooms[roomindex].width - splitPoint + overlapAmount;
                CurrentRoom = newroom;
                if (newroom.width <= maximumRoomSize.width && newroom.height <= maximumRoomSize.height)
                {
                    doneRooms.Add(newroom);
                }
                else
                {
                    toDoRooms.Add(newroom);
                }
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }

                newroom.width = toDoRooms[roomindex].width - (toDoRooms[roomindex].width - splitPoint);
                newroom.x = toDoRooms[roomindex].x + (toDoRooms[roomindex].width - splitPoint);
                CurrentRoom = newroom;
                if (newroom.width <= maximumRoomSize.width && newroom.height <= maximumRoomSize.height)
                {
                    doneRooms.Add(newroom);
                }
                else
                {
                    toDoRooms.Add(newroom);
                }
                toDoRooms.Remove(toDoRooms[roomindex]);
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }
            }
            else
            {
                int splitPoint = random.Next(minSplitPoint, toDoRooms[roomindex].height - minSplitPoint);
                RectInt newroom = toDoRooms[roomindex];
                newroom.height = toDoRooms[roomindex].height - splitPoint + overlapAmount;
                CurrentRoom = newroom;
                if (newroom.height <= maximumRoomSize.height && newroom.width <= maximumRoomSize.width)
                {
                    doneRooms.Add(newroom);
                }
                else
                {
                    toDoRooms.Add(newroom);
                }
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }

                newroom.height = toDoRooms[roomindex].height - (toDoRooms[roomindex].height - splitPoint);
                newroom.y = toDoRooms[roomindex].y + (toDoRooms[roomindex].height - splitPoint);
                CurrentRoom = newroom;
                if (newroom.height <= maximumRoomSize.height && newroom.width <= maximumRoomSize.width)
                {
                    doneRooms.Add(newroom);
                }
                else
                {
                    toDoRooms.Add(newroom);
                }
                toDoRooms.Remove(toDoRooms[roomindex]);
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }
            }
        }
        CurrentRoom = RectInt.zero;
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator AddDoors()
    {
        for (int i = 0; i < doneRooms.Count; i++)
        {
            for (int j = i + 1; j < doneRooms.Count; j++)
            {
                if (AlgorithmsUtils.Intersects(doneRooms[i], doneRooms[j]))
                {
                    RectInt door = AlgorithmsUtils.Intersect(doneRooms[i], doneRooms[j]);
                    if (door.height > 10 || door.width > 10)
                    {
                        if (door.width > door.height)
                        {
                            door.x += door.width / 2 - 1;
                            door.width = 3;
                        }
                        else
                        {
                            door.y += door.height / 2 - 1;
                            door.height = 3;
                        }
                        currentDoor = door;
                        doors.Add(door);
                    }
                    if (splitType != SplitType.instant)
                    {
                        yield return SplitWait();
                    }
                }
            }
        }
        currentDoor = RectInt.zero;
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator GenerateDungeonWithDoors()
    {
        StartCoroutine(Splitrooms());
        yield return new WaitUntil(() => toDoRooms.Count == 0);
        StartCoroutine(AddDoors());
        yield return null;
    }

    public IEnumerator SplitWait()
    {
        switch (splitType)
        {
            case SplitType.withDelay:
                yield return new WaitForSeconds(secondsToWait);
                break;
            case SplitType.withSpacebar:
                yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
                yield return null;
                break;
        }
    }
}
