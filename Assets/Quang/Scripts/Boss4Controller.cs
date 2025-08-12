using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boss4Controller : MonoBehaviour
{
    [Header("Di chuy?n & T?n c�ng")]
    public float moveSpeed = 5f;
    public float chaseSpeedMultiplier = 1.5f;
    public float idlePatrolSpeed = 1f;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.8f;
    public int attackDamage = 25;
    private float lastAttackTime;

    [Header("Player & Animator")]
    public Transform player1;
    public Transform player2;
    public Transform player3;
    public Animator animator;

    [Header("Ph�t hi?n Player")]
    public float detectRange = 6f;
    private bool hasDetectedPlayer = false;

    [Header("Teleport")]
    public float teleportCooldown = 5f;
    private float lastTeleportTime;

    [Header("�m thanh")]
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

    [Header("HP & Revive")]
    public int maxHealth = 300;
    private int currentHealth;
    private bool isDead = false;
    public int extraLives = 1;
    public float reviveDelay = 1.5f;
    private bool isReviving = false;

    [Header("Fire Breath")]
    public ParticleSystem fireBreath;
    public Transform firePoint;         // Child c?a mouthTransform
    public Transform mouthTransform;    // G?n ��ng v? tr� mi?ng
    public float fireCooldown = 7f;
    public float fireDuration = 2f;
    public float fireDamagePerSecond = 15f;
    public float fireRadius = 1.5f;
    private float lastFireTime;
    private bool isFiring;

    private Rigidbody2D rb;
    private Vector3 originalScale;
    private bool isAttacking = false;

    private float patrolTimer = 0f;
    private int patrolDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        originalScale = transform.localScale;
        currentHealth = maxHealth;

        // T? t?m Animator n?u ch�a g�n
        if (animator == null)
            animator = GetComponent<Animator>();

        // T? t?m firePoint n?u ch�a g�n
        if (firePoint == null && mouthTransform != null)
        {
            Transform fp = mouthTransform.Find("FirePoint");
            if (fp != null)
                firePoint = fp;
        }

        // N?u v?n ch�a c� firePoint th? b�o l?i r? r�ng
        if (firePoint == null)
            Debug.LogError("Boss4Controller: FirePoint ch�a ��?c g�n v� kh�ng t?m th?y trong MouthTransform!");

        // �?m b?o firePoint b�m v�o mouthTransform
        if (mouthTransform != null && firePoint != null)
            firePoint.SetParent(mouthTransform, false);

        if (fireBreath != null)
            fireBreath.Stop();

        lastFireTime = -fireCooldown;
        lastSummonTime = -summonCooldown;
        lastTeleportTime = -teleportCooldown;
    }

    void Update()
    {
        if (isDead) return;

        Transform target = GetNearestPlayer();
        if (target == null)
        {
            IdlePatrol();
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (!hasDetectedPlayer && distance <= detectRange)
            hasDetectedPlayer = true;

        if (!hasDetectedPlayer)
        {
            IdlePatrol();
            return;
        }

        if (Time.time - lastTeleportTime >= teleportCooldown)
            TeleportToPlayer(target);

        if (Time.time - lastSummonTime >= summonCooldown)
        {
            StartCoroutine(UseSkill());
            lastSummonTime = Time.time;
        }

        if (!isLowHealthSummon && currentHealth <= maxHealth * 0.3f)
        {
            StartCoroutine(SummonSkeletons(5));
            isLowHealthSummon = true;
        }

        if (distance <= attackRange)
        {
            rb.velocity = Vector2.zero;

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack(target);
                lastAttackTime = Time.time;
            }

            animator.SetBool("isMoving", false);
        }
        else
        {
            MoveToPlayer(distance, target);
        }

        if (!isFiring && Time.time - lastFireTime >= fireCooldown)
        {
            StartCoroutine(FireOnce(target));
        }
    }

    Transform GetNearestPlayer()
    {
        Transform nearest = null;
        float minDist = float.MaxValue;
        Transform[] players = { player1, player2, player3 };

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

    void IdlePatrol()
    {
        if (isDead) return;

        patrolTimer += Time.deltaTime;
        if (patrolTimer >= 2f)
        {
            patrolTimer = 0f;
            patrolDirection *= -1;
        }

        Vector2 patrolVelocity = new Vector2(patrolDirection * idlePatrolSpeed, 0f);
        rb.velocity = patrolVelocity;
        FlipBoss(patrolVelocity);

        animator.SetBool("isMoving", true);
    }

    void MoveToPlayer(float distance, Transform target)
    {
        if (isAttacking || isDead) return;

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
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void TeleportToPlayer(Transform target)
    {
        if (isDead) return;

        lastTeleportTime = Time.time;
        transform.position = target.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);

        Vector2 dir = (target.position - transform.position).normalized;
        FlipBoss(dir);

        // Xoay mi?ng h�?ng v? player
        if (mouthTransform != null)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            mouthTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (teleportClip != null && audioSource != null)
            audioSource.PlayOneShot(teleportClip);
    }

    void Attack(Transform target)
    {
        if (isDead || isReviving) return;

        animator.SetTrigger("Attack");
        isAttacking = true;
        Invoke(nameof(EndAttack), 0.5f);

        if (attackClip != null) audioSource.PlayOneShot(attackClip);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
    }

    void EndAttack() => isAttacking = false;

    IEnumerator FireOnce(Transform target)
    {
        if (isDead || firePoint == null) yield break;

        isFiring = true;
        lastFireTime = Time.time;

        // Xoay mi?ng h�?ng v? player tr�?c khi b?n
        if (mouthTransform != null && target != null)
        {
            Vector2 dir = (target.position - mouthTransform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            mouthTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

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
            if (isDead) break;

            elapsed += Time.deltaTime;
            damageTimer += Time.deltaTime;

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
        if (isDead) yield break;

        if (skillClip != null) audioSource.PlayOneShot(skillClip);

        animator.SetTrigger("Skill");
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(SummonSkeletons(summonCount));
    }

    IEnumerator SummonSkeletons(int count)
    {
        if (skeletonPrefab == null || isDead) yield break;

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
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.velocity = Vector2.zero;

        animator.SetBool("isMoving", false);
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Skill");
        animator.SetTrigger("Die");

        StartCoroutine(DelayedDestroyAfterAnimation("Demon die"));
    }

    IEnumerator DelayedDestroyAfterAnimation(string animStateName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName(animStateName))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        yield return new WaitForSeconds(stateInfo.length + 0.1f);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.magenta;
        if (firePoint != null)
            Gizmos.DrawWireSphere(firePoint.position, fireRadius);
    }
}
