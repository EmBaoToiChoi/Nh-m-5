using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MuaVuKhi : MonoBehaviour
{
    public TMP_Text coinText;
    public GameObject imageGunUnlocked;
    public GameObject imageBowUnlocked;

    public Button btnMuaGun;
    public Button btnMuaBow;
    public Button btnMuaDan;
    public Button btnMuaMau;

    public GameObject imageBinhMauUnlocked;
    public Image binhMauImage;
    public TMP_Text binhMauText;

    private int soBinhMau = 0;
    private const int maxBinhMau = 5;

    public static bool daMuaSung = false;
    public static bool daMuaCung = false;

    public static object Instance { get; internal set; }

    private void Start()
    {
        soBinhMau = PlayerPrefs.GetInt("SoBinhMau", 0);

        CapNhatUI();
    }

    public void MuaGun()
    {
        int vang = PlayerPrefs.GetInt("Player1", 0);
        if (vang >= 100 && !daMuaSung)
        {
            vang -= 100;
            PlayerPrefs.SetInt("Player1", vang);
            PlayerPrefs.Save();

            daMuaSung = true;
            CapNhatUI();
        }
        else { Debug.Log("Không đủ tiền mua súng."); }
    }

    public void MuaBow()
    {
        int vang = PlayerPrefs.GetInt("Player1", 0);
        if (vang >= 50 && !daMuaCung)
        {
            vang -= 50;
            PlayerPrefs.SetInt("Player1", vang);
            PlayerPrefs.Save();

            daMuaCung = true;
            CapNhatUI();
        }
        else { Debug.Log("Không đủ tiền mua cung."); }
    }

    public void MuaDan()
    {
        int vang = PlayerPrefs.GetInt("Player1", 0);
        if (vang >= 50)
        {
            if (GameData2.Instance != null)
            {
                int reserve = GameData2.Instance.reserveAmmo;

                if (reserve >= 100)
                {
                    if (reserve > 0)
                    {
                        Debug.Log("Đã đạt giới hạn 100 viên. Chỉ được mua tiếp khi hết sạch đạn dự trữ.");
                        return;
                    }
                }

                int danThem = 25;
                if (reserve + danThem > 100)
                {
                    danThem = 100 - reserve;
                }

                GameData2.Instance.reserveAmmo += danThem;

                vang -= 50;
                PlayerPrefs.SetInt("Player1", vang);
                PlayerPrefs.Save();
                CapNhatUI();

                Gun gun = FindObjectOfType<Gun>();
                if (gun != null)
                {
                    gun.AddReserveAmmo(danThem);
                    gun.UpdateAmmoUI();
                }

                Debug.Log($"Đã mua {danThem} viên đạn.");
            }
            else
            {
                Debug.LogError("GameData2.Instance is null - chưa khởi tạo GameData2 trong scene!");
            }
        }
        else
        {
            Debug.Log("Không đủ tiền mua đạn");
        }
    }

    public void MuaMau()
    {
        int vang = PlayerPrefs.GetInt("Player1", 0);
        int soLuongHienTai = PlayerPrefs.GetInt("SoBinhMau", 0);

        if (vang >= 50 && soLuongHienTai < maxBinhMau)
        {
            vang -= 50;
            soLuongHienTai++;
            PlayerPrefs.SetInt("Player1", vang);
            PlayerPrefs.SetInt("SoBinhMau", soLuongHienTai);
            PlayerPrefs.Save();

            // Cập nhật hiển thị từ thanh máu
            ThanhMauPl_1.Instance?.CapNhatUIBinhMau();
            CapNhatUI();

            Debug.Log($"Đã mua bình máu. Tổng cộng: {soLuongHienTai}");
        }
        else
        {
            Debug.Log("Không đủ vàng hoặc đã tối đa 5 bình máu");
        }
    }







    private void CapNhatUI()
    {
        coinText.text = "X " + PlayerPrefs.GetInt("Player1", 0);

        imageGunUnlocked.SetActive(daMuaSung);
        btnMuaGun.interactable = !daMuaSung;

        imageBowUnlocked.SetActive(daMuaCung);
        btnMuaBow.interactable = !daMuaCung;

        int soBinhMau = PlayerPrefs.GetInt("SoBinhMau", 0);
        bool coBinhMau = soBinhMau > 0;

        binhMauImage.gameObject.SetActive(coBinhMau);
        binhMauText.gameObject.SetActive(coBinhMau);
        binhMauText.text = soBinhMau.ToString();

        if (imageBinhMauUnlocked != null)
            imageBinhMauUnlocked.SetActive(coBinhMau);

        if (btnMuaMau != null)
            btnMuaMau.interactable = soBinhMau < maxBinhMau;
    }
    public void MuaTen()
    {
        int vang = PlayerPrefs.GetInt("Player1", 0);
        if (vang >= 30) // giá 30 vàng
        {
            BOW bow = FindObjectOfType<BOW>();
            if (bow != null)
            {
                if (bow.currentAmmo < bow.maxAmmo)
                {
                    int them = 10; // mua thêm 10 mũi tên
                    bow.AddAmmo(them);

                    vang -= 30;
                    PlayerPrefs.SetInt("Player1", vang);
                    PlayerPrefs.Save();

                    CapNhatUI();
                    Debug.Log($"✅ Đã mua {them} mũi tên.");
                }
                else
                {
                    Debug.Log("❌ Đã đủ 50 mũi tên, không thể mua thêm.");
                }
            }
        }
        else
        {
            Debug.Log("❌ Không đủ vàng để mua tên.");
        }
    }








    public static bool DaMuaSung()
    {
        return daMuaSung;
    }

    public static bool DaMuaCung()
    {
        return daMuaCung;
    }

}
