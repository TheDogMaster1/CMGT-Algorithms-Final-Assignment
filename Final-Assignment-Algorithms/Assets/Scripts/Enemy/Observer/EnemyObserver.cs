using UnityEngine;

public abstract class EnemyObserver : MonoBehaviour
{
    [SerializeField]
    protected EnemyController enemyController;
    protected void OnEnable()
    {
        enemyController.onEnemyCreated += OnEnemyCreated;
        enemyController.onHit += OnEnemyHit;
    }

    protected void OnDisable()
    {
        enemyController.onEnemyCreated -= OnEnemyCreated;
        enemyController.onHit -= OnEnemyHit;
    }

    protected abstract void OnEnemyCreated(Enemy enemy);

    protected abstract void OnEnemyHit(Enemy enemy, Transform playerTransform);

}
