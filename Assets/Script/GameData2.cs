using UnityEngine;
using System.IO;

public class GameData2 : MonoBehaviour
{
    [System.Serializable]
    public class AmmoData
    {
        public int currentAmmo;
        public int reserveAmmo;
        public int bowAmmo;
    }

    public static GameData2 Instance;

    // Gun
    public int reserveAmmo = 25;
    public int currentAmmo = 25;
    public int maxAmmo = 25;

    // Bow
    public int bowAmmo = 50;  // chỉ có 50 mũi tên

    private string ammoSavePath => Application.persistentDataPath + "/ammo.json";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAmmo(); // Load dữ liệu khi bắt đầu
    }

    public void SaveAmmo()
    {
        AmmoData data = new AmmoData()
        {
            currentAmmo = this.currentAmmo,
            reserveAmmo = this.reserveAmmo,
            bowAmmo = this.bowAmmo
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(ammoSavePath, json);
        Debug.Log("💾 Đã lưu dữ liệu ammo vào " + ammoSavePath);
    }

    public void LoadAmmo()
    {
        if (File.Exists(ammoSavePath))
        {
            string json = File.ReadAllText(ammoSavePath);
            AmmoData data = JsonUtility.FromJson<AmmoData>(json);

            this.currentAmmo = data.currentAmmo;
            this.reserveAmmo = data.reserveAmmo;
            this.bowAmmo = data.bowAmmo;

            Debug.Log($"🎯 Load thành công Gun: {currentAmmo}/{reserveAmmo}, Bow: {bowAmmo}");
        }
        else
        {
            Debug.Log("⚠ Không tìm thấy ammo.json, dùng mặc định.");
            currentAmmo = maxAmmo;
            reserveAmmo = 25;
            bowAmmo = 50;
        }
    }

    public void ResetAmmo() // Dùng khi chơi game mới
    {
        currentAmmo = maxAmmo;
        reserveAmmo = 25;
        bowAmmo = 50;
        SaveAmmo();
    }
}
