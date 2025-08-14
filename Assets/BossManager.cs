using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public GameObject boss1;
    public GameObject boss2;
    public string winSceneName = "Win"; // tên scene win

    void Update()
    {
        // Kiểm tra nếu cả 2 boss đều bị destroy
        if (boss1 == null && boss2 == null)
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}
