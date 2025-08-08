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

    public static bool daMuaSung = false;
    public static bool daMuaCung = false;

    private void Start()
    {
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
        else { Debug.Log("Khong Du Tien"); }
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
        }else { Debug.Log("Khong Du Tien"); }
    }
    public void MuaDan()
    {
        int vang = PlayerPrefs.GetInt("Player1", 0);
        if (vang >= 50)
        {
            if (GameData2.Instance != null)
            {
                int reserve = GameData2.Instance.reserveAmmo;

                // Đã từng đạt giới hạn 100 -> chỉ được mua tiếp nếu reserve = 0
                if (reserve >= 100)
                {
                    if (reserve > 0)
                    {
                        Debug.Log("Đã đạt giới hạn 100 viên. Chỉ được mua tiếp khi hết sạch đạn dự trữ.");
                        return;
                    }
                    // reserve == 0 thì cho phép mua tiếp
                }

                int danThem = 25;
                if (reserve + danThem > 100)
                {
                    danThem = 100 - reserve;
                }

                GameData2.Instance.reserveAmmo += danThem;

                // Trừ tiền
                vang -= 50;
                PlayerPrefs.SetInt("Player1", vang);
                PlayerPrefs.Save();
                   CapNhatUI();
                // Cập nhật UI nếu có Gun trong scene
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












    private void CapNhatUI()
    {
        coinText.text = "X " + PlayerPrefs.GetInt("Player1", 0);

        imageGunUnlocked.SetActive(daMuaSung);
        btnMuaGun.interactable = !daMuaSung;

        imageBowUnlocked.SetActive(daMuaCung);
        btnMuaBow.interactable = !daMuaCung;
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
