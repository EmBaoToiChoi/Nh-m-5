using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToPreviousScene : MonoBehaviour
{
    public void RetryGame()
    {
        // Lấy lại scene vừa chơi
        int previousScene = PlayerPrefs.GetInt("PreviousScene", 0);
        SceneManager.LoadScene(previousScene);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Thoát game!");
    }
}
