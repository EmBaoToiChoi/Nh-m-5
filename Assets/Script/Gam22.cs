using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gam22 : MonoBehaviour
{
    public string sceneToLoad = "Gam2,2";
    public GameObject pressEText;       // Text: "Nhấn E để vào"
    public GameObject warningText;      // Text: "Cần có chìa khóa!"
    public GameObject keyImageUI;       // Hình ảnh chìa khóa (UI)

    private bool isPlayerNearby = false;

    void Start()
    {
        pressEText?.SetActive(false);
        warningText?.SetActive(false);

        // Tắt image nếu key chưa được nhặt
        if (PlayerPrefs.GetInt("KeyCollected", 0) == 1)
        {
            keyImageUI.SetActive(true);
        }
        else
        {
            keyImageUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby)
        {
            if (keyImageUI.activeSelf && Input.GetKeyDown(KeyCode.E))
            {
                // XÓA TRẠNG THÁI ĐÃ CÓ KEY TRƯỚC KHI LOAD SCENE
                PlayerPrefs.DeleteKey("KeyCollected");
                PlayerPrefs.Save();

                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = true;

            if (PlayerPrefs.GetInt("KeyCollected", 0) == 1)
            {
                pressEText?.SetActive(true);
                warningText?.SetActive(false);
            }
            else
            {
                pressEText?.SetActive(false);
                warningText?.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = false;
            pressEText?.SetActive(false);
            warningText?.SetActive(false);
        }
    }
}
