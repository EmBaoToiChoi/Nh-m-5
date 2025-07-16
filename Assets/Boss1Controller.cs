using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public Animator animator;
    public Transform player;

    private float lastAttackTime;

    // ?? Thêm d?ng này ð? khai báo bi?n g?c
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // Lýu scale ban ð?u
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
            animator.SetBool("isMoving", false);
        }
        else
        {
            MoveTowardsPlayer();
            animator.SetBool("isMoving", true);
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // ? Flip ðúng cách
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        // Gây damage n?u c?n
    }

    public void OnDeath()
    {
        animator.SetTrigger("Die");
    }
}
