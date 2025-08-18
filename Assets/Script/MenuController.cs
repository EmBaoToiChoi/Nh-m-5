using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Hàm này gọi khi nhấn nút "Chơi lại"
    public void ReplayGame()
    {
        // Lấy scene hiện tại và load lại
        SceneManager.LoadScene("Menu");
    }

    // Hàm này gọi khi nhấn nút "Thoát"
    public void QuitGame()
    {
        Debug.Log("Thoát game!"); // Chỉ để test trong editor
        Application.Quit();
    }
}
