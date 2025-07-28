using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Transform target;        // Enemy
    public Vector3 offset = new Vector3(0, 1.5f, 0); // V? trí trên ð?u enemy

    public void SetHealth(int current, int max)
    {
        if (slider != null)
        {
            slider.value = (float)current / max;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

}
