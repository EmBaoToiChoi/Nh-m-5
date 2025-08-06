using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    [Header("Bắn")]
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource source;
    private float rorateOffset = 180f;

    public float shootDelay = 0.15f;
    private float nextShootTime;

    [Header("Đạn")]
    public int currentAmmo = 25;
    public int reserveAmmo = 25;
    public int maxAmmo = 25;

    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;
    public Vector3 offsetUI = new Vector3(0, 2, 0);

    [Header("Reload")]
    public AudioClip reloadSound;
    public AudioSource reloadSource;
    public int maxReserveAmmo = 100;


    void Start()
    {
        if (GameData2.Instance != null)
        {
            currentAmmo = GameData2.Instance.currentAmmo;
            reserveAmmo = GameData2.Instance.reserveAmmo;
            UpdateAmmoUI();
        }
    }

    void OnDisable()
    {
        if (GameData2.Instance != null)
        {
            GameData2.Instance.currentAmmo = currentAmmo;
            GameData2.Instance.reserveAmmo = reserveAmmo;
            GameData2.Instance.maxAmmo = maxAmmo;
        }
    }

    void Update()
    {
        RotateGun();
        Shoot();
        HandleReload();
    }

    void RotateGun()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorldPos - transform.position;
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }





    void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShootTime)
        {
            if (currentAmmo > 0)
            {
                source.PlayOneShot(shootSound);
                Instantiate(bulletPrefab, firePos.position, firePos.rotation);
                currentAmmo--;
                UpdateAmmoUI();
                nextShootTime = Time.time + shootDelay;

                if (GameData2.Instance != null)
                    GameData2.Instance.SaveAmmo();
            }
            else
            {
                Debug.Log("Hết đạn!");
                reloadText.gameObject.SetActive(true);
            }
        }
    }

    void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            int needed = maxAmmo - currentAmmo;
            int reloadAmount = Mathf.Min(needed, reserveAmmo);

            currentAmmo += reloadAmount;
            reserveAmmo -= reloadAmount;

            reloadText.gameObject.SetActive(false);
            reloadSource.PlayOneShot(reloadSound);
            Invoke("HideReloadText", 1f);
            UpdateAmmoUI();

            if (GameData2.Instance != null)
                GameData2.Instance.SaveAmmo();
        }
    }

    void HideReloadText()
    {
        reloadText.gameObject.SetActive(false);
    }

    public void UpdateAmmoUI()
    {
        ammoText.text = currentAmmo + " / " + reserveAmmo;
    }

    public void SaveAmmo()
    {
        PlayerPrefs.SetInt("currentAmmo", currentAmmo);
        PlayerPrefs.SetInt("reserveAmmo", reserveAmmo);

        if (GameData2.Instance != null)
        {
            GameData2.Instance.currentAmmo = currentAmmo;
            GameData2.Instance.reserveAmmo = reserveAmmo;
        }
    }

    public void LoadAmmo()
    {
        currentAmmo = PlayerPrefs.GetInt("currentAmmo", maxAmmo);
        reserveAmmo = PlayerPrefs.GetInt("reserveAmmo", 0);
    }

    public void AddReserveAmmo(int amount)
    {
        reserveAmmo = Mathf.Min(reserveAmmo + amount, 100);

        if (GameData2.Instance != null)
        {
            GameData2.Instance.reserveAmmo = reserveAmmo;
            GameData2.Instance.SaveAmmo();
        }

        UpdateAmmoUI();
    }
}
