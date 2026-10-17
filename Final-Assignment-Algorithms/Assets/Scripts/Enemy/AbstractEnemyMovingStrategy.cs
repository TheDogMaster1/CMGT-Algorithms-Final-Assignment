using System.Collections;
using UnityEngine;

public abstract class AbstractEnemyMovingStrategy : MonoBehaviour
{
    [SerializeField]
    EnemyController enemyController;

    public abstract void PlayerFound(Transform pPlayerPos, Enemy pEnemy);
    public abstract void PlayerLost();

    private void OnEnable()
    {
        enemyController.onPlayerFound = PlayerFound;
        enemyController.onPlayerLost = PlayerLost;
    }

    private void OnDisable()
    {
        enemyController.onPlayerFound = PlayerFound;
        enemyController.onPlayerLost = PlayerLost;
    }
}
