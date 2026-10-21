using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    public event Action<PlayerData, Enemy> onPlayerHit;

    private void GetHit(PlayerHitEvent playerHitEvent)
    {
        if (!playerData.invulnerable)
        {
            playerData.currentHp -= playerHitEvent.enemy.DamageData.damage;
        Debug.Log("Damaged");
        }
        onPlayerHit?.Invoke(playerData, playerHitEvent.enemy);
            playerData.invulnerable = true;
        Invoke("StopInvulnerability", playerData.invulnerabilityTime);
    }

    private void StopInvulnerability()
    {
        playerData.invulnerable = false;
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    private void OnEnable()
    {
        EventBus<PlayerHitEvent>.OnEventPublished += GetHit;
    }

    private void OnDisable()
    {
        EventBus<PlayerHitEvent>.OnEventPublished -= GetHit;
    }
}
