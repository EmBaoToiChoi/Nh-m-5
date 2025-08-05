using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Slime_1_x : MonoBehaviour
{
    public Transform enermy;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject xpPrefab;

    [SerializeField] private GameObject healthItemPrefab;


    [Header("Animation")]
    [SerializeField] private Animator nie;

    [Header("UI Máu")]
    [SerializeField] private GameObject healthBarPrefab; // Prefab thanh máu
    [SerializeField] private Image healthFill;
    [SerializeField] private GameObject damageTextPrefab;

    private GameObject healthBarUI;
    private Transform mainCam;

    private bool isChasing = false;
    private float speed = 2f;
    private float PVipHien = 10f;

    private float maxHealth = 20f;
    private float currentHealth;
    public Transform player1;
    public Transform player2;

    public Transform player3;


private Transform targetPlayer;

    void Start()
    {
        currentHealth = maxHealth;

        // Tìm nhân vật đang active
        if (player1 != null && player1.gameObject.activeInHierarchy)
            targetPlayer = player1;
        else if (player2 != null && player2.gameObject.activeInHierarchy)
            targetPlayer = player2;
        else if (player3 != null && player3.gameObject.activeInHierarchy)
            targetPlayer = player3;
        else
            Debug.LogWarning("Không tìm thấy nhân vật nào đang hoạt động!");

        // Spawn thanh máu
        healthBarUI = Instantiate(healthBarPrefab, enermy.position + Vector3.up * 1.5f, Quaternion.identity);
        healthBarUI.transform.SetParent(null);

        mainCam = Camera.main.transform;
    }



    void Update()
    {
        if (targetPlayer != null)
        {
            float khoangCachPlayer = Vector2.Distance(enermy.position, targetPlayer.position);
            isChasing = khoangCachPlayer < PVipHien;

            if (isChasing)
            {
                dichuyentoiPlayer(targetPlayer.position);
            }
        }


        // Cập nhật vị trí thanh máu
        if (healthBarUI != null)
        {
            healthBarUI.transform.position = enermy.position + Vector3.up * 1.5f;
            healthBarUI.transform.rotation = Quaternion.identity; // Không cần quay nếu 2D
        }
    }

    void dichuyentoiPlayer(Vector3 target)
    {
        Vector3 direction = (target - enermy.position).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        // Lật hướng enemy
        if (direction.x > 0)
            enermy.localScale = new Vector3(5, 5, 5);
        if (direction.x < 0)
            enermy.localScale = new Vector3(-5, 5, 5);
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth > 0)
        {
            ShowDamageText(damage); // Gọi bình thường
        }
        else
        {
            SpawnDrops();
            ShowDamageText(damage);
            if (healthBarUI != null)
                Destroy(healthBarUI);
            Destroy(gameObject);
        }

        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        float damage = 0f;

        if (other.CompareTag("Hit"))
            damage = Random.Range(1f, 6f);
        else if (other.CompareTag("Bullet"))
            damage = Random.Range(10f, 16f);
        else if (other.CompareTag("Bow"))
            damage = Random.Range(5f, 11f);

        if (damage > 0)
            TakeDamage(damage + GlobalData.damageBonus); // cộng thêm bonus ở đây
    }


    public Canvas worldCanvas;
    void ShowDamageText(float damage)
    {
        if (damageTextPrefab == null || enermy == null || worldCanvas == null) return;

        // Tạo text ở canvas world
        GameObject dmgText = Instantiate(damageTextPrefab, worldCanvas.transform);

        // Cập nhật vị trí text (vị trí thế giới → vị trí UI)
        Vector3 worldPos = enermy.position + Vector3.up * 1.5f;
        dmgText.transform.position = worldPos;

        // Hiển thị nội dung số damage
        TextMeshProUGUI text = dmgText.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = damage.ToString("F0");

        // Hủy text sau 1s
        Destroy(dmgText, 1f);
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
