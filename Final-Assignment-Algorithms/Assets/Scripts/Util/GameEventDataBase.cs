public class PlayerHitEvent : EventData
{
    public readonly int damage;

    public PlayerHitEvent(int pDamage)
    {
        damage = pDamage;
    }
}
