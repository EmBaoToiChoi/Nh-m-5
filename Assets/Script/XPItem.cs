using UnityEngine;

public class XPItem : MonoBehaviour
{
    [Header("Âm thanh")]
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;

    [Header("Kinh nghiệm")]
    [SerializeField] private float xpAmount = 5f;

    [Header("Thông báo")]
    [SerializeField] private PickupMessageManager messageManager;

    private bool isCollected = false;

    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        // Lấy component sẵn có
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Nếu chưa có AudioSource thì thêm
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (!other.CompareTag("Player1")) return;

        isCollected = true;

        // ===== CỘNG XP =====
        if (XPManager.Instance != null)
        {
            XPManager.Instance.AddXP(xpAmount);
            Debug.Log("✅ Player nhận " + xpAmount + " XP");
        }
        else
        {
            Debug.LogError("❌ Không tìm thấy XPManager!");
        }

        // ===== HIỆN THÔNG BÁO =====
        if (messageManager != null)
        {
            messageManager.ShowXPMessageStackable(xpAmount);
        }

        // ===== PHÁT ÂM THANH =====
        float delay = 0f;

        if (pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
            delay = pickupSound.length;
        }
        else
        {
            Debug.LogWarning("⚠️ Chưa gán pickupSound!");
        }

        // ===== ẨN OBJECT =====
        if (sr != null) sr.enabled = false;
        if (col != null) col.enabled = false;

        // ===== XOÁ OBJECT =====
        Destroy(gameObject, delay);
    }
}