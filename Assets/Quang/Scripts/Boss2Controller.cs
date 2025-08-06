using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss2Controller : MonoBehaviour
{
    [Header("Di chuyển & Tấn công")]
    public float moveSpeed = 5f;
    public float chaseSpeedMultiplier = 1.5f;
    public float idlePatrolSpeed = 1f;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.8f;
    public int attackDamage = 25;
    private float lastAttackTime;

    [Header("Player & Animator")]
    public Transform player;     // Player 1
    public Transform player2;    // Player 2
    public Transform player3;    // Player 3
    public Animator animator;

    [Header("Phát hiện Player")]
    public float detectRange = 6f;
    private bool hasDetectedPlayer = false;

    [Header("Teleport")]
    public float teleportTriggerRange = 12f;
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

    [Header("Triệu hồi")]
    public GameObject skeletonPrefab;
    public int summonCount = 3;
    public float summonCooldown = 8f;
    private float lastSummonTime;
    private bool isLowHealthSummon = false;

    [Header("HP & Revive")]
    public int maxHealth = 300;
    private int currentHealth;
    private bool isDead = false;
    public int extraLives = 1;
    public float reviveDelay = 1.5f;
    private bool isReviving = false;

    [Header("Fire Breath")]
    public ParticleSystem fireBreath;
    public Transform firePoint;
    public Transform mouthTransform;
    public float fireCooldown = 7f;
    public float fireDuration = 2f;
    public float fireDamagePerSecond = 15f;
    public float fireRadius = 1.5f;
    private float lastFireTime;
    private bool isFiring;

    private Rigidbody2D rb;
    private Vector3 originalScale;
    private bool isAttacking = false;

    // Idle patrol
    private float patrolTimer = 0f;
    private int patrolDirection = 1;

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
        if ((player == null && player2 == null) || isDead) return;

        // Lấy player gần nhất
        Transform targetPlayer = GetNearestPlayer();
        if (targetPlayer == null) return;

        float distance = Vector2.Distance(transform.position, targetPlayer.position);

        // Phát hiện Player
        if (!hasDetectedPlayer && distance <= detectRange)
        {
            hasDetectedPlayer = true;
            Debug.Log("Boss2 phát hiện player!");
        }

        // Nếu chưa phát hiện, đi tuần
        if (!hasDetectedPlayer)
        {
            IdlePatrol();
            return;
        }

        // Teleport khi player quá xa
        if (distance >= teleportTriggerRange && Time.time - lastTeleportTime >= teleportCooldown)
        {
            TeleportToPlayer(targetPlayer);
        }

        // Triệu hồi theo cooldown
        if (Time.time - lastSummonTime >= summonCooldown)
        {
            StartCoroutine(UseSkill());
            lastSummonTime = Time.time;
        }

        // Triệu hồi khi máu thấp
        if (!isLowHealthSummon && currentHealth <= maxHealth * 0.3f)
        {
            StartCoroutine(SummonSkeletons(5));
            isLowHealthSummon = true;
        }

        // Di chuyển hoặc tấn công
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
            MoveToPlayer(distance, targetPlayer);
        }

        // Phun lửa theo cooldown
        if (!isFiring && Time.time - lastFireTime >= fireCooldown)
        {
            StartCoroutine(FireOnce(targetPlayer));
        }
    }

    Transform GetNearestPlayer()
    {
        Transform nearest = null;
        float minDist = float.MaxValue;

        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist < minDist) { minDist = dist; nearest = player; }
        }

        if (player2 != null)
        {
            float dist = Vector2.Distance(transform.position, player2.position);
            if (dist < minDist) { minDist = dist; nearest = player2; }
        }

        return nearest;
    }

    void IdlePatrol()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= 2f)
        {
            patrolTimer = 0f;
            patrolDirection *= -1;
            FlipBoss(new Vector2(patrolDirection, 0));
        }

        rb.velocity = new Vector2(patrolDirection * idlePatrolSpeed, 0f);
        animator.SetBool("isMoving", true);
    }

    void MoveToPlayer(float distance, Transform target)
    {
        if (isAttacking) return;

        Vector2 direction = (target.position - transform.position).normalized;
        float speed = moveSpeed;
        if (distance < 4f) speed *= chaseSpeedMultiplier;

        rb.velocity = direction * speed;
        animator.SetBool("isMoving", true);

        FlipBoss(direction);

        if (walkClip != null && audioSource != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(walkClip);
    }

    void FlipBoss(Vector2 direction)
    {
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void TeleportToPlayer(Transform target)
    {
        lastTeleportTime = Time.time;
        transform.position = target.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);

        Vector2 dir = (target.position - transform.position).normalized;
        FlipBoss(dir);

        if (mouthTransform != null && firePoint != null)
            firePoint.position = mouthTransform.position;

        if (firePoint != null)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (teleportClip != null && audioSource != null)
            audioSource.PlayOneShot(teleportClip);

        Debug.Log("Boss2 teleport đến Player!");
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
            if (hit.CompareTag("Player1") || hit.CompareTag("Player2"))
            {
                Player1 p = hit.GetComponent<Player1>();
                if (p != null) p.TakeDamage(attackDamage);
            }
        }
    }

    void EndAttack() => isAttacking = false;

    IEnumerator FireOnce(Transform target)
    {
        isFiring = true;
        lastFireTime = Time.time;

        if (fireBreath != null)
        {
            fireBreath.Play();
            if (fireClip != null) audioSource.PlayOneShot(fireClip);
        }

        float elapsed = 0f;
        float damageInterval = 0.2f;
        float damageTimer = 0f;

        while (elapsed < fireDuration)
        {
            elapsed += Time.deltaTime;
            damageTimer += Time.deltaTime;

            if (mouthTransform != null && firePoint != null)
                firePoint.position = mouthTransform.position;

            if (firePoint != null && target != null)
            {
                Vector2 dir = (target.position - firePoint.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(0, 0, angle);
            }

            // Gây damage nếu cần
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                Collider2D[] hits = Physics2D.OverlapCircleAll(firePoint.position, fireRadius);
                // TODO: thêm damage cho Player nếu cần
            }

            yield return null;
        }

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
        if (isDead || isReviving) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (extraLives > 0)
            {
                StartCoroutine(Revive());
            }
            else
            {
                Die();
            }
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    IEnumerator Revive()
    {
        isReviving = true;
        isDead = true;
        rb.velocity = Vector2.zero;

        animator.SetTrigger("Die");
        yield return new WaitForSeconds(reviveDelay);

        extraLives--;
        currentHealth = maxHealth;
        isDead = false;
        isReviving = false;

        animator.SetTrigger("Revive");
        Debug.Log("Boss2 hồi sinh! Mạng còn lại: " + extraLives);
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
        Gizmos.DrawWireSphere(transform.position, teleportTriggerRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.magenta;
        if (firePoint != null)
            Gizmos.DrawWireSphere(firePoint.position, fireRadius);
    }
}
