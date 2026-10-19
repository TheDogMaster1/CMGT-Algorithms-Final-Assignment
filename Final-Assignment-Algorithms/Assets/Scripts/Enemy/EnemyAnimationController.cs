using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class EnemyAnimationController : MonoBehaviour
{
    private AbstractEnemyMovingStrategy strategy;
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
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
            animator.SetFloat("CurrentX", Mathf.Round(moveVector.x));
            animator.SetFloat("CurrentY", Mathf.Round(moveVector.y));
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
