using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading2 : MonoBehaviour
{
    public static string Next_Scene = "Gam1"; // sẽ được gán từ MainMenu

    public GameObject progressBar;
    public Text text;
    private float fixedLoadingtime = 3f;

    private void Start()
    {
        StartCoroutine(LoadScene2(Next_Scene));
    }

    public IEnumerator LoadScene2(string sceneName)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fixedLoadingtime)
        {
            float progress = Mathf.Clamp01(elapsedTime / fixedLoadingtime);
            progressBar.GetComponent<Image>().fillAmount = progress;
            text.text = (progress * 100).ToString("0") + "%";
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
