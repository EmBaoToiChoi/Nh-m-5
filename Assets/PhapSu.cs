using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhapSu : MonoBehaviour
{
    private enum State { Chase, Attack, Flee, Return }
    private State currentState = State.Chase;
    [SerializeField] private float fleeDuration = 1.5f;
    [SerializeField] private float returnDelay = 0.5f;


    private float fleeTimer = 0f;

    private Vector3 fleeDirection;


    [SerializeField] private float fleeHealthThreshold = 10f;
    public Transform enermy, player;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject xpPrefab;

    [SerializeField] private GameObject healthItemPrefab;


    [Header("Animation")]
    [SerializeField] private Animator nie;

    [Header("UI Máu")]
    [SerializeField] private GameObject healthBarPrefab; // Prefab thanh máu
    [SerializeField] private Image healthFill;           // Gán trong Inspector

    private GameObject healthBarUI;
    private Transform mainCam;

    private bool isChasing = false;
    private float speed = 2f;
    private float PVipHien = 10f;

    private float maxHealth = 80f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        // Spawn thanh máu
        healthBarUI = Instantiate(healthBarPrefab, enermy.position + Vector3.up * 1.5f, Quaternion.identity);
        healthBarUI.transform.SetParent(null);

        mainCam = Camera.main.transform;
    }

    void Update()
    {
        if (this == null || enermy == null || player == null) return;

        switch (currentState)
        {
            case State.Chase:
                float distance = Vector2.Distance(enermy.position, player.position);
                isChasing = distance < PVipHien;

                if (currentHealth <= fleeHealthThreshold)
                {
                    ChayKhoiPlayer(player.position);
                }
                else if (isChasing)
                {
                    dichuyentoiPlayer(player.position);
                }
                break;

            case State.Flee:
                fleeTimer -= Time.deltaTime;
                enermy.Translate(fleeDirection * speed * Time.deltaTime);

                if (fleeTimer <= 0f)
                {
                    StartCoroutine(DelayReturn());
                }
                break;

            case State.Return:
                dichuyentoiPlayer(player.position);
                break;
        }

        // Cập nhật thanh máu
        if (healthBarUI != null && enermy != null)
        {
            healthBarUI.transform.position = enermy.position + Vector3.up * 1.5f;
            healthBarUI.transform.rotation = Quaternion.identity;
        }
    }
    IEnumerator DelayReturn()
    {
        yield return new WaitForSeconds(returnDelay);
        currentState = State.Return;
    }



    void StartFlee()
    {
        currentState = State.Flee;
        fleeTimer = fleeDuration;
        fleeDirection = (enermy.position - player.position).normalized;

        if (fleeDirection.x > 0)
            enermy.localScale = new Vector3(3,3,3);
        else if (fleeDirection.x < 0)
            enermy.localScale = new Vector3(-3,3,3);
    }




    void dichuyentoiPlayer(Vector3 target)
    {
        Vector3 direction = (target - enermy.position).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        // Lật hướng enemy
        if (direction.x > 0)
            enermy.localScale = new Vector3(3,3,3);
        if (direction.x < 0)
            enermy.localScale = new Vector3(-3,3,3);
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthFill != null)
        {
            DamageTextManager.Instance.ShowDamage(enermy.position, damage);
            healthFill.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            DamageTextManager.Instance.ShowDamage(enermy.position, damage);
            SpawnDrops();

            Destroy(healthBarUI);
            Destroy(gameObject);

            enabled = false; // Dừng Update() để tránh lỗi
        }
    }


    void ChayKhoiPlayer(Vector3 target)
    {
        Vector3 direction = (enermy.position - target).normalized; // Ngược hướng với player
        enermy.Translate(direction * speed * Time.deltaTime);

        // Lật hướng enemy
        if (direction.x > 0)
            enermy.localScale = new Vector3(3,3,3);
        else if (direction.x < 0)
            enermy.localScale = new Vector3(-3,3,3);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", true);

            if (currentState == State.Chase || currentState == State.Return)
            {
                StartFlee();
            }
        }
    
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", true);

            if (currentState == State.Chase || currentState == State.Return)
            {
                StartFlee();
            }
        }
    
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            nie.SetBool("danh", false);
        }
    }
    void SpawnDrops()
    {
        Vector3 basePosition = enermy.position;

        // Spawn coin slightly to the left
        if (coinPrefab != null)
            Instantiate(coinPrefab, basePosition + new Vector3(-0.3f, 0, 0), Quaternion.identity);

        // Spawn XP slightly to the center
        if (xpPrefab != null)
            Instantiate(xpPrefab, basePosition + new Vector3(0f, 0, 0), Quaternion.identity);

        // Spawn health item slightly to the right
        if (healthItemPrefab != null)
            Instantiate(healthItemPrefab, basePosition + new Vector3(0.3f, 0, 0), Quaternion.identity);
    }


}
