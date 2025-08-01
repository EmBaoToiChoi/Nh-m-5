using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject panelToClose;
    public Button damageButton;
    public Button healthButton;

    private bool hasUpgraded = false;

    public void UpgradeDamage()
    {
        if (hasUpgraded) return;

        GlobalData.damageBonus += 5f;
        Debug.Log("Đã nâng damage: +" + GlobalData.damageBonus);

        LockUpgrade();
    }

    public void UpgradeHealth()
    {
        if (hasUpgraded) return;

        GlobalData.healthBonus += 10f;
        Debug.Log("Đã nâng máu thêm: " + GlobalData.healthBonus);

        LockUpgrade();
    }

    void LockUpgrade()
    {
        hasUpgraded = true;

        // Tắt các nút để không ấn tiếp
        damageButton.interactable = false;
        healthButton.interactable = false;

        // Tắt panel sau 0.5 giây để cho cảm giác mượt
        Invoke(nameof(ClosePanel), 0.5f);
    }

    void ClosePanel()
    {
        panelToClose.SetActive(false);

        // Reset lại trạng thái lần sau dùng tiếp
        damageButton.interactable = true;
        healthButton.interactable = true;
        hasUpgraded = false;
    }
}
