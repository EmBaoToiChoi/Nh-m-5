using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
  

    public GameObject[] players; // Gán 3 RF nhân vật (đã tắt hết)
    public Cinemachine.CinemachineVirtualCamera mainCamera;
    public Transform minimapCamera;

    void Start()
    {
        int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(i == selected); // Chỉ bật đúng nhân vật
        }

        GameObject player = players[selected];

        // Gán camera follow
        mainCamera.Follow = player.transform;
        mainCamera.LookAt = player.transform;

        // Gán minimap follow
        minimapCamera.GetComponent<MinimapFollow>().target = player.transform;
    }


}
