using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField]
    private AbstractEnemyMovingStrategy strategy;
    private Animator animator;
    private Rigidbody2D rb;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        strategy = GetComponentInParent<AbstractEnemyMovingStrategy>();
    }

    private void OnMove(Vector2 moveVector)
    {
        if (moveVector.magnitude == 0)
        {
            animator.SetBool("Moving", false);
        }
        else
        {
            animator.SetBool("Moving", true);
            if (Mathf.Abs(moveVector.y) > Mathf.Abs(moveVector.x))
            {
                if (moveVector.y > 0f) animator.Play("SlimeWalkUp");
                if (moveVector.y < 0f) animator.Play("SlimeWalkDown");
            }
            else
            {
                if (moveVector.x > 0f) animator.Play("SlimeWalkRight");
                if (moveVector.x < 0f) animator.Play("SlimeWalkLeft");
            }
        }
    }

    private void OnEnable()
    {
        strategy.onMove += OnMove;
    }

    private void OnDisable()
    {
        strategy.onMove -= OnMove;
    }
}
