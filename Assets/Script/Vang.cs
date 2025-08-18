using UnityEngine;
using TMPro;

public class Vang : MonoBehaviour
{
    public AudioClip pickupSound;   // Gắn file âm thanh ở Inspector
    private AudioSource audioSource;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int scoreValue = 1;

    [Header("Thông báo")]
    [SerializeField] private PickupMessageManager messageManager;

    void Start()
    {
        // Gắn AudioSource (nếu chưa có thì tự động thêm vào coin)
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        int savedScore = PlayerPrefs.GetInt("Player1", 0); // có mặc định = 0 để tránh lỗi
        UpdateScoreText(savedScore);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            int currentScore = PlayerPrefs.GetInt("Player1", 0);
            currentScore += scoreValue;
            PlayerPrefs.SetInt("Player1", currentScore);
            PlayerPrefs.Save();
            UpdateScoreText(currentScore);

            if (messageManager != null)
            {
                messageManager.ShowGoldMessageStackable(); // Cộng dồn vàng
            }

            // Phát âm thanh
            audioSource.PlayOneShot(pickupSound);

            // Ẩn coin để không bị nhặt lại
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Xóa coin sau khi âm thanh kết thúc
            Destroy(gameObject, pickupSound.length);
        }
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "X " + score;
        }
    }
}
