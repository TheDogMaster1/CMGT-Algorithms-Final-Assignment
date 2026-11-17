using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int ID;
    public int maxHP;
    public float speed;
    public int xp;
    public int money;
    public DamageData damageData;

    public Enemy CreateEnemy(Transform pTransform)
    {
        return new Enemy(ID, maxHP, speed, xp, money, damageData, pTransform);
    }
}

[Serializable]
public class Enemy
{
    public int ID => iD;
    private int iD;
    public int MaxHP => maxHP;
    private int maxHP;
    public int currentHP;
    public float Speed => speed;
    private float speed;
    public int Xp => xp;
    private int xp;
    public int Money => money;
    private int money;
    public DamageData DamageData => damageData;
    private DamageData damageData;

    public Transform EnemyPos => enemyPos;
    private Transform enemyPos;
    public Enemy(int pID, int pMaxHP, float pSpeed, int pXp, int pMoney, DamageData pDamageData, Transform pEnemyPos)
    {
        iD = pID;
        maxHP = pMaxHP;
        currentHP = pMaxHP;
        speed = pSpeed;
        xp = pXp;
        money = pMoney;
        damageData = pDamageData;
        enemyPos = pEnemyPos;
    }
}
