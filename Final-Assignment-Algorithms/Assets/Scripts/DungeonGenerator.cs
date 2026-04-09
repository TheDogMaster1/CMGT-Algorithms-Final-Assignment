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

    private IEnumerator Splitrooms()
    {

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
            if (splitType != SplitType.instant)
            {
                yield return SplitWait();
            }
            if (toDoRooms[0].width > toDoRooms[0].height)
            {
                int splitPoint = random.Next(minSplitPoint, toDoRooms[0].width - minSplitPoint);
                //Debug.Log(toDoRooms[0]);
                //Debug.Log("SplitPoint is " + splitPoint);
                RectInt newroom = toDoRooms[0];
                newroom.width = toDoRooms[0].width - splitPoint + overlapAmount;
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

                newroom.width = toDoRooms[0].width - (toDoRooms[0].width - splitPoint);
                newroom.x = toDoRooms[0].x + (toDoRooms[0].width - splitPoint);
                CurrentRoom = newroom;
                if (newroom.width <= maximumRoomSize.width && newroom.height <= maximumRoomSize.height)
                {
                    doneRooms.Add(newroom);
                }
                else
                {
                    toDoRooms.Add(newroom);
                }
                toDoRooms.Remove(toDoRooms[0]);
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }
            }
            else
            {
                int splitPoint = random.Next(minSplitPoint, toDoRooms[0].height - minSplitPoint);
                RectInt newroom = toDoRooms[0];
                newroom.height = toDoRooms[0].height - splitPoint + overlapAmount;
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

                newroom.height = toDoRooms[0].height - (toDoRooms[0].height - splitPoint);
                newroom.y = toDoRooms[0].y + (toDoRooms[0].height - splitPoint);
                CurrentRoom = newroom;
                if (newroom.height <= maximumRoomSize.height && newroom.width <= maximumRoomSize.width)
                {
                    doneRooms.Add(newroom);
                }
                else
                {
                    toDoRooms.Add(newroom);
                }
                toDoRooms.Remove(toDoRooms[0]);
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }
            }
        }
        CurrentRoom = RectInt.zero;
    }

    private void ResetEverything()
    {
        toDoRooms.Clear();
        doneRooms.Clear();
        doors.Clear();
        graphController.roomGraph.ClearGraph();
        random = new();
        currentDoor = new();
        CurrentRoom = new();
        StopAllCoroutines();
        StartCoroutine(Splitrooms());
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
    private void GenerateDungeonWithDoors()
    {
        ResetEverything();
        if (toDoRooms.Count == 0) StartCoroutine(AddDoors());
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
