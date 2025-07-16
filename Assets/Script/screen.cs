using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creen : MonoBehaviour
{
    public void SetFullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        Screen.fullScreen = true;
        Debug.Log("Đã bật FullScreen");
    }


    public void SetWindowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.fullScreen = false;
        Debug.Log("Đã tắt FullScreen");
    }

}
