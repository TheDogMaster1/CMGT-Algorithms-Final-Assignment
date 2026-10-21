using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;
    private Enemy enemy;

    private int enemyState = 0;

    public event Action<Enemy> onEnemyCreated;
    public event Action<Enemy, Transform> onHit;

    public event Action<Transform> onPlayerFound;
    public event Action onPlayerLost;

    void Start()
    {
        enemy = enemyData.CreateEnemy(transform);
        onEnemyCreated?.Invoke(enemy);
    }

    public void GetHit(PlayerData playerData, Transform playerTransform)
    {
        enemy.currentHP -= playerData.damageData.damage;
        if (enemy.currentHP < 0)
        {
            enemy.currentHP = 0;
        }
        onHit.Invoke(enemy, playerTransform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyState++;
            if (enemyState == 1)
            {
                onPlayerFound?.Invoke(collision.transform);
            }
            if (enemyState == 2) //hit
            {
                EventBus<PlayerHitEvent>.Publish(new PlayerHitEvent(enemy));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyState--;
            if (enemyState == 0)
            {
                onPlayerLost?.Invoke();
            }
        }
    }
}
