using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public Rigidbody2D Play;
    public float move = 4f;
    public float ngang, doc;
    public Animator ani2;
    [SerializeField] private GameObject hit_right, hit_up, hit_down;
    [SerializeField] private GameObject gunObject; // Súng
    private bool is_attack;
    private float timer;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioSource source;
    [HideInInspector] public SpriteRenderer spriteRenderer;


    private bool isMelee = true; // Mặc định là đánh

    private void Awake()
    {
        Play = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gunObject.SetActive(false); // Ẩn súng khi bắt đầu
    }

    private void MovePlayer()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Play.velocity = input.normalized * move;
    }


    void Update()
    {
        MovePlayer();

        // Chuyển chế độ bằng phím 1 (đánh) và phím 2 (bắn)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isMelee = true;
            gunObject.SetActive(false); // Ẩn súng
            Debug.Log("Chế độ ĐÁNH");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isMelee = false;
            gunObject.SetActive(true); // Hiện súng
            Debug.Log("Chế độ BẮN");
        }

        ngang = Input.GetAxisRaw("Horizontal");
        doc = Input.GetAxisRaw("Vertical");
        Play.velocity = new Vector2(ngang * move, doc * move);

        if (isMelee)
        {
            HandleMelee();
        }
        else
        {
            HandleShooting(); // Thêm animation chạy khi cầm súng
        }
    }

    void HandleMelee()
    {
        ngang = Input.GetAxisRaw("Horizontal");
        doc = Input.GetAxisRaw("Vertical");
        Play.velocity = new Vector2(ngang * move, doc * move);

        if (Input.GetAxisRaw("Horizontal") > 0)
        {

            ani2.SetBool("chayad", true);
            transform.localScale = new Vector3(5, 5, 5);

            if (Input.GetMouseButtonDown(0))
            {
                is_attack = true;
                source.PlayOneShot(hit);
                ani2.SetTrigger("danhad");
                hit_right.SetActive(is_attack);

            }
            timer = 0;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            ani2.SetBool("chayad", true);

            transform.localScale = new Vector3(-5, 5, 5);
            if (Input.GetMouseButtonDown(0))
            {
                is_attack = true;
                source.PlayOneShot(hit);
                ani2.SetTrigger("danhad");
                hit_right.SetActive(is_attack);

            }
            timer = 0;
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            ani2.SetBool("chayad", false);

        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            ani2.SetBool("chayw", true);

            ani2.SetBool("chays", false);
            if (Input.GetMouseButtonDown(0))
            {
                is_attack = true;
                source.PlayOneShot(hit);
                ani2.SetTrigger("danhw");
                hit_up.SetActive(is_attack);

            }
            timer = 0;
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            ani2.SetBool("chays", true);

            ani2.SetBool("chayw", false);
            if (Input.GetMouseButtonDown(0))
            {
                is_attack = true;
                source.PlayOneShot(hit);
                ani2.SetTrigger("danhs");
                hit_down.SetActive(is_attack);

            }
            timer = 0;
        }
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            ani2.SetBool("chays", false);
            ani2.SetBool("chayw", false);

        }
        if (is_attack)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                is_attack = false;
                hit_right.SetActive(is_attack);
                hit_up.SetActive(is_attack);
                hit_down.SetActive(is_attack);
            }
        }
            
}

    void HandleShooting()
    {
        // Animation di chuyển khi cầm súng
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            ani2.SetBool("chayad", true);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            ani2.SetBool("chaya", true);
            ani2.SetBool("chayad", false);
        }
        else
        {
            ani2.SetBool("chaya", false);
            ani2.SetBool("chayad", false);
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            ani2.SetBool("chayw", true);
            ani2.SetBool("chays", false);
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            ani2.SetBool("chays", true);
            ani2.SetBool("chayw", false);
        }
        else
        {
            ani2.SetBool("chays", false);
            ani2.SetBool("chayw", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Gọi code bắn đạn của bạn ở đây
            Debug.Log("Bắn đạn!");
        }
    }
}
