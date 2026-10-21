using UnityEngine;

public class HitVFXController : PlayerObserver
{
    [SerializeField]
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    protected override void OnPlayerHit(PlayerData playerData, Enemy enemy)
    {
        if (playerData.invulnerable) return;
        _animator.Play("PlayerHit");
    }
}
