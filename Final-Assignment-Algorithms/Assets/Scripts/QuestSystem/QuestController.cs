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
        EventBus<EnemyDieEvent>.OnEventPublished += UpdateEnemyProgress;
    }

    public void SetQuest(Quest quest)
    {
        if (currentQuest.questID == quest.questID) return;
        currentQuest = new QuestProgress(quest);
    }

    public void UpdateEnemyProgress(EnemyDieEvent enemyDieEvent)
    {
        if (currentQuest == null) return;
        foreach (var objective in currentQuest.objectives)
        {
            if (objective.type != EObjectiveType.killEnemies) continue;

        }
    }

    public QuestProgress GetCurrentQuest()
    {
        return currentQuest;
    }
}
