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

    [Header("Player Follow")]
    public Transform[] possiblePlayers; // Gán 3 player ở đây
    private Transform currentPlayer;
    public Vector3 offset = new Vector3(0f, 0f, 0); // Vị trí lệch so với player

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
        // Tìm player đang active
        if (currentPlayer == null || !currentPlayer.gameObject.activeInHierarchy)
        {
            foreach (Transform p in possiblePlayers)
            {
                if (p != null && p.gameObject.activeInHierarchy)
                {
                    currentPlayer = p;
                    break;
                }
            }
        }

        if (currentPlayer == null) return;

        FollowPlayer();
        RotateGun();
        Shoot();
        HandleReload();
        UpdateReloadTextPosition();
    }

    void FollowPlayer()
    {
        transform.position = currentPlayer.position + offset;
    }

    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return;

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rorateOffset);
        transform.localScale = (angle < -90 || angle > 90) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
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

            reloadText.gameObject.SetActive(true);
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

    void UpdateReloadTextPosition()
    {
        if (reloadText.gameObject.activeSelf && currentPlayer != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentPlayer.position + offsetUI);
            reloadText.transform.position = screenPos;
        }
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
