using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThanhMauPl_1 : MonoBehaviour
{
    private static ThanhMauPl_1 instance;
    public Image thanhmau1;
    public void Capnhatthanhmau(float mauhientai, float mautoida)
    {
        thanhmau1.fillAmount = mauhientai / mautoida;
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // 🔥 Giữ thanh máu khi load scene mới
        }
        else
        {
            Destroy(gameObject); // Xóa UI trùng lặp nếu có
        }
    }

}
