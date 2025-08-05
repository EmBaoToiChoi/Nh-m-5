using UnityEngine;
using TMPro;

public class Vang : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int scoreValue = 1;

    [Header("Thông báo")]
    [SerializeField] private PickupMessageManager messageManager;

    void Start()
    {
        int savedScore = PlayerPrefs.GetInt("Player1");
        UpdateScoreText(savedScore);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            int currentScore = PlayerPrefs.GetInt("Player1");
            currentScore += scoreValue;
            PlayerPrefs.SetInt("Player1", currentScore);
            PlayerPrefs.Save();
            UpdateScoreText(currentScore);

            if (messageManager != null)
            {
                messageManager.ShowGoldMessageStackable(); // Cộng dồn vàng
            }

            Destroy(gameObject);
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
