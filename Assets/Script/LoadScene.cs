using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
     public void play(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    public void pause(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
