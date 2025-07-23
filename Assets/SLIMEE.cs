using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLIMEE : MonoBehaviour
{
    public Transform enermy, player;
    // [SerializeField] private AudioClip Go;
    // [SerializeField] private AudioSource source;

    private bool isRight;
    [SerializeField]
    Animator nie;
    bool isChasing = false;
    private float speed = 2f, PVipHien = 30f;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        float khoangCachPlayer = Vector2.Distance(enermy.position, player.position);
        if (khoangCachPlayer < PVipHien)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
        if (isChasing)
        {
            dichuyentoiPlayer(player.position);
        }
    }
    void dichuyentoiPlayer(Vector3 target)
    {
        Vector3 direction = (target - enermy.position).normalized;
        enermy.Translate(direction * speed * Time.deltaTime);

        // Lật quái
        if (direction.x > 0)
            enermy.localScale = new Vector3(5,5,5);
        if (direction.x < 0)
            enermy.localScale = new Vector3(-5,5,5);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hit"))
        {
            Destroy(gameObject); 
        }

    }
    
}
