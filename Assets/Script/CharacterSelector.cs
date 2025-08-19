using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] playersPreview; // Preview trong Menu

    // Gọi khi nhấn nút chọn nhân vật 0, 1, 2
    public void SelectCharacter(int index)
    {
        PlayerPrefs.SetInt("SelectedCharacter", index);
        PlayerPrefs.SetString("NextScene", "Gam1");
        SceneManager.LoadScene("Load"); // Hoặc scene đầu tiên bạn muốn chơi
    }
}
