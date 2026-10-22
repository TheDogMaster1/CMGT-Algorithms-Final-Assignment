using System.Collections;
using UnityEngine;

public class FollowPlayerEnemyMoveStrategy : AbstractEnemyMovingStrategy
{
    private Vector2 moveVector = Vector2.zero;
    private Transform playerPos = null;
    private bool inKnockBack = false;
    [SerializeField]
    private float kbForce = 20;

    private void FixedUpdate()
    {
        if (playerPos == null || inKnockBack) return;
        //transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
        moveVector = (playerPos.position - transform.position).normalized;
        rb.linearVelocity = moveVector * enemy.Speed;
    }

    protected override void OnEnemyHit(Enemy enemy, Transform playerTransform)
    {
        inKnockBack = true;
        rb.linearVelocity = Vector2.zero;
        Vector2 kbVector = (transform.position - playerTransform.position).normalized;
        kbVector.x = Mathf.Round(kbVector.x);
        kbVector.y = Mathf.Round(kbVector.y);
        rb.AddForce(kbVector.normalized * kbForce, ForceMode2D.Impulse);
        StartCoroutine(BeStunned());
    }

    private IEnumerator BeStunned()
    {
        yield return new WaitForSeconds(0.07f);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(.5f);
        inKnockBack = false;
    }
    protected override void PlayerFound(Transform pPlayerPos)
    {
        playerPos = pPlayerPos;
    }
    protected override void PlayerLost()
    {
        Debug.Log("player lost");
        rb.linearVelocity = Vector2.zero;
        playerPos = null;
    }
}
