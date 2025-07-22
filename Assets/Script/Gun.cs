using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource source;
    private float rorateOffset = 180f;

    public float shootDelay = 0.15f;
    private float nextShootTime;

    void Update()
    {
        RotateGun();
        Shoot();
    }

    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rorateOffset);
        if (angle < -90 || angle > 90)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShootTime)
        {
            source.PlayOneShot(shootSound);
            Instantiate(bulletPrefab, firePos.position, firePos.rotation);
            nextShootTime = Time.time + shootDelay;
        }
    }
}
