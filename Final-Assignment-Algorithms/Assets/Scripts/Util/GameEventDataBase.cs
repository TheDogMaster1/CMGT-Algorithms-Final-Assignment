using UnityEngine;

public class PlayerHitEvent : EventData
{
    public Enemy enemy;

    public GameObject enemyObject;

    public PlayerHitEvent(Enemy pEnemy, GameObject pEnemyObject)
    {
        name = "PlayerHitEvent";
        enemy = pEnemy;
        enemyObject = pEnemyObject;
    }
}
