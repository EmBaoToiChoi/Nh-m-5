using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOW : MonoBehaviour
{
    [SerializeField] private Transform firePos1;
    [SerializeField] private GameObject BowPrefab;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource source1;

    public float shootDelay = 0.15f;
    private float nextShootTime;

    void Update()
    {
        RotateBow();
        Shoot1();
    }

    void RotateBow()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    
    
    }

    void Shoot1()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShootTime)
        {
            source1.PlayOneShot(shootSound);
            Instantiate(BowPrefab, firePos1.position, firePos1.rotation);
            nextShootTime = Time.time + shootDelay;
        }
    
    }
}
