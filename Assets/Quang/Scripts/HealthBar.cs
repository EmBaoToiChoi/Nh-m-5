using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public HealthSystem enemyHealth;

    void Start()
    {
        if (enemyHealth != null && slider != null)
        {
            slider.maxValue = enemyHealth.maxHealth;
            slider.value = enemyHealth.maxHealth;
        }
    }

    void Update()
    {
        if (enemyHealth != null && slider != null)
        {
            slider.value = Mathf.Clamp(enemyHealth.CurrentHealth, 0, enemyHealth.maxHealth);
        }
    }
}
