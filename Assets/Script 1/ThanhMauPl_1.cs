using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThanhMauPl_1 : MonoBehaviour
{
    public Image thanhmau1;
    public void Capnhatthanhmau(float mauhientai, float mautoida){
        thanhmau1.fillAmount = mauhientai / mautoida ;
    }
}
