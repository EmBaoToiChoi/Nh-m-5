using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] players;              // Nhân vật prefab
    public GameObject[] characterObjects;     // GameObject tương ứng với mỗi nhân vật (ví dụ: hình ảnh 3D, khung UI, biểu tượng,...)
    public CinemachineVirtualCamera mainCamera;
    public Transform minimapCamera;

    void Start()
    {
        int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Bật đúng nhân vật
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(i == selected);
        }

        // Bật đúng GameObject tương ứng (ví dụ: ảnh nhân vật)
        for (int i = 0; i < characterObjects.Length; i++)
        {
            characterObjects[i].SetActive(i == selected);
        }

        GameObject player = players[selected];

        // Gán camera follow
        mainCamera.Follow = player.transform;
        mainCamera.LookAt = player.transform;

        // Gán minimap follow
        minimapCamera.GetComponent<MinimapFollow>().target = player.transform;
    }
}
