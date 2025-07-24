using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    public float currentXP = 0f;
    public float maxXP = 100f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại qua các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddXP(float amount)
    {
        currentXP += amount;

        // Nếu đủ XP để lên cấp
        while (currentXP >= maxXP)
        {
            currentXP -= maxXP; // Giữ lại phần dư XP
            LevelUp();
        }

        UILevelUp.Instance?.UpdateXPBar(currentXP / maxXP); // Cập nhật thanh XP
    }

    private void LevelUp()
    {
        Debug.Log("Level Up!");
        UILevelUp.Instance?.ShowLevelUpUI();
    }
}
