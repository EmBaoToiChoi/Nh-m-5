using UnityEngine;
using UnityEngine.UI;

public class ThanhMauPl_1 : MonoBehaviour
{
    private static ThanhMauPl_1 instance;

    public Image thanhmau1;

    public void Capnhatthanhmau(float mauhientai, float mautoida)
    {
        if (thanhmau1 != null)
        {
            thanhmau1.fillAmount = mauhientai / mautoida;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không bị lỗi "only works for root GameObject"
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Xoá bản trùng
        }
    }
}
