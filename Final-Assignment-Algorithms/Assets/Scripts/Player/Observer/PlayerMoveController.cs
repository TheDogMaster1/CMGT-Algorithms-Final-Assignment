using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : PlayerObserver
{
    private Rigidbody2D rb;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float kbForce;

    private Vector2 moveVector;

    private bool inKnockBack = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(!inKnockBack) rb.linearVelocity = moveVector * movementSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    protected override void OnPlayerHit(PlayerData playerData, Enemy enemy)
    {
        inKnockBack = true;
        Vector2 kbVector = (transform.position - enemy.EnemyPos.position).normalized;
        kbVector.x = Mathf.Round(kbVector.x); 
        kbVector.y = Mathf.Round(kbVector.y);
        rb.AddForce(kbVector.normalized * kbForce, ForceMode2D.Impulse);
        Invoke("RegainControl", 0.07f);
    }

    private void RegainControl()
    {
        inKnockBack = false;
    }
}
