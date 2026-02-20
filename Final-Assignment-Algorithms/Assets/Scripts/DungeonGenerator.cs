using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<RectInt> ToDoRooms;
    public List<RectInt> DoneRooms;
    public RectInt BaseRoom;
    public RectInt maximumRoomSize;
    private RectInt CurrentRoom;

    public int minSplitPoint;

    private int roomindex;
    public float secondsToWait;

    public enum SplitType { instant, withDelay, withSpacebar }

    public SplitType splitType = SplitType.withDelay;




    private void Update()
    {
        for (int i = 0; i < ToDoRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(ToDoRooms[i], Color.red);
        }

        for (int i = 0; i < DoneRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(DoneRooms[i], Color.green, 0);
            //AlgorithmsUtils.DebugRectInt(DoneRooms[i], Color.blue, 0, false, 10);
        }
        AlgorithmsUtils.DebugRectInt(CurrentRoom, Color.cyan, 0);
    }


    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator Splitrooms()
    {
        ToDoRooms.Clear();
        DoneRooms.Clear();

        ToDoRooms.Add(BaseRoom);
        while (ToDoRooms.Count > 0)
        {
            if (ToDoRooms[roomindex].width > ToDoRooms[roomindex].height)
            {
                int splitPoint = Random.Range(minSplitPoint, ToDoRooms[roomindex].width - minSplitPoint);
                Debug.Log("SplitPoint is " + splitPoint);
                RectInt newroom = ToDoRooms[roomindex];
                newroom.width = ToDoRooms[roomindex].width - splitPoint + 1;
                CurrentRoom = newroom;
                if (newroom.width <= maximumRoomSize.width)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }

                newroom.width = ToDoRooms[roomindex].width - (ToDoRooms[roomindex].width - splitPoint);
                newroom.x = ToDoRooms[roomindex].x + (ToDoRooms[roomindex].width - splitPoint);
                CurrentRoom = newroom;
                if (newroom.width <= maximumRoomSize.width)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }
                ToDoRooms.Remove(ToDoRooms[roomindex]);
            }
            else
            {
                int splitPoint = Random.Range(minSplitPoint, ToDoRooms[roomindex].height - minSplitPoint);
                RectInt newroom = ToDoRooms[roomindex];
                newroom.height = ToDoRooms[roomindex].height - splitPoint + 1;
                CurrentRoom = newroom;
                if (newroom.height <= maximumRoomSize.height)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }

                newroom.height = ToDoRooms[roomindex].height - (ToDoRooms[roomindex].height - splitPoint);
                newroom.y = ToDoRooms[roomindex].y + (ToDoRooms[roomindex].height - splitPoint);
                CurrentRoom = newroom;
                if (newroom.height <= maximumRoomSize.height)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                if (splitType != SplitType.instant)
                {
                    yield return SplitWait();
                }

                ToDoRooms.Remove(ToDoRooms[roomindex]);
            }
        }
    }

    private IEnumerator SplitWait()
    {
        switch (splitType)
        {
            case SplitType.withDelay:
                yield return new WaitForSeconds(secondsToWait);
                break;
            case SplitType.withSpacebar:
                yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
                break;
        }
    }
}
