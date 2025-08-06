using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss1Controller : MonoBehaviour
{
    [Header("Di chuy?n & T?n công")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public int attackDamage = 20;
    private float lastAttackTime;

    [Header("Player & Animator")]
    public Transform player;
    public Animator animator;

    [Header("Phát hi?n Player")]
    public float detectRange = 10f;
    private bool hasDetectedPlayer = false;

    [Header("Âm thanh")]
    public AudioSource audioSourceWalk;
    public AudioSource audioSourceSkill;
    public AudioClip attackClip;
    public AudioClip walkClip;
    public AudioClip skillClip;
    public AudioClip summonClip;
    public AudioClip fireClip;

    [Header("Tri?u h?i")]
    public GameObject skeletonPrefab;
    public int summonCount = 3;
    public float summonCooldown = 10f;
    private float lastSummonTime;
    private bool isLowHealthSummon = false;

    [Header("HP")]
    public int maxHealth = 200;
    private int currentHealth;
    private bool isDead = false;

    [Header("Fire Breath")]
    public ParticleSystem fireBreath;
    public Transform firePoint;
    public float fireCooldown = 10f;
    public float fireDuration = 2f;
    private float lastFireTime;
    private bool isFiring;
    public float fireDamagePerSecond = 10f;   // damage m?i giây
    public float fireRadius = 1.5f;           // bán kính vùng gây damage

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
    }

    void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Ki?m tra phát hi?n Player
        if (!hasDetectedPlayer && distance <= detectRange)
            hasDetectedPlayer = true;

        // Tri?u h?i theo th?i gian
        if (hasDetectedPlayer && Time.time - lastSummonTime >= summonCooldown)
        {
            StartCoroutine(UseSkill());
            lastSummonTime = Time.time;
        }

        // Tri?u h?i khi máu <30%
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
        }
        else
        {
            MoveToPlayer();
        }

        // Phun l?a
        if (!isFiring && Time.time - lastFireTime >= fireCooldown)
        {
            StartCoroutine(FireOnce());
        }
    }

    void MoveToPlayer()
    {
        if (isAttacking) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        animator.SetBool("isMoving", true);

        // Flip Boss & firePoint hý?ng player
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Ch?y âm thanh bý?c chân
        if (walkClip != null && audioSourceWalk != null && !audioSourceWalk.isPlaying)
            audioSourceWalk.PlayOneShot(walkClip);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
        Invoke(nameof(EndAttack), 0.5f);

        if (attackClip != null && audioSourceSkill != null)
            audioSourceSkill.PlayOneShot(attackClip);

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

    void EndAttack() => isAttacking = false;

    IEnumerator FireOnce()
    {
        isFiring = true;
        lastFireTime = Time.time;

        if (fireBreath != null)
        {
            fireBreath.Play();
            if (fireClip != null && audioSourceSkill != null)
                audioSourceSkill.PlayOneShot(fireClip);
        }

        float elapsed = 0f;
        float damageInterval = 0.2f; // 5 l?n/s
        float damageTimer = 0f;

        while (elapsed < fireDuration)
        {
            elapsed += Time.deltaTime;
            damageTimer += Time.deltaTime;

            // Xoay firePoint liên t?c theo player
            if (firePoint != null && player != null)
            {
                Vector2 dir = (player.position - firePoint.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(0, 0, angle);
            }

            // Gây damage ð?nh k?
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;

                Collider2D[] hits = Physics2D.OverlapCircleAll(firePoint.position, fireRadius);
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Player1"))
                    {
                        Player1 p = hit.GetComponent<Player1>();
                        if (p != null)
                            p.TakeDamage(Mathf.RoundToInt(fireDamagePerSecond * damageInterval));
                    }
                }
            }

            yield return null;
        }

        if (fireBreath != null)
            fireBreath.Stop();

        isFiring = false;
    }

    IEnumerator UseSkill()
    {
        if (skillClip != null && audioSourceSkill != null)
            audioSourceSkill.PlayOneShot(skillClip);

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

            if (summonClip != null && audioSourceSkill != null)
                audioSourceSkill.PlayOneShot(summonClip);

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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(firePoint != null ? firePoint.position : transform.position, fireRadius);
    }
}
