using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaiDat : MonoBehaviour
{
    public GameObject UI; 

    
    public void ShowPanel()
    {
        UI.SetActive(true);
    }

    
    public void HidePanel()
    {
        UI.SetActive(false);
    }
}
