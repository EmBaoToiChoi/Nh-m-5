using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MechaStoneEnemy : MonoBehaviour
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

        if (animator == null)
            animator = GetComponent<Animator>();

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
            MoveToPlayer(distance, target);
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
        rb.linearVelocity = patrolVelocity;
        FlipBoss(patrolVelocity);

        animator.SetBool("isMoving", true);
    }

    void MoveToPlayer(float distance, Transform target)
    {
        if (isAttacking || isDead) return;

        Vector2 direction = (target.position - transform.position).normalized;
        float speed = moveSpeed;
        if (distance < 4f) speed *= chaseSpeedMultiplier;

        rb.linearVelocity = direction * speed;
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

        if (teleportClip != null && audioSource != null)
            audioSource.PlayOneShot(teleportClip);
    }

    void Attack(Transform target)
    {
        if (isDead) return;

        animator.SetTrigger("Attack");
        isAttacking = true;
        Invoke(nameof(EndAttack), 0.5f);

        if (attackClip != null) audioSource.PlayOneShot(attackClip);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
    }

    void EndAttack() => isAttacking = false;

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
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;

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
    }
}
