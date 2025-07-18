using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource source;

    public float shootDelay = 0.15f;
    private float nextShootTime;

    void Update()
    {
        RotateGun();
        Shoot();
    }

    void RotateGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Xoay súng theo chuột
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // ❌ KHÔNG flipY, KHÔNG lật scale
    }






    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextShootTime)
        {
            source.PlayOneShot(shootSound);
            Instantiate(bulletPrefab, firePos.position, firePos.rotation);
            nextShootTime = Time.time + shootDelay;
        }
    }
}
