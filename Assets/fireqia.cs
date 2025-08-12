using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireqia : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            Destroy(this.gameObject);
        }
    }
}
