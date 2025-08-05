using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{
    public GameObject panelToClose;
    public Button damageButton;
    public Button healthButton;

    private bool hasUpgraded = false;

    public void ShowPanel()
    {
        panelToClose.SetActive(true);
        Time.timeScale = 0f;
        hasUpgraded = false;

        damageButton.interactable = true;
        healthButton.interactable = true;
    }

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

        damageButton.interactable = false;
        healthButton.interactable = false;

        // Dùng Coroutine để đợi theo thời gian thực
        StartCoroutine(HidePanelAfterDelay());
    }

    IEnumerator HidePanelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f); // Đợi 0.5s bất chấp game đang pause
        Time.timeScale = 1f;
        panelToClose.SetActive(false);
    }
}
