using UnityEngine;

public class XPItem : MonoBehaviour
{
    [Header("Âm thanh")]
    public AudioClip pickupSound;   // Gắn file âm thanh ở Inspector
    private AudioSource audioSource;

    [Header("Kinh nghiệm")]
    public float xpAmount = 5f;

    [Header("Thông báo")]
    [SerializeField] private PickupMessageManager messageManager;

    private bool isCollected = false;

    private void Start()
    {
        // Tự động gắn AudioSource nếu chưa có
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return; // tránh nhặt 2 lần

        if (other.CompareTag("Player1"))
        {
            isCollected = true;

            // Cộng XP
            XPManager.Instance.AddXP(xpAmount);
            Debug.Log("✅ Player nhận " + xpAmount + " kinh nghiệm");

            // Hiện thông báo
            if (messageManager != null)
            {
                messageManager.ShowXPMessageStackable(xpAmount);
            }

            // Phát âm thanh
            audioSource.PlayOneShot(pickupSound);

            // Ẩn object ngay (không bị nhặt lại)
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Xoá object sau khi âm thanh chạy xong
            Destroy(gameObject, pickupSound.length);
        }
    }
}
