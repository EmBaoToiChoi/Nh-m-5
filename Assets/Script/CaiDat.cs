using UnityEngine;

public class CaiDat : MonoBehaviour
{
    public GameObject UI; 

    public void ShowPanel()
    {
        UI.SetActive(true);
        Time.timeScale = 0f; // ⏸ Dừng game khi mở panel
    }

    public void HidePanel()
    {
        UI.SetActive(false);
        Time.timeScale = 1f; // ▶️ Tiếp tục game khi đóng panel
    }
}
