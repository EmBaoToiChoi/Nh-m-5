using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public int attackDamage = 20;
    private float lastAttackTime;

    public Transform player;
    public Animator animator;

    [Header("Âm thanh")]
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip walkClip;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
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
            MoveToPlayer();
            animator.SetBool("isMoving", true);

            // Chõi âm thanh ði b?
            if (walkClip != null && audioSource != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(walkClip);
            }
        }
    }

    void MoveToPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        // Chõi âm thanh ðánh
        if (attackClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackClip);
        }

        
    }
}
