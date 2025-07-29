using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterSceneWithKey : MonoBehaviour
{
    public string sceneToLoad = "Gam1,2";
    public GameObject pressEText;           // Text: "Nhấn E để vào"
    public GameObject keyImageUI;           // Hình ảnh chìa khóa trên UI
    public GameObject warningText;          // Text: "Cần có chìa khóa!"
    
    private bool isPlayerNearby = false;
    private bool hasKey = false;

    void Start()
    {
        pressEText?.SetActive(false);
        keyImageUI?.SetActive(false);
        warningText?.SetActive(false);

        // Giả lập việc có chìa khóa (nếu bạn dùng hệ thống lưu thì thay dòng này)
        hasKey = PlayerPrefs.GetInt("HasKey_Gam1_1", 0) == 1;
    }

    void Update()
    {
        if (isPlayerNearby)
        {
            if (hasKey && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = true;

            if (hasKey)
            {
                pressEText?.SetActive(true);
                keyImageUI?.SetActive(true);
            }
            else
            {
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
            keyImageUI?.SetActive(false);
            warningText?.SetActive(false);
        }
    }
}
