using UnityEngine;
using UnityEngine.UI;

public class KEY_UI : MonoBehaviour
{
    public static KEY_UI Instance;

    [Header("Key Image")]
    public Image keyImage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (keyImage != null)
        {
            keyImage.gameObject.SetActive(GameData.Instance != null && GameData.Instance.isKeyCollected);
        }
    }

    public void ShowKeyImage()
    {
        if (keyImage != null)
            keyImage.gameObject.SetActive(true);
    }
}
