using System;
using UnityEngine;

public class PlayerLevelController : MonoBehaviour
{
    public event Action<int> onLevelUp;

    [Header("PlayerDamageStats")]
    [SerializeField]
    private DamageData initialDamageData;
    private DamageData damageData;
    [SerializeField]
    private int damageincrease;

    [Header("LevelStats")]
    [SerializeField]
    private int[] levelXpRequirements;

    private int currentXp = 0;

    [Min(1)]
    private int currentLevel = 1;

    private void Start()
    {
        damageData = initialDamageData;
    }

    private void OnEnemyDeath(EnemyDieEvent enemyDieEvent)
    {
        if (currentLevel <= levelXpRequirements.Length)
        {
            currentXp += enemyDieEvent.enemy.Xp;

            int previousLevel = currentLevel;

            while (currentXp >= levelXpRequirements[currentLevel - 1])
            {
                currentXp -= levelXpRequirements[currentLevel - 1];
                damageData.damage += damageincrease;

                currentLevel++;

                if (currentLevel > levelXpRequirements.Length)
                {
                    break;
                }
            }

            if (previousLevel < currentLevel)
            {
                Debug.Log("leveled up to level " + currentLevel);
                onLevelUp?.Invoke(currentLevel);
            }
        }
    }

    public DamageData GetDamageData()
    {
        return damageData;
    }

    private void OnEnable()
    {
        EventBus<EnemyDieEvent>.OnEventPublished += OnEnemyDeath;
    }

    private void OnDisable()
    {
        EventBus<EnemyDieEvent>.OnEventPublished -= OnEnemyDeath;
    }

}
