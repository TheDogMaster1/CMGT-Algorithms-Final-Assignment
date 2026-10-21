using System;
using System.Collections;
using UnityEngine;

public abstract class AbstractEnemyMovingStrategy : MonoBehaviour
{
    [SerializeField]
    EnemyController enemyController;
    protected Enemy enemy;
    [SerializeField]
    protected Rigidbody2D rb;
    public Action<Vector2> onMove;
    protected void OnEnemyCreated(Enemy pEnemy) 
    {
        enemy = pEnemy;
        rb = GetComponent<Rigidbody2D>();
    }

    protected abstract void OnEnemyHit(Enemy enemy, Transform playerTransform);

    protected abstract void PlayerFound(Transform pPlayerPos);
    protected abstract void PlayerLost();

    private void OnEnable()
    {
        enemyController.onEnemyCreated += OnEnemyCreated;
        enemyController.onHit += OnEnemyHit;
        enemyController.onPlayerFound += PlayerFound;
        enemyController.onPlayerLost += PlayerLost;
    }

    private void OnDisable()
    {
        enemyController.onEnemyCreated -= OnEnemyCreated;
        enemyController.onHit -= OnEnemyHit;
        enemyController.onPlayerFound -= PlayerFound;
        enemyController.onPlayerLost -= PlayerLost;
    }
}
