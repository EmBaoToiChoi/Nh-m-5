using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPItem : MonoBehaviour
{
    public float xpAmount = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            XPManager.Instance.AddXP(xpAmount);
            Destroy(gameObject);
        }
    }
}
