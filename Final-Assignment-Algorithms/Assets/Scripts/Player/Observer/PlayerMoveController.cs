using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : PlayerObserver
{
    private Rigidbody2D rb;

    [SerializeField]
    private float movementSpeed;

    private Vector2 moveVector;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveVector * movementSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public override void OnPlayerHit(PlayerData playerData)
    {
        //used for knockback later
    }
}
