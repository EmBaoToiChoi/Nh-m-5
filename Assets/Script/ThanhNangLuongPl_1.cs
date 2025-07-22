using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThanhNangLuongPl_1 : MonoBehaviour
{
    private static ThanhNangLuongPl_1 instance;
    public Image thanhNangLuong1;

    public void CapNhatThanhNangLuong(float nangLuongHienTai, float nangLuongToiDa)
    {
        thanhNangLuong1.fillAmount = nangLuongHienTai / nangLuongToiDa;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
