using UnityEngine;

public class HideKeyImageOnSceneStart : MonoBehaviour
{
    void Start()
    {
        // Mỗi lần vào scene mới thì image key tắt đi
        gameObject.SetActive(false);
    }
}
