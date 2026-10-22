public class PlayerHitEvent : EventData
{
    public Enemy enemy;

    public PlayerHitEvent(Enemy pEnemy)
    {
        name = "PlayerHitEvent";
        enemy = pEnemy;
    }
}

public class EnemyDieEvent : EventData
{
    public Enemy enemy;

    public EnemyDieEvent(Enemy pEnemy)
    {
        name = "EnemyDieEvent";
        enemy = pEnemy;
    }
}
