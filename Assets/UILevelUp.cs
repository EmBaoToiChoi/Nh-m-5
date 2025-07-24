using UnityEngine;
using UnityEngine.UI;

public class UILevelUp : MonoBehaviour
{
    public static UILevelUp Instance;

    [Header("XP Bar")]
    public Image xpBarFill; // Thanh ảnh XP (Image fillAmount)

    [Header("UI Level Up")]
    public GameObject levelUpImage; // Ảnh thông báo "Level Up"

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateXPBar(float fillPercent)
    {
        if (xpBarFill != null)
            xpBarFill.fillAmount = fillPercent; // 0 đến 1
    }

    public void ShowLevelUpUI()
    {
        if (levelUpImage != null)
            levelUpImage.SetActive(true);
    }
}
