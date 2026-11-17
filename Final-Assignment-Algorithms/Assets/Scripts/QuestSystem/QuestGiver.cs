using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Collider2D))]
public class QuestGiver : MonoBehaviour
{
    [SerializeField]
    private List<Quest> quests;

    [SerializeField]
    private GameObject questMenu;
    [SerializeField]
    private Transform questsParent;

    [SerializeField]
    private GameObject questUIPrefab;

    private bool insideRadius = false;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (questMenu.activeSelf || !insideRadius || !context.started) return;
        Debug.Log("Did stuff");
        questMenu.SetActive(true);
        foreach (var quest in quests)
        {
            QuestObject questObject = Instantiate(questUIPrefab, questsParent).GetComponent<QuestObject>();
            questObject.Init(quest);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        insideRadius = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        insideRadius = false;
    }
}
