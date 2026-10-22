using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D attackHitBox;
    private PlayerLevelController playerLevelController;

    private void Start()
    {
        playerLevelController = GetComponentInParent<PlayerLevelController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveVector = context.ReadValue<Vector2>();
        if (moveVector.y > 0f) attackHitBox.offset = new Vector2(0, 1) / 3;
        if (moveVector.y < 0f) attackHitBox.offset = new Vector2(0, -1) / 3;
        if (moveVector.x > 0f) attackHitBox.offset = new Vector2(1, 0) / 3;
        if (moveVector.x < 0f) attackHitBox.offset = new Vector2(-1, 0) / 3;
    }

    private void OnAttack()
    {
        if (attackHitBox != null) attackHitBox.enabled = true;
    }

    private void EndAttack()
    {
        if (attackHitBox != null) attackHitBox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("hit enemy: " + collision.gameObject.name + "with collider: " + this.name);
            EnemyController enemyController = collision.GetComponentInParent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.GetHit(playerLevelController.GetDamageData(), transform.parent.transform);
            }
            else
            {
                Debug.Log("Didn't find enemycontroller");
            }
        }
    }

    private void OnEnable()
    {
        AttackAnimationBehaviour.onAttack += OnAttack;
        AttackAnimationBehaviour.onAttackComplete += EndAttack;
    }

    private void OnDisable()
    {
        AttackAnimationBehaviour.onAttack -= OnAttack;
        AttackAnimationBehaviour.onAttackComplete -= EndAttack;
    }
}
