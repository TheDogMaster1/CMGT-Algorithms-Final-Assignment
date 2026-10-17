using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    public static Action<PlayerData, Enemy> OnPlayerHit;

    private void OnHit(Enemy enemy)
    {
        if (!playerData.invulnerable)
        {
            playerData.currentHp -= enemy.DamageData.damage;
        Debug.Log("Damaged");
        }
        OnPlayerHit?.Invoke(playerData, enemy);
            playerData.invulnerable = true;
        Invoke("StopInvulnerability", playerData.invulnerabilityTime);
    }

    private void StopInvulnerability()
    {
        playerData.invulnerable = false;
    }

    private void OnEnable()
    {
        EnemyController.onPlayerHit += OnHit;   
    }

    private void OnDisable()
    {
        EnemyController.onPlayerHit -= OnHit;
    }
}
