using UnityEngine;

[CreateAssetMenu(fileName = "KillEnemyQuestData", menuName = "Scriptable Objects/Quests/KillEnemyQuestData")]
public class KillEnemyQuestData : Quest
{
    public int killsNeeded = 0;
    [SerializeField]
    private int killsAmount = 0;

    private void OnEnemyDeath(EnemyDieEvent enemyDieEvent)
    {
        killsAmount++;
    }

    public override void StartQuest()
    {
        base.StartQuest();
        EventBus<EnemyDieEvent>.OnEventPublished += OnEnemyDeath;
    }

    public override void StopQuest()
    {
        base.StopQuest();
        EventBus<EnemyDieEvent>.OnEventPublished -= OnEnemyDeath;
    }

    private void OnEnable()
    {
        killsAmount = 0;
    }

    private void OnDisable()
    {
        EventBus<EnemyDieEvent>.OnEventPublished -= OnEnemyDeath;
    }
}
