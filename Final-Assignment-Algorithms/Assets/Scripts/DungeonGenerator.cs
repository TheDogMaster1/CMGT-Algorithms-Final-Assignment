using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<RectInt> ToDoRooms;
    public List<RectInt> DoneRooms;
    public RectInt maximumRoomSize;
    private RectInt CurrentRoom;

    private int roomindex;
    public float secondsToWait;

    public enum SplitType { withDelay, withSpacebar }

    public SplitType splitType = SplitType.withDelay;




    private void Update()
    {
        for (int i = 0; i < ToDoRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(ToDoRooms[i], Color.red);
        }

        for (int i = 0; i < DoneRooms.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(DoneRooms[i], Color.green);
        }
        AlgorithmsUtils.DebugRectInt(CurrentRoom, Color.cyan);
    }


    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private IEnumerator Splitrooms()
    {
        while (ToDoRooms.Count > 0)
        {
            if (ToDoRooms[roomindex].width > ToDoRooms[roomindex].height)
            {
                int splitPoint = Random.Range(5, ToDoRooms[roomindex].width - 5);
                Debug.Log("SplitPoint is " + splitPoint);
                RectInt newroom = ToDoRooms[roomindex];
                newroom.width = ToDoRooms[roomindex].width - splitPoint + 1;
                CurrentRoom = newroom;
                if (newroom.width <= maximumRoomSize.width && newroom.height <= maximumRoomSize.height)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                yield return SplitWait();

                newroom.width = ToDoRooms[roomindex].width - (ToDoRooms[roomindex].width - splitPoint);
                newroom.x = ToDoRooms[roomindex].x + (ToDoRooms[roomindex].width - splitPoint);
                CurrentRoom = newroom;
                if (newroom.width <= maximumRoomSize.width && newroom.height <= maximumRoomSize.height)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                yield return SplitWait();
                ToDoRooms.Remove(ToDoRooms[roomindex]);
            }
            else
            {
                int splitPoint = Random.Range(5, ToDoRooms[roomindex].height - 5);
                RectInt newroom = ToDoRooms[roomindex];
                newroom.height = ToDoRooms[roomindex].height - splitPoint + 1;
                CurrentRoom = newroom;
                if (newroom.height <= maximumRoomSize.height && newroom.width <= maximumRoomSize.width)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                yield return SplitWait();

                newroom.height = ToDoRooms[roomindex].height - (ToDoRooms[roomindex].height - splitPoint);
                newroom.y = ToDoRooms[roomindex].y + (ToDoRooms[roomindex].height - splitPoint);
                CurrentRoom = newroom;
                if (newroom.height <= maximumRoomSize.height && newroom.width <= maximumRoomSize.width)
                {
                    DoneRooms.Add(newroom);
                }
                else
                {
                    ToDoRooms.Add(newroom);
                }
                yield return SplitWait();
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
