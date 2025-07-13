using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public static string Next_Scene = "Gam1";
    public GameObject progressBar;
    public Text text;
    private float fixedLoadingtime = 3f;
    private void Start()
    {
        StartCoroutine(LoadScene(Next_Scene));
    }
    public IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation Operation = SceneManager.LoadSceneAsync(sceneName);
        while (!Operation.isDone)
        {
            float progress = Mathf.Clamp01(Operation.progress / 0.9f);
            progressBar.GetComponent<Image>().fillAmount = progress;
            text.text = (progress * 100).ToString(format: "0") + "%";

            yield return null;
        }

    }
}

