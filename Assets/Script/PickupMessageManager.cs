using System.Collections;
using TMPro;
using UnityEngine;

public class PickupMessageManager : MonoBehaviour
{
    [Header("Thời gian hiển thị mỗi loại (giây)")]
    public float goldDuration = 0.5f;
    public float healthDuration = 0.5f;
    public float expDuration = 0.5f;

    [Header("Vàng")]
    public GameObject goldMessageUI;
    public TMP_Text goldText;
    private int goldCount = 0;
    private Coroutine goldCoroutine;

    [Header("Máu")]
    public GameObject healthMessageUI;
    public TMP_Text healthText;
    private float healthAmount = 0;
    private Coroutine healthCoroutine;

    [Header("Kinh nghiệm")]
    public GameObject xpMessageUI;
    public TMP_Text xpText;
    private float xpAmount = 0;
    private Coroutine xpCoroutine;

    // ========== VÀNG ==========
    public void ShowGoldMessageStackable()
    {
        goldCount++;

        if (goldText != null)
            goldText.text = $"Bạn đã nhặt được {goldCount} vàng";

        if (goldMessageUI != null)
            goldMessageUI.SetActive(true);

        if (goldCoroutine != null)
            StopCoroutine(goldCoroutine);

        goldCoroutine = StartCoroutine(HideGoldMessageAfterDelay(goldDuration));
    }

    private IEnumerator HideGoldMessageAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (goldMessageUI != null)
            goldMessageUI.SetActive(false);

        goldCount = 0;
        goldCoroutine = null;
    }

    // ========== MÁU ==========
    public void ShowHealthMessageStackable(float heal)
    {
        healthAmount += heal;

        if (healthText != null)
            healthText.text = $"Bạn đã hồi {healthAmount} máu";

        if (healthMessageUI != null)
            healthMessageUI.SetActive(true);

        if (healthCoroutine != null)
            StopCoroutine(healthCoroutine);

        healthCoroutine = StartCoroutine(HideHealthMessageAfterDelay(healthDuration));
    }

    private IEnumerator HideHealthMessageAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (healthMessageUI != null)
            healthMessageUI.SetActive(false);

        healthAmount = 0;
        healthCoroutine = null;
    }

    // ========== KINH NGHIỆM ==========
    public void ShowXPMessageStackable(float xp)
    {
        xpAmount += xp;

        if (xpText != null)
            xpText.text = $"Bạn đã nhận {xpAmount} kinh nghiệm";

        if (xpMessageUI != null)
            xpMessageUI.SetActive(true);

        if (xpCoroutine != null)
            StopCoroutine(xpCoroutine);

        xpCoroutine = StartCoroutine(HideXPMessageAfterDelay(expDuration));
    }

    private IEnumerator HideXPMessageAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (xpMessageUI != null)
            xpMessageUI.SetActive(false);

        xpAmount = 0;
        xpCoroutine = null;
    }
}
