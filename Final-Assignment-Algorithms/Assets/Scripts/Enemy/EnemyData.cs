using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int maxHP;
    public int Speed;
    public DamageData damageData;

    public Enemy CreateEnemy(Transform pTransform)
    {
        return new Enemy(maxHP, Speed, damageData, pTransform);
    }
}

public class Enemy
{
    public int MaxHP => maxHP;
    private int maxHP;
    public int currentHP;
    public int Speed => speed;
    private int speed;

    public DamageData DamageData => damageData;
    private DamageData damageData;

    public Transform EnemyPos => enemyPos;
    private Transform enemyPos;
    public Enemy(int pMaxHP, int pSpeed, DamageData pDamageData, Transform pEnemyPos)
    {
        maxHP = pMaxHP;
        currentHP = pMaxHP;
        speed = pSpeed;
        damageData = pDamageData;
        enemyPos = pEnemyPos;
    }
}
