using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;
    private Enemy enemy;

    private int enemyState = 0;

    public Action<Enemy> onEnemyCreated;
    public Action<Transform> onPlayerFound;
    public Action onPlayerLost;

    void Start()
    {
        enemy = enemyData.CreateEnemy(transform);
        onEnemyCreated.Invoke(enemy);
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
