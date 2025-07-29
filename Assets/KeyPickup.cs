using UnityEngine;
using UnityEngine.UI;

public class KeyPickup : MonoBehaviour
{
    public GameObject uiImageToShow;
    private const string KeyCollectedKey = "KeyCollected"; // Khóa lưu

    void Start()
    {
        // Nếu đã nhặt key trước đó thì không cần hiện key nữa
        if (PlayerPrefs.GetInt(KeyCollectedKey, 0) == 1)
        {
            uiImageToShow.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            uiImageToShow.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            uiImageToShow.SetActive(true);
            PlayerPrefs.SetInt(KeyCollectedKey, 1); // Lưu trạng thái
            PlayerPrefs.Save(); // Ghi vào ổ cứng

            gameObject.SetActive(false); // Ẩn key
        }
    }
}
