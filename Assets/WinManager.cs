using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [Header("Boss cần tiêu diệt")]
    public GameObject boss1;
    public GameObject boss2;

    [Header("Tên scene Win")]
    public string winSceneName = "Win";

    void Update()
    {
        // Kiểm tra nếu cả 2 boss đều đã bị Destroy
        if (boss1 == null && boss2 == null)
        {
            LoadWinScene();
        }
    }

    void LoadWinScene()
    {
        SceneManager.LoadScene(winSceneName);
    }
}
