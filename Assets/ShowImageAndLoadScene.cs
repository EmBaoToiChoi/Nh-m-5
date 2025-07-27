using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowImageAndLoadScene : MonoBehaviour
{
 
    public Button buttonA;       
         



    void Start()
    {

        // Gán sự kiện cho buttonB
        buttonA.onClick.AddListener(LoadNextScene);
    }


    void LoadNextScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
