using UnityEngine;
using UnityEngine.UI;

public class ChestOpen : MonoBehaviour
{
    public Animator chestAnimator;         // Gán Animator cho chest
    public GameObject hiddenObject;        // Object sẽ bật sau khi chest mở
    public GameObject textPressE;          // UI Text hướng dẫn (ví dụ: "Nhấn E để mở rương")

    private bool isPlayerNearby = false;
    private bool isOpened = false;

    void Start()
    {
        if (textPressE != null)
            textPressE.SetActive(false); // Ẩn text khi bắt đầu
    }

    void Update()
    {
        if (isPlayerNearby && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            isOpened = true;
            chestAnimator.SetTrigger("moruong");     // Chạy animation mở rương
            hiddenObject.SetActive(true);            // Bật object ẩn
            if (textPressE != null)
                textPressE.SetActive(false);         // Ẩn hướng dẫn
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && other.CompareTag("Player1"))
        {
            isPlayerNearby = true;
            if (textPressE != null)
                textPressE.SetActive(true);          // Hiện hướng dẫn nhấn E
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = false;
            if (textPressE != null)
                textPressE.SetActive(false);         // Ẩn hướng dẫn khi rời xa
        }
    }
}
