using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss2Controller : MonoBehaviour
{
    [Header("Di chuy?n & T?n công")]
    public float moveSpeed = 5f;
    public float chaseSpeedMultiplier = 1.5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.8f;
    public int attackDamage = 25;
    private float lastAttackTime;

    [Header("Player & Animator")]
    public Transform player;
    public Animator animator;

    [Header("Phát hi?n Player")]
    public float detectRange = 10f;    // Kho?ng cách ð? b?t ð?u dùng skill
    private bool hasDetectedPlayer = false;

    [Header("Teleport")]
    public float teleportDistanceThreshold = 8f;
    public float teleportCooldown = 5f;
    private float lastTeleportTime;

    [Header("Âm thanh")]
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip walkClip;
    public AudioClip skillClip;
    public AudioClip summonClip;
    public AudioClip fireClip;
    public AudioClip teleportClip;

    [Header("Tri?u h?i")]
    public GameObject skeletonPrefab;
    public int summonCount = 3;
    public float summonCooldown = 8f;
    private float lastSummonTime;
    private bool isLowHealthSummon = false;

    [Header("HP")]
    public int maxHealth = 300;
    private int currentHealth;
    private bool isDead = false;

    [Header("Fire Breath")]
    public ParticleSystem fireBreath;
    public Transform firePoint;
    public float fireCooldown = 7f;
    public float fireDuration = 2f;
    private float lastFireTime;
    private bool isFiring;

    private Rigidbody2D rb;
    private Vector3 originalScale;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        currentHealth = maxHealth;

        if (fireBreath != null)
            fireBreath.Stop();

        lastFireTime = -fireCooldown;
        lastSummonTime = -summonCooldown;
        lastTeleportTime = -teleportCooldown;
    }

    void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Khi th?y Player trong ph?m vi th? m?i b?t ð?u dùng skill
        if (!hasDetectedPlayer && distance <= detectRange)
        {
            hasDetectedPlayer = true;
        }

        // Teleport n?u player quá xa và h?t cooldown
        if (distance >= teleportDistanceThreshold && Time.time - lastTeleportTime >= teleportCooldown)
        {
            TeleportToPlayer();
        }

        // Ch? tri?u h?i khi ð? phát hi?n Player
        if (hasDetectedPlayer && Time.time - lastSummonTime >= summonCooldown)
        {
            StartCoroutine(UseSkill());
            lastSummonTime = Time.time;
        }

        // Ch? tri?u h?i khi máu th?p sau khi phát hi?n Player
        if (hasDetectedPlayer && !isLowHealthSummon && currentHealth <= maxHealth * 0.3f)
        {
            StartCoroutine(SummonSkeletons(5));
            isLowHealthSummon = true;
        }

        // Di chuy?n ho?c t?n công
        if (distance <= attackRange)
        {
            rb.velocity = Vector2.zero;

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }

            animator.SetBool("isMoving", false);

            // Phun l?a ch? khi ð? phát hi?n Player
            if (hasDetectedPlayer && !isFiring && Time.time - lastFireTime >= fireCooldown)
            {
                StartCoroutine(FireOnce());
            }
        }
        else
        {
            MoveToPlayer(distance);
        }
    }

    void MoveToPlayer(float distance)
    {
        if (isAttacking) return;

        Vector2 direction = (player.position - transform.position).normalized;
        float speed = moveSpeed;
        if (distance < 4f) speed *= chaseSpeedMultiplier;

        rb.velocity = direction * speed;
        animator.SetBool("isMoving", true);

        // Flip Boss & FirePoint
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            if (firePoint != null)
                firePoint.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            if (firePoint != null)
                firePoint.localScale = new Vector3(-1, 1, 1);
        }

        if (walkClip != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkClip);
        }
    }

    void TeleportToPlayer()
    {
        lastTeleportTime = Time.time;
        transform.position = player.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);

        if (teleportClip != null && audioSource != null)
            audioSource.PlayOneShot(teleportClip);

        Debug.Log("Boss2 teleport ð?n Player!");
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
        Invoke(nameof(EndAttack), 0.5f);

        if (attackClip != null) audioSource.PlayOneShot(attackClip);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player1"))
            {
                Player1 p = hit.GetComponent<Player1>();
                if (p != null) p.TakeDamage(attackDamage);
            }
        }
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    IEnumerator FireOnce()
    {
        isFiring = true;
        lastFireTime = Time.time;

        if (fireBreath != null)
        {
            fireBreath.Play();
            if (fireClip != null) audioSource.PlayOneShot(fireClip);
        }

        yield return new WaitForSeconds(fireDuration);

        if (fireBreath != null)
            fireBreath.Stop();

        isFiring = false;
    }

    IEnumerator UseSkill()
    {
        if (skillClip != null) audioSource.PlayOneShot(skillClip);

        animator.SetTrigger("Skill");

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(SummonSkeletons(summonCount));
    }

    IEnumerator SummonSkeletons(int count)
    {
        if (skeletonPrefab == null) yield break;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            GameObject skel = Instantiate(skeletonPrefab, pos, Quaternion.identity);

            SmallSkeleton skelScript = skel.GetComponent<SmallSkeleton>();
            if (skelScript != null)
                skelScript.canRespawnSmall = true;

            if (summonClip != null) audioSource.PlayOneShot(summonClip);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, teleportDistanceThreshold);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
