using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int maxHP;
    public float Speed;
    public DamageData damageData;

    public Enemy CreateEnemy(Transform pTransform)
    {
        return new Enemy(maxHP, Speed, damageData, pTransform);
    }
}

[Serializable]
public class Enemy
{
    public int MaxHP => maxHP;
    private int maxHP;
    public int currentHP;
    public float Speed => speed;
    private float speed;

    public DamageData DamageData => damageData;
    private DamageData damageData;

    public Transform EnemyPos => enemyPos;
    private Transform enemyPos;
    public Enemy(int pMaxHP, float pSpeed, DamageData pDamageData, Transform pEnemyPos)
    {
        maxHP = pMaxHP;
        currentHP = pMaxHP;
        speed = pSpeed;
        damageData = pDamageData;
        enemyPos = pEnemyPos;
    }
}
