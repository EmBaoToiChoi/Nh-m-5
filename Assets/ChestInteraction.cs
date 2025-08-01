using UnityEngine;
using UnityEngine.UI;

public class ChestInteraction : MonoBehaviour
{
    [Header("References")]
    public Animator chestAnimator;
    public GameObject textPressE;
    public GameObject keyObject;

    private bool isPlayerNearby = false;
    private bool isOpened = false;

    void Start()
    {
        textPressE.SetActive(false);
        keyObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            chestAnimator.SetTrigger("moruong");
            isOpened = true;
            textPressE.SetActive(false);
            keyObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = true;
            if (!isOpened)
                textPressE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayerNearby = false;
            textPressE.SetActive(false);
        }
    }
}
