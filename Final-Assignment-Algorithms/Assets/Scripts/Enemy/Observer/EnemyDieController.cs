using UnityEngine;

public class EnemyDieController : EnemyObserver
{
    private bool isDead = false;
    protected override void OnEnemyCreated(Enemy enemy)
    {
        //nothing for now
    }

    protected override void OnEnemyHit(Enemy enemy, Transform playerTransform)
    {
        if (!isDead)
        {
            if(enemy.currentHP <= 0)
            {
                isDead = true;
                Destroy(gameObject);
            }
        }
    }
}
