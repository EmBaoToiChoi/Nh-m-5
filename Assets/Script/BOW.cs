using UnityEngine;
using TMPro;

public class BOW : MonoBehaviour
{
    [Header("Bắn")]
    [SerializeField] private Transform firePos1;
    [SerializeField] private GameObject BowPrefab;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource source1;

    public float shootDelay = 0.25f;
    private float nextShootTime;

    [Header("Đạn")]
    public int currentAmmo;     // Không khởi tạo sẵn = 50 nữa
    public int maxAmmo = 50;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    private void Start()
    {
        // 🔹 Load số tên từ GameData2
        currentAmmo = GameData2.Instance.bowAmmo;

        // Nếu lần đầu (chưa có dữ liệu) thì set mặc định
        if (currentAmmo <= 0)
            currentAmmo = maxAmmo;

        UpdateAmmoUI();
    }

    private void Update()
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
            if (currentAmmo > 0)
            {
                source1.PlayOneShot(shootSound);
                Instantiate(BowPrefab, firePos1.position, firePos1.rotation);
                currentAmmo--;

                // 🔹 Lưu số tên còn lại vào GameData2 để giữ qua scene
                GameData2.Instance.bowAmmo = currentAmmo;

                UpdateAmmoUI();
                nextShootTime = Time.time + shootDelay;
            }
            else
            {
                Debug.Log("❌ Hết tên!");
            }
        }
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / ";
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);

        // 🔹 Cập nhật lại GameData2
        GameData2.Instance.bowAmmo = currentAmmo;

        UpdateAmmoUI();
    }
}
