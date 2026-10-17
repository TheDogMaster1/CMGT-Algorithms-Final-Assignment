using TreeEditor;
using UnityEngine;

public class FollowPlayerEnemyMoveStrategy : AbstractEnemyMovingStrategy 
{
    private Rigidbody2D rb;
    private Vector2 moveVector;
    private Transform playerPos = null;
    private float speed = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (playerPos == null) return;
        //transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
        moveVector = (playerPos.position - transform.position).normalized;
        rb.linearVelocity = moveVector * speed;
    }

    public override void PlayerFound(Transform pPlayerPos, Enemy enemy)
    {
        playerPos = pPlayerPos;
        speed = enemy.Speed;
    }
    public override void PlayerLost()
    {
        Debug.Log("player lost");
        rb.linearVelocity = Vector2.zero;
        playerPos = null;
    }
}
