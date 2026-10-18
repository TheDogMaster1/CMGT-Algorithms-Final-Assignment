using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 moveVector = context.ReadValue<Vector2>();
        if (moveVector.magnitude == 0)
        {
            animator.SetBool("Moving", false);
        }
        else
        {
            animator.SetBool("Moving", true);
            if (moveVector.y > 0f) animator.Play("WalkUp");
            if (moveVector.y < 0f) animator.Play("WalkDown");
            if (moveVector.x > 0f) animator.Play("WalkRight");
            if (moveVector.x < 0f) animator.Play("WalkLeft");
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if(context.started) animator.SetTrigger("Attack");
    }
}
