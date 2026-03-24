using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss1Controller : MonoBehaviour
{
    [Header("Di chuyển & Tấn công")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public int attackDamage = 20;
    private float lastAttackTime;

    [Header("Player & Animator")]
    public Transform player;     // Player 1
    public Transform player2;    // Player 2
    public Transform player3;    // Player 3
    public Animator animator;

    [Header("Phát hiện Player")]
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

    [Header("Triệu hồi")]
    public GameObject skeletonPrefab;
    public int summonCount = 3;
    public float summonCooldown = 10f;
    private float lastSummonTime;
    private bool isLowHealthSummon = false;

    [Header("HP")]
    public int maxHealth = 700;
    private int currentHealth;
    private bool isDead = false;

    [Header("Fire Breath")]
    public ParticleSystem fireBreath;
    public Transform firePoint;
    public float fireCooldown = 10f;
    public float fireDuration = 2f;
    private float lastFireTime;
    private bool isFiring;
    public float fireDamagePerSecond = 10f;   // damage mỗi giây
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
        Transform target = GetNearestPlayer();
        if (target == null || isDead) return;

        float distance = Vector2.Distance(transform.position, target.position);

        // Kiểm tra phát hiện Player
        if (!hasDetectedPlayer && distance <= detectRange)
            hasDetectedPlayer = true;

        // Triệu hồi theo thời gian
        if (hasDetectedPlayer && Time.time - lastSummonTime >= summonCooldown)
        {
            StartCoroutine(UseSkill());
            lastSummonTime = Time.time;
        }

        // Triệu hồi khi máu <30%
        if (hasDetectedPlayer && !isLowHealthSummon && currentHealth <= maxHealth * 0.3f)
        {
            StartCoroutine(SummonSkeletons(5));
            isLowHealthSummon = true;
        }

        // Di chuyển hoặc tấn công
        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack(target);
                lastAttackTime = Time.time;
            }
            animator.SetBool("isMoving", false);
        }
        else
        {
            MoveToPlayer(target);
        }

        // Phun lửa
        if (!isFiring && Time.time - lastFireTime >= fireCooldown)
        {
            StartCoroutine(FireOnce(target));
        }
    }

    Transform GetNearestPlayer()
    {
        Transform nearest = null;
        float minDist = float.MaxValue;

        Transform[] players = { player, player2, player3 };
        foreach (Transform pl in players)
        {
            if (pl == null) continue;
            float dist = Vector2.Distance(transform.position, pl.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = pl;
            }
        }
        return nearest;
    }

    void MoveToPlayer(Transform target)
    {
        if (isAttacking) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        animator.SetBool("isMoving", true);

        // Flip Boss & firePoint hướng player
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Chạy âm thanh bước chân
        if (walkClip != null && audioSourceWalk != null && !audioSourceWalk.isPlaying)
            audioSourceWalk.PlayOneShot(walkClip);
    }

    void Attack(Transform target)
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
        Invoke(nameof(EndAttack), 0.5f);

        if (attackClip != null && audioSourceSkill != null)
            audioSourceSkill.PlayOneShot(attackClip);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
    }

    void EndAttack() => isAttacking = false;

    IEnumerator FireOnce(Transform target)
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
        float damageInterval = 0.2f; // 5 lần/s
        float damageTimer = 0f;

        while (elapsed < fireDuration)
        {
            elapsed += Time.deltaTime;
            damageTimer += Time.deltaTime;

            // Xoay firePoint liên tục theo player
            if (firePoint != null && target != null)
            {
                Vector2 dir = (target.position - firePoint.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(0, 0, angle);
            }

            // Gây damage định kỳ
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;

                Collider2D[] hits = Physics2D.OverlapCircleAll(firePoint.position, fireRadius);
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

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= (int)amount;
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
        rb.linearVelocity = Vector2.zero;
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hit"))
        {
            float damage = Random.Range(1f, 6f);
            TakeDamage(damage + GlobalData.damageBonus);
        }
        else if (other.CompareTag("Bullet"))
        {
            float damage = Random.Range(10f, 16f);
            TakeDamage(damage + GlobalData.damageBonus);
        }
        else if (other.CompareTag("Bow"))
        {
            float damage = Random.Range(5f, 11f);
            TakeDamage(damage + GlobalData.damageBonus);
        }
    }
}
