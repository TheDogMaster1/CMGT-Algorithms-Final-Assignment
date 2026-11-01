using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Collider2D))]
public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private List<Quest> Quests;

    [SerializeField]
    private GameObject QuestMenu;

    [SerializeField]
    private GameObject QuestUIPrefab;

    private void OnInteract(InputAction.CallbackContext context)
    {

    }
}
