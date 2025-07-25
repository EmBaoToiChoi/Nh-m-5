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
