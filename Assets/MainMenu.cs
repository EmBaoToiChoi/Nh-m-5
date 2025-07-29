using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   

    public string sceneToLoad = "Menu"; // Game chính
    public string loadingScene2 = "Load2"; // Scene loading

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("SaveData"); // Xóa toàn bộ dữ liệu cũ
        Loading2.Next_Scene = sceneToLoad; // Gán scene cần load sau loading
        SceneManager.LoadScene(sceneToLoad); // Tới scene loading
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            Loading2.Next_Scene = sceneToLoad;
            SceneManager.LoadScene(loadingScene2);
        }
    }

}
