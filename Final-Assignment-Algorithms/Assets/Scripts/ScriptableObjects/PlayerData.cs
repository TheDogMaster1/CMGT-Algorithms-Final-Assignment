using System;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public int maxHp;
    public int currentHp;
    public float invulnerabilityTime;
    public bool invulnerable = false;
    public DamageData damageData;
}
