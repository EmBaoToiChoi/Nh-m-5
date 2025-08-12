using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball2 : MonoBehaviour
{
    public Animator ani;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enermy"))
        {
            ani.SetTrigger("No");
            Destroy(this.gameObject, 0.1f);
        }
    }
}
