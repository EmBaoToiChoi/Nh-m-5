using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour
{
    [Header("Di chuyển")]
    public float move = 4f;
    public float fastMove = 7f;
    private Rigidbody2D rb;

    [Header("Combat")]
    public float mautoida = 100f;
    public float mauhientai;
    private bool is_attack;
    private float timer;
    private bool isMelee = true;

    [Header("Tham chiếu")]
    public ThanhMauPl_1 thanhmau;
    public Animator ani2;
    public GameObject gunObject;
    public GameObject hit_right, hit_up, hit_down;

    [Header("Âm thanh")]
    public AudioSource source;
    public AudioClip hit;
    public AudioSource source1;
    public AudioClip stepSound;
    public AudioSource audioSource;
    public AudioClip enemyHitSound;

    public static float mauLuuTru;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gunObject.SetActive(false);
    }

    private void Start()
    {
        if (mauLuuTru <= 0)
            mauhientai = mautoida;
        else
            mauhientai = mauLuuTru;

        thanhmau = FindObjectOfType<ThanhMauPl_1>();
        if (thanhmau != null)
            thanhmau.Capnhatthanhmau(mauhientai, mautoida);
    }

    private void Update()
    {
        Move();
        HandleAttackInput();
        HandleSound();

        if (isMelee)
            HandleMelee();
        else
            HandleShooting();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? fastMove : move;

        rb.velocity = new Vector2(horizontal, vertical).normalized * currentSpeed;

        // Flip
        if (horizontal > 0)
            transform.localScale = new Vector3(5, 5, 5);
        else if (horizontal < 0)
            transform.localScale = new Vector3(-5, 5, 5);
    }

    private void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isMelee = true;
            gunObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isMelee = false;
            gunObject.SetActive(true);
        }
    }

    private void HandleSound()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            if (!source1.isPlaying)
            {
                source1.clip = stepSound;
                source1.loop = true;
                source1.Play();
            }
        }
        else
        {
            if (source1.isPlaying)
                source1.Stop();
        }
    }

    private void HandleMelee()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.x != 0)
        {
            ani2.SetBool("chayad", true);
            if (Input.GetMouseButtonDown(0))
                TriggerAttack("danhad", hit_right);
        }
        else
            ani2.SetBool("chayad", false);

        if (input.y > 0)
        {
            ani2.SetBool("chayw", true);
            ani2.SetBool("chays", false);
            if (Input.GetMouseButtonDown(0))
                TriggerAttack("danhw", hit_up);
        }
        else if (input.y < 0)
        {
            ani2.SetBool("chays", true);
            ani2.SetBool("chayw", false);
            if (Input.GetMouseButtonDown(0))
                TriggerAttack("danhs", hit_down);
        }
        else
        {
            ani2.SetBool("chayw", false);
            ani2.SetBool("chays", false);
        }

        if (is_attack)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                is_attack = false;
                hit_right.SetActive(false);
                hit_up.SetActive(false);
                hit_down.SetActive(false);
            }
        }
    }

    private void TriggerAttack(string triggerName, GameObject hitBox)
    {
        is_attack = true;
        source.PlayOneShot(hit);
        ani2.SetTrigger(triggerName);
        hitBox.SetActive(true);
        timer = 0;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Bắn súng!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("enermy") || collision.collider.CompareTag("Trap"))
        {
            audioSource.PlayOneShot(enemyHitSound);
            TakeDamage(10);
        }

        switch (collision.collider.tag)
        {
            case "Gam1,1":
                SceneManager.LoadScene("Gam1,1");
                break;
            case "Gam1,2":
                SceneManager.LoadScene("Gam1,2");
                break;
            case "Gam1,3":
                SceneManager.LoadScene("Gam1,3");
                break;
            case "BoxBack":
                SceneManager.LoadScene("Gam1");
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        mauhientai -= damage;
        thanhmau?.Capnhatthanhmau(mauhientai, mautoida);

        if (mauhientai <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        mauLuuTru = mauhientai;
    }
}
