using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int maxHp;
    public int currentHp;
    public float invulnerabilityTime;
    [HideInInspector]
    public bool invulnerable = false;
}
