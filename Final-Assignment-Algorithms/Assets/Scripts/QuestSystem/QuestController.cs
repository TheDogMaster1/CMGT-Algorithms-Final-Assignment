using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController instance { get; private set; }
    [SerializeField]
    private QuestProgress currentQuest;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SetQuest(Quest quest)
    {
        if (currentQuest.questID == quest.questID) return;
        currentQuest = new QuestProgress(quest);
    }

    public QuestProgress GetCurrentQuest()
    {
        return currentQuest;
    }
}
