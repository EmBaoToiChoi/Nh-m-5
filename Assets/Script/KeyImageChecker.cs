using UnityEngine;

public class KeyImageChecker : MonoBehaviour
{
    public GameObject keyImageUI; // Gán image key ở đây

    void Start()
    {
        if (PlayerPrefs.GetInt("KeyCollected", 0) == 1)
        {
            keyImageUI.SetActive(true); // bật hình
        }
        else
        { 
            keyImageUI.SetActive(true); // ẩn hình
        }
    }
}
