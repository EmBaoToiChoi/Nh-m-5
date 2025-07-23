using System.Collections;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    [Header("Di chuy?n & T?n công")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public int attackDamage = 20;
    private float lastAttackTime;

    [Header("Player & Animation")]
    public Transform player;
    public Animator animator;

    [Header("Âm thanh")]
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip walkClip;

    [Header("Phun L?a")]
    public ParticleSystem fireBreath;
    public Transform firePoint;
    public float fireCooldown = 10f;
    public float fireDuration = 2f;
    private float lastFireTime;
    private bool isFiring;

    private Vector3 originalScale;
    private Rigidbody2D rb;

    void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();

        if (fireBreath != null)
            fireBreath.Stop(); // Không phun ngay t? ð?u
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            rb.velocity = Vector2.zero; // Ng?ng di chuy?n khi t?n công

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }

            animator.SetBool("isMoving", false);

            // Phun l?a cooldown
            if (!isFiring && Time.time - lastFireTime >= fireCooldown)
            {
                StartCoroutine(FireOnce());
            }
        }
        else
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        animator.SetBool("isMoving", true);

        // Flip Boss & l?a
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            if (fireBreath != null)
                fireBreath.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            if (fireBreath != null)
                fireBreath.transform.localScale = new Vector3(-1, 1, 1);
        }

        // Âm thanh bý?c ði
        if (walkClip != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkClip);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        if (attackClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackClip);
        }
    }

    IEnumerator FireOnce()
    {
        isFiring = true;
        lastFireTime = Time.time;

        if (fireBreath != null)
        {
            fireBreath.Play();
            Debug.Log("?? Boss2 b?t ð?u phun l?a");
        }

        yield return new WaitForSeconds(fireDuration);

        if (fireBreath != null)
        {
            fireBreath.Stop();
            Debug.Log("?? Boss2 ng?ng phun l?a");
        }

        isFiring = false;
    }
}
