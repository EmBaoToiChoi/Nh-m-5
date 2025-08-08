using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ThanhMauPl_1 : MonoBehaviour
{
    private static ThanhMauPl_1 instance;
    public static ThanhMauPl_1 Instance => instance;

    [Header("UI Thanh máu")]
    public Image thanhmau1;

    [Header("Chỉ số máu")]
    public float mautoida = 100f;
    public float mauhientai;

    [Header("Bình máu")]
    public int soLuongBinhMau = 3;
    public int soLuongToiDa = 5;
    public float hoiMauMoiBinh = 50f;
    public Image iconBinhMau; // ảnh bình máu (ẩn khi hết)
    public TextMeshProUGUI soLuongBinhText; // text hiện số bình máu
    public GameObject imageBinhMauUnlocked;
    

    private void Awake()
    {
        Debug.Log("Awake from ThanhMauPl_1: " + this.gameObject.name);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        soLuongBinhMau = PlayerPrefs.GetInt("SoBinhMau", soLuongBinhMau); // Load từ prefs

        if (mauhientai <= 0f || mauhientai > mautoida)
            mauhientai = mautoida;

        Capnhatthanhmau();
        CapNhatUIBinhMau();
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SuDungBinhMau();
        }
    }

    // ---------------- MAU -----------------
    public void Capnhatthanhmau()
    {
        thanhmau1.fillAmount = Mathf.Clamp01(mauhientai / mautoida);
    }

    public void HoiMau(float soLuong)
    {
        mauhientai += soLuong;
        if (mauhientai > mautoida)
            mauhientai = mautoida;

        Capnhatthanhmau();
        Debug.Log("Đã hồi máu: " + soLuong + " | Máu hiện tại: " + mauhientai);
    }

    public void TruMau(float soLuong)
    {
        mauhientai -= soLuong;
        if (mauhientai < 0)
            mauhientai = 0;

        Capnhatthanhmau();
        Debug.Log("Đã trừ máu: " + soLuong + " | Máu hiện tại: " + mauhientai);
    }

    public void HoiDayMau()
    {
        mauhientai = mautoida;
        Capnhatthanhmau();
    }

    // ------------- BINH MAU ---------------

    public void SuDungBinhMau()
    {
        soLuongBinhMau = PlayerPrefs.GetInt("SoBinhMau", 0);

        if (soLuongBinhMau > 0)
        {
            soLuongBinhMau--;
            PlayerPrefs.SetInt("SoBinhMau", soLuongBinhMau);
            PlayerPrefs.Save();

            HoiMau(hoiMauMoiBinh);
            CapNhatUIBinhMau();
        }
        else
        {
            Debug.Log("Không còn bình máu để sử dụng!");
        }
        MuaVuKhi muaVuKhi = FindObjectOfType<MuaVuKhi>();
        if (muaVuKhi != null)
        {
            muaVuKhi.SendMessage("CapNhatUI");
        }

    }







    public void ThemBinhMau(int soLuong)
    {
        soLuongBinhMau += soLuong;
        if (soLuongBinhMau > soLuongToiDa)
            soLuongBinhMau = soLuongToiDa;

        CapNhatUIBinhMau();
    }

    public void CapNhatUIBinhMau()
    {
        soLuongBinhMau = PlayerPrefs.GetInt("SoBinhMau", 0);

        bool coBinhMau = soLuongBinhMau > 0;

        if (iconBinhMau != null)
            iconBinhMau.gameObject.SetActive(coBinhMau);

        if (soLuongBinhText != null)
        {
            soLuongBinhText.gameObject.SetActive(coBinhMau);
            soLuongBinhText.text = soLuongBinhMau.ToString();
        }

        if (imageBinhMauUnlocked != null)
            imageBinhMauUnlocked.SetActive(coBinhMau);
    }
    // Thêm vào trong ThanhMauPl_1
    public void Heal(float amount)
    {
        HoiMau(amount); // Gọi lại hàm HoiMau bạn đã có
    }







}
