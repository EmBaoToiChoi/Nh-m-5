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
    public AudioSource audioSourceWalk;   // cho bý?c chân
    public AudioSource audioSourceSkill;  // cho attack & skill
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

        // Ð?t th?i gian l?n cu?i = -cooldown ð? test nhanh
        lastFireTime = -fireCooldown;
        lastSummonTime = -summonCooldown;
    }

    void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Ki?m tra phát hi?n Player
        if (!hasDetectedPlayer && distance <= detectRange)
        {
            hasDetectedPlayer = true;
        }

        // Tri?u h?i theo th?i gian khi ð? phát hi?n Player
        if (hasDetectedPlayer && Time.time - lastSummonTime >= summonCooldown)
        {
            StartCoroutine(UseSkill());
            lastSummonTime = Time.time;
        }

        // Tri?u h?i khi máu <30% khi ð? phát hi?n Player
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

        // Phun l?a theo cooldown
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

        // Flip Boss & xoay firePoint theo hý?ng
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            if (firePoint != null)
                firePoint.rotation = Quaternion.Euler(0, 0, 0); // b?n ph?i
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            if (firePoint != null)
                firePoint.rotation = Quaternion.Euler(0, 180, 0); // b?n trái
        }

        // Ch?y âm thanh bý?c chân n?u chýa phát
        if (walkClip != null && audioSourceWalk != null && !audioSourceWalk.isPlaying)
        {
            audioSourceWalk.PlayOneShot(walkClip);
        }
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

    void EndAttack()
    {
        isAttacking = false;
    }

    IEnumerator FireOnce()
    {
        Debug.Log("Boss b?t ð?u phun l?a!");
        isFiring = true;
        lastFireTime = Time.time;

        if (fireBreath != null)
        {
            fireBreath.Play();
            if (fireClip != null && audioSourceSkill != null)
                audioSourceSkill.PlayOneShot(fireClip);
        }

        yield return new WaitForSeconds(fireDuration);

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
    }
}
