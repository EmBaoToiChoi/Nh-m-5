using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime); // Bay lên
    }
}
