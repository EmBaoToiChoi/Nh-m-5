using UnityEngine;
using System.Collections;

public class Boss2Controller : MonoBehaviour
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

    [Header("Phun l?a")]
    public ParticleSystem fireBreath;     // Particle System g?n trong firePoint
    public Transform firePoint;
    public float fireCooldown = 5f;
    public float fireDuration = 2f;
    private float lastFireTime;
    private bool isFiring;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (fireBreath != null)
            fireBreath.Stop(); // T?t lúc ð?u
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

            // Ki?m tra cooldown ð? phun l?a
            if (!isFiring && Time.time - lastFireTime >= fireCooldown)
            {
                StartCoroutine(FireOnce());
            }
        }
        else
        {
            MoveToPlayer();
            animator.SetBool("isMoving", true);

            // T?t l?a n?u ðang phun khi xa player
            if (fireBreath != null && fireBreath.isPlaying)
            {
                fireBreath.Stop();
            }

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
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            if (firePoint != null)
                firePoint.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            if (firePoint != null)
                firePoint.localRotation = Quaternion.Euler(0, 180, 0);
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
            Debug.Log("?? Boss2 phun l?a!");
        }

        yield return new WaitForSeconds(fireDuration);

        if (fireBreath != null)
        {
            fireBreath.Stop();
            Debug.Log("?? Boss2 ng?ng phun l?a.");
        }

        isFiring = false;
    }
}
