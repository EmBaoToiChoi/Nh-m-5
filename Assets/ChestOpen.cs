using UnityEngine;
using UnityEngine.UI;

public class ChestOpen : MonoBehaviour
{
    [Header("References")]
    public Animator chestAnimator;     // Animator gắn vào rương
    public GameObject hiddenObject;    // Vật phẩm ẩn bên trong
    public GameObject textPressE;      // UI Text hướng dẫn

    private bool isPlayerNearby = false;
    private bool isOpened = false;

    void Start()
    {
        if (textPressE != null)
            textPressE.SetActive(false); // Ẩn hướng dẫn khi bắt đầu

        if (hiddenObject != null)
            hiddenObject.SetActive(false); // Ẩn vật phẩm từ đầu
    }

    void Update()
    {
        if (isPlayerNearby && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;

        if (chestAnimator != null)
            chestAnimator.SetTrigger("moruong"); // Gửi trigger tới Animator

        if (hiddenObject != null)
            hiddenObject.SetActive(true); // Bật vật phẩm

        if (textPressE != null)
            textPressE.SetActive(false); // Ẩn hướng dẫn
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && other.CompareTag("Player1"))
        {
            isPlayerNearby = true;

            if (textPressE != null)
                textPressE.SetActive(true); // Hiện hướng dẫn
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = false;

            if (textPressE != null)
                textPressE.SetActive(false); // Ẩn hướng dẫn khi rời xa
        }
    }
}
