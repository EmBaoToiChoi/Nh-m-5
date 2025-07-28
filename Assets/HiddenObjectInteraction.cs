using UnityEngine;
using UnityEngine.UI;

public class HiddenObjectInteraction : MonoBehaviour
{
    public Image finalImage;

    private bool isActivated = false;

    void Start()
    {
        // Kiểm tra nếu image đã được bật từ trước (qua scene)
        if (GameData.Instance != null && GameData.Instance.isFinalImageShown)
        {
            finalImage.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            finalImage.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player1"))
        {
            isActivated = true;
            finalImage.gameObject.SetActive(true);
            gameObject.SetActive(false);

            if (GameData.Instance != null)
            {
                GameData.Instance.isFinalImageShown = true;
            }
        }
    }
}
