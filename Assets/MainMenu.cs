using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneToLoad = "Gam1";
    public string loadingScene2 = "Load2";

    public void NewGame()
    {
        // Xóa file save
        string path = Application.persistentDataPath + "/save.json";
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            Debug.Log("Đã xóa dữ liệu cũ");
        }

        GameState.isContinue = false; // Rõ ràng là New Game
        Loading2.Next_Scene = sceneToLoad;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ContinueGame()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (System.IO.File.Exists(path))
        {
            GameState.isContinue = true; // đánh dấu là tiếp tục game
            // Loading2.Next_Scene = sceneToLoad;
            SceneManager.LoadScene(loadingScene2); // chuyển đến Load2
        }
        else
        {
            Debug.Log("Không có dữ liệu để tiếp tục.");
        }
    }

}
