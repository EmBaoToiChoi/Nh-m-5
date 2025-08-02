using UnityEngine;
using System.IO;

public class GameData2 : MonoBehaviour
{
    public static GameData2 Instance;
    public int reserveAmmo = 25;
    public int currentAmmo = 25;
    public int maxAmmo = 25;

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
            reserveAmmo = this.reserveAmmo
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(ammoSavePath, json);
        Debug.Log("Đã lưu ammo vào " + ammoSavePath);
    }

    public void LoadAmmo()
    {
        if (File.Exists(ammoSavePath))
        {
            string json = File.ReadAllText(ammoSavePath);
            AmmoData data = JsonUtility.FromJson<AmmoData>(json);

            this.currentAmmo = data.currentAmmo;
            this.reserveAmmo = data.reserveAmmo;

            Debug.Log("Đã load lại ammo: " + currentAmmo + "/" + reserveAmmo);
        }
        else
        {
            Debug.Log("Không tìm thấy file ammo.json, dùng giá trị mặc định.");
        }
    }

    public void ResetAmmo() // Dùng khi chơi game mới
    {
        currentAmmo = maxAmmo;
        reserveAmmo = 25;
        SaveAmmo();
    }
}
