using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePortal : MonoBehaviour
{
    public string sceneToLoad = "Gam1,2"; // Gán tên scene cần load trong Inspector
    public Image finalImage;                   // Image là "chìa khoá"
    public GameObject warningText;             // Text cảnh báo nếu chưa có image

    private bool canLoad = false;

    void Start()
    {
        if (warningText != null)
            warningText.SetActive(false); // Ẩn cảnh báo lúc đầu
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu Player chạm vào cổng (Gam1 hoặc Gam2)
        if (other.CompareTag("Player1"))
        {
            if (GameData.Instance != null && GameData.Instance.isFinalImageShown)
            {
                // Nếu đã có image key -> Load scene
                if (finalImage != null)
                    finalImage.gameObject.SetActive(false); // Ẩn image nếu cần

                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                // Nếu chưa có key -> Hiện cảnh báo
                if (warningText != null)
                    warningText.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Khi Player rời khỏi cổng, ẩn cảnh báo
        if (other.CompareTag("Player1") && warningText != null)
        {
            warningText.SetActive(false);
        }
    }
}
