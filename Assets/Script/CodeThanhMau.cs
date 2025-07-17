using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CodeThanhMau : MonoBehaviour
{
    public Image thanhmau1;
    public void Capnhatthanhmau(float mauhientai, float mautoida){
        thanhmau1.fillAmount = mauhientai / mautoida ;
    }
}
