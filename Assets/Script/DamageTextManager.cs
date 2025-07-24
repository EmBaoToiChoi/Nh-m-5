using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance;

    public Canvas worldCanvas;
    public GameObject damageTextPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(Vector3 position, float damage)
    {
        GameObject dmgText = Instantiate(damageTextPrefab, worldCanvas.transform);
        dmgText.transform.position = position + Vector3.up * 1.5f;

        TextMeshProUGUI text = dmgText.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
            text.text = damage.ToString("F0");

        Destroy(dmgText, 1f);
    }
}
