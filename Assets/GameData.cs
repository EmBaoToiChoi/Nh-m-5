using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public bool isKeyCollected = false;

    private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // Phải có dòng này!
    }
    else
    {
        Destroy(gameObject);
    }
}

}
