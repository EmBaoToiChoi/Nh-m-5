using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Máu")]
    public float healAmount = 5f;

    [Header("Âm thanh")]
    public AudioClip pickupSound;   // Gắn file âm thanh trong Inspector
    private AudioSource audioSource;

    [Header("Thông báo")]
    [SerializeField] private PickupMessageManager messageManager;

    private bool isCollected = false;

    private void Start()
    {
        // Thêm AudioSource tự động nếu chưa có
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return; // tránh nhặt 2 lần

        if (other.CompareTag("Player1"))
        {
            isCollected = true;

            if (ThanhMauPl_1.Instance != null)
            {
                ThanhMauPl_1.Instance.Heal(healAmount);
                Debug.Log("✅ Player nhặt máu và hồi " + healAmount);

                if (messageManager != null)
                {
                    messageManager.ShowHealthMessageStackable(healAmount);
                }
            }
            else
            {
                Debug.LogWarning("❌ Không tìm thấy ThanhMauPl_1.Instance");
            }

            // Phát âm thanh
            audioSource.PlayOneShot(pickupSound);

            // Ẩn object để không nhặt thêm lần nữa
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Xóa sau khi âm thanh chạy xong
            Destroy(gameObject, pickupSound.length);
        }
    }
}
