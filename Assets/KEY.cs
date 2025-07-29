using UnityEngine;

public class KEY : MonoBehaviour
{
    public static KEY Instance;
    private bool isCollected = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ object này
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (GameData.Instance != null && GameData.Instance.isKeyCollected)
        {
            gameObject.SetActive(false); // Đã nhặt → ẩn chìa khóa
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player1"))
        {
            isCollected = true;

            if (GameData.Instance != null)
                GameData.Instance.isKeyCollected = true;

            KEY_UI.Instance?.ShowKeyImage(); // Gọi UI hiện ảnh

            gameObject.SetActive(false); // ẩn key
        }
    }
}
