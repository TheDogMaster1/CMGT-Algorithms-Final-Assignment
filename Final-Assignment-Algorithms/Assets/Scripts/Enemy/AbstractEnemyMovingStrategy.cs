using System.Collections;
using UnityEngine;

public abstract class AbstractEnemyMovingStrategy : MonoBehaviour
{
    [SerializeField]
    EnemyController enemyController;
    protected Enemy enemy;

    private void OnEnemyCreated(Enemy pEnemy) 
    {
        enemy = pEnemy;
    }

    public abstract void PlayerFound(Transform pPlayerPos);
    public abstract void PlayerLost();

    private void OnEnable()
    {
        enemyController.onEnemyCreated += OnEnemyCreated;
        enemyController.onPlayerFound += PlayerFound;
        enemyController.onPlayerLost += PlayerLost;
    }

    private void OnDisable()
    {
        enemyController.onEnemyCreated -= OnEnemyCreated;
        enemyController.onPlayerFound -= PlayerFound;
        enemyController.onPlayerLost -= PlayerLost;
    }
}
