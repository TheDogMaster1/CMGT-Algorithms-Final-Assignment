using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;
    private Enemy enemy;

    public static Action<Enemy> onPlayerHit;

    void Start()
    {
        enemy = enemyData.CreateEnemy(transform);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Collided with " +  collision.gameObject.name);
            onPlayerHit?.Invoke(enemy);
        }
    }

}
