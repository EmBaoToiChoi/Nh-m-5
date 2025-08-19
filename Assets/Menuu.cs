using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menuu : MonoBehaviour
{
    public GameObject optionPanel;
    public Slider volumeSlider;

    void Start()
    {
        optionPanel.SetActive(false);

        // Gán giá trị slider từ âm lượng đã lưu
        if (Volumsetting.Instance != null)
        {
            volumeSlider.value = Volumsetting.Instance.GetVolume();
        }

        volumeSlider.onValueChanged.AddListener((value) =>
        {
            if (Volumsetting.Instance != null)
            {
                Volumsetting.Instance.SetVolume(value);
            }
        });
    }

    public void OpenOptions()
    {
        optionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseOptions()
    {
        optionPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu2");
    }
}