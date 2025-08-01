using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] players; // Prefab 3 nhân vật
    public Transform spawnPoint;
    public CinemachineVirtualCamera mainCamera;
    public Transform minimapCamera; // Camera minimap (gán bằng tay hoặc tìm qua tag)

    void Start()
    {
        int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Spawn nhân vật
        GameObject player = Instantiate(players[selected], spawnPoint.position, Quaternion.identity);

        // Gán player cho camera chính
        mainCamera.Follow = player.transform;
        mainCamera.LookAt = player.transform;

        // Gán player cho minimap
        if (minimapCamera != null)
        {
            minimapCamera.GetComponent<MinimapFollow>().target = player.transform;
        }
    }
}
