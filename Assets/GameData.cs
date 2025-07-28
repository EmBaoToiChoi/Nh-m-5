using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public bool isFinalImageShown = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không bị xoá khi load scene mới
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
