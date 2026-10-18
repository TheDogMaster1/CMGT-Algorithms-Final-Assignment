using UnityEngine;

public class PlayerHitEvent : EventData
{
    public Enemy enemy;

    public PlayerHitEvent(Enemy pEnemy)
    {
        name = "PlayerHitEvent";
        enemy = pEnemy;
    }
}
