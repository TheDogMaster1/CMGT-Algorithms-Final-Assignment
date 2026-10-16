using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    public static Action<PlayerData> OnPlayerHit;

    public void OnHit(PlayerData playerData, DamageData damageData)
    {
        playerData.currentHp -= damageData.damage;
        OnPlayerHit?.Invoke(playerData);
        Debug.Log("Damaged");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        OnHit(playerData, playerData.damageData);
    }
}
