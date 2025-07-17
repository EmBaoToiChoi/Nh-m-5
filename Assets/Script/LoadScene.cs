using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
     public void Play(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    public void Pause(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
