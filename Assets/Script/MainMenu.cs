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
            GameData2.Instance.ResetAmmo();

            Debug.Log("Đã xóa dữ liệu cũ");
        }

        GameState.isContinue = false; // Rõ ràng là New Game
        Loading2.Next_Scene = sceneToLoad;
        SceneManager.LoadScene(sceneToLoad);
         StoryState.ResetAll();
    }

    public void ContinueGame()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);

            GameState.isContinue = true;
            Loading2.Next_Scene = data.sceneName; // Load lại đúng scene

            SceneManager.LoadScene("Load2"); // Giả sử bạn có màn hình load trước
          
        }
        else
        {
            Debug.Log("Không có dữ liệu để tiếp tục.");
        }
    }





}
