using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;
    private Enemy enemy;

    private int enemyState = 0;

    public Action<Transform, Enemy> onPlayerFound;
    public Action onPlayerLost;

    void Start()
    {
        enemy = enemyData.CreateEnemy(transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemyState == 0) //detected
            {
                onPlayerFound?.Invoke(collision.transform, enemy);
            }
            else //hit
            {
                EventBus<PlayerHitEvent>.Publish(new PlayerHitEvent(enemy, gameObject));
            }
            enemyState++;
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
