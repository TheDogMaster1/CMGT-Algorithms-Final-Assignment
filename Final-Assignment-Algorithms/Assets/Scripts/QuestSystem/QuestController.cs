using System;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    [SerializeField]
    private QuestProgress currentQuest;

    public static event Action<QuestProgress> updateQuestProgress;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        EventBus<EnemyDieEvent>.OnEventPublished += UpdateEnemyProgress;
    }

    public void SetQuest(Quest quest)
    {
        if (currentQuest.quest == null || currentQuest.questID != quest.questID)
        {
            currentQuest = new QuestProgress(quest);
            updateQuestProgress?.Invoke(currentQuest);
        }
    }

    public void UpdateEnemyProgress(EnemyDieEvent enemyDieEvent)
    {
        if (currentQuest.quest == null) return;
        foreach (var objective in currentQuest.objectives)
        {
            if (objective.type != EObjectiveType.killEnemies || int.Parse(objective.objectiveID) != enemyDieEvent.enemy.ID || objective.isCompleted) continue;
            UpdateProgress(objective);
        }
    }

    private void UpdateProgress(QuestObjective objective)
    {
        if (objective.isCompleted) return;
        objective.currentAmount++;
        updateQuestProgress?.Invoke(currentQuest);
    }

    public QuestProgress GetCurrentQuest()
    {
        return currentQuest;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (currentQuest == null) return;
            foreach (var objective in currentQuest.objectives)
            {
                UpdateProgress(objective);
            }
        }
    }
}
