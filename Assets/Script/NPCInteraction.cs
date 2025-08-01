using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject textE;            // Chữ E khi gần NPC
    public GameObject panelButtons;     // Panel chứa các Button
    public GameObject imageA;           // Hình ảnh được bật tắt
    public GameObject panelMua;         // Panel hiện khi nhấn nút Mua

    private bool isPlayerNearby = false;

    void Start()
    {
        textE.SetActive(false);
        panelButtons.SetActive(false);
        imageA.SetActive(false);
        if (panelMua != null)
            panelMua.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            panelButtons.SetActive(true);
            textE.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = true;
            textE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = false;
            textE.SetActive(false);
            panelButtons.SetActive(false);
            imageA.SetActive(false);
            if (panelMua != null)
                panelMua.SetActive(false);
        }
    }

    // 👉 Gán vào Button "Hiện hình ảnh"
    public void ShowImage()
    {
        imageA.SetActive(true);
    }

    // 👉 Gán vào Button "Tắt hình ảnh"
    public void HideImage()
    {
        imageA.SetActive(false);
    }

    // 👉 Gán vào Button "Đóng panel chính"
    public void HidePanel()
    {
        panelButtons.SetActive(false);
    }

    // 👉 Gán vào Button "Mua" để hiện panel mua
    public void ShowMuaPanel()
    {
        if (panelMua != null)
            panelMua.SetActive(true);
        panelButtons.SetActive(false);
    }

    // 👉 Gán vào Button "Không cần" để tắt tất cả
    public void HideAllPanels()
    {
        panelButtons.SetActive(false);
        if (panelMua != null)
            panelMua.SetActive(false);
        imageA.SetActive(false);
    }
    public void HidePanel1()
    {
        panelMua.SetActive(false);
    }
}
