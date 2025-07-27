using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour
{
    public Rigidbody2D Play;
    public float fastMove = 7f;
    public float move = 4f;
    public float ngang, doc;
    public Animator ani2;
    public float baseMaxHealth = 100f; // máu gốc
    private float currentHealth;

    [SerializeField] private GameObject hit_right, hit_up, hit_down;
    [SerializeField] private GameObject gunObject; 
    private bool is_attack;
    private float timer;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip hit1;
    [SerializeField] private AudioSource source1;
    [SerializeField] private AudioClip enemyHitSound; 
    [SerializeField] private AudioSource audioSource;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public ThanhMauPl_1 thanhmau;
    public float mauhientai;
    public float mautoida;
    [SerializeField]
    private bool isMelee = true;
    public static float mauLuuTru;
    public ThanhNangLuongPl_1 thanhNangLuong;

    public float nangLuongHienTai;   
    public float nangLuongToiDa = 100f; 
    public float tocDoHoiNangLuong = 10f; 
    public float tocDoTieuHaoNangLuong = 20f; 

    private bool coTheChayNhanh = true;
    public static float nangLuongLuuTru;
    private bool isBow = false;
    private bool isGun = false;
    [SerializeField] private GameObject bowObject;
    [SerializeField] private Vector3 offsetBow = new Vector3((float)0.1, (float)0.1, (float)0.1);
    
    public void Heal(float amount)
    {
        currentHealth -= amount;
        mauhientai += amount;
        mauhientai = Mathf.Clamp(mauhientai, 0, mautoida);
        thanhmau.Capnhatthanhmau(mauhientai, mautoida);
        Debug.Log("❤️ Player hồi máu: +" + amount);
    }
    

    void UpdateBowPosition()
    {
        if (isBow)
        {
            // Cho cung di chuyển theo Player
            bowObject.transform.position = transform.position + offsetBow;
        }
    }

    void loadsence1()
    {
        SceneManager.LoadScene("Gam1,1");

    }
    void loadsence2()
    {
        SceneManager.LoadScene("Gam1,2");

    }
    void loadsence3()
    {
        SceneManager.LoadScene("Gam1,3");

    }
    void loadsence4()
    {
        SceneManager.LoadScene("Gam1");

    }
    void loadsencethua()
    {
        SceneManager.LoadScene("Loss");

    }
    // 


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 👉 Tìm lại ThanhMauPl_1 khi sang scene mới
        thanhmau = FindObjectOfType<ThanhMauPl_1>();
        if (thanhmau != null)
        {
            thanhmau.Capnhatthanhmau(mauhientai, mautoida);
        }
    }


// 
    private void Awake()
    {
        
        Play = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gunObject.SetActive(false); 
    }

    private void MovePlayer()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Play.velocity = input.normalized * move;
        float speed = move; 

        if (Input.GetKey(KeyCode.LeftShift) && coTheChayNhanh)
        {
            speed = fastMove;

            // ⚡ Tiêu hao năng lượng khi chạy nhanh
            nangLuongHienTai -= tocDoTieuHaoNangLuong * Time.deltaTime;
            if (nangLuongHienTai <= 0)
            {
                nangLuongHienTai = 0;
                coTheChayNhanh = false; 
            }
            thanhNangLuong.CapNhatThanhNangLuong(nangLuongHienTai, nangLuongToiDa);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enermy") || collision.gameObject.CompareTag("Trap"))
        {
            audioSource.PlayOneShot(enemyHitSound);
            mauhientai -= 10;

    
            thanhmau.Capnhatthanhmau(mauhientai, mautoida);

            if (mauhientai <= 0)
            {
                PlayerPrefs.SetInt("PreviousScene", SceneManager.GetActiveScene().buildIndex);
                loadsencethua();
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Gam1,1"))
        {
            loadsence1();
        }
        if (collision.gameObject.CompareTag("Gam1,2"))
        {
            loadsence2();
        }
        if (collision.gameObject.CompareTag("Gam1,3"))
        {
            loadsence3();
        }
        if (collision.gameObject.CompareTag("BoxBack"))
        {
            loadsence4();
        }


    }

    void Start()
    {
        


        float bonus = GlobalData.healthBonus;
        currentHealth = baseMaxHealth + bonus;

        nangLuongHienTai = nangLuongToiDa;
        thanhNangLuong.CapNhatThanhNangLuong(nangLuongHienTai, nangLuongToiDa);


        if (mauLuuTru <= 0) 
        {
            mauhientai = mautoida;
        }
        else 
        {
            mauhientai = mauLuuTru;
        }


        thanhmau.Capnhatthanhmau(mauhientai, mautoida);


       
        if (nangLuongLuuTru <= 0)
        {
            nangLuongHienTai = nangLuongToiDa;
        }
        else
        {
            nangLuongHienTai = nangLuongLuuTru;
        }


        thanhNangLuong.CapNhatThanhNangLuong(nangLuongHienTai, nangLuongToiDa);

        
        if (mauLuuTru <= 0)
        {
            mauhientai = mautoida;
        }
        else
        {
            mauhientai = mauLuuTru;
        }
        thanhmau.Capnhatthanhmau(mauhientai, mautoida);
    
    
    }
    void OnDestroy()
    {
        
        mauLuuTru = mauhientai;
        nangLuongLuuTru = nangLuongHienTai;
    }


    void Update()
    {
        // PHÍM 1 - Đánh cận chiến
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isMelee = true;
            isBow = false;
            isGun = false;

            gunObject.SetActive(false);
            bowObject.SetActive(false);
            Debug.Log("Chế độ ĐÁNH");
        }

        // PHÍM 2 - Bắn súng (nếu đã mua)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (MuaVuKhi.DaMuaSung())
            {
                isMelee = false;
                isBow = false;
                isGun = true;

                gunObject.SetActive(true);
                bowObject.SetActive(false);
                Debug.Log("Chế độ SÚNG");
            }
            else
            {
                Debug.Log("Bạn chưa mua súng!");
            }
        }

        // PHÍM 3 - Bắn cung (nếu đã mua)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (MuaVuKhi.DaMuaCung())
            {
                isMelee = false;
                isBow = true;
                isGun = false;

                gunObject.SetActive(false);
                bowObject.SetActive(true);
                Debug.Log("Chế độ CUNG");
            }
            else
            {
                Debug.Log("Bạn chưa mua cung!");
            }
        }

        // Hồi năng lượng nếu không chạy
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (nangLuongHienTai < nangLuongToiDa)
            {
                nangLuongHienTai += tocDoHoiNangLuong * Time.deltaTime;
                nangLuongHienTai = Mathf.Clamp(nangLuongHienTai, 0, nangLuongToiDa);
                thanhNangLuong.CapNhatThanhNangLuong(nangLuongHienTai, nangLuongToiDa);
            }

            if (nangLuongHienTai > 0)
            {
                coTheChayNhanh = true;
            }
        }

        // Di chuyển nhân vật
        ngang = Input.GetAxisRaw("Horizontal");
        doc = Input.GetAxisRaw("Vertical");
        Play.velocity = new Vector2(ngang * move, doc * move);

        // Âm thanh bước chân
        if (ngang != 0 || doc != 0)
        {
            if (!source1.isPlaying)
            {
                source1.clip = hit1;
                source1.loop = true;
                source1.Play();
            }
        }
        else
        {
            if (source1.isPlaying)
            {
                source1.Stop();
            }
        }

        // Xử lý vũ khí đang sử dụng
        if (isMelee)
        {
            HandleMelee();
        }
        else if (isBow)
        {
            HandleBow();
            UpdateBowPosition();
        }
        else if (isGun)
        {
            HandleShooting();
        }
    }


    void HandleMelee()
    {
        float currentSpeed = move;

        // Tốc độ khi nhấn shift và còn năng lượng
        if (Input.GetKey(KeyCode.LeftShift) && nangLuongHienTai > 0)
        {
            currentSpeed = fastMove;
            nangLuongHienTai -= tocDoTieuHaoNangLuong * Time.deltaTime;
            if (nangLuongHienTai <= 0)
            {
                nangLuongHienTai = 0;
                coTheChayNhanh = false;
            }
            thanhNangLuong.CapNhatThanhNangLuong(nangLuongHienTai, nangLuongToiDa);
        }

        // Nhận input di chuyển
        ngang = Input.GetAxisRaw("Horizontal");
        doc = Input.GetAxisRaw("Vertical");
        Play.velocity = new Vector2(ngang * currentSpeed, doc * currentSpeed);

        // Xử lý hướng & animation di chuyển
        if (ngang > 0)
        {
            ani2.SetBool("chayad", true);
            transform.localScale = new Vector3(5, 5, 5);
        }
        else if (ngang < 0)
        {
            ani2.SetBool("chayad", true);
            transform.localScale = new Vector3(-5, 5, 5);
        }
        else
        {
            ani2.SetBool("chayad", false);
        }

        if (doc > 0)
        {
            ani2.SetBool("chayw", true);
            ani2.SetBool("chays", false);
        }
        else if (doc < 0)
        {
            ani2.SetBool("chays", true);
            ani2.SetBool("chayw", false);
        }
        else
        {
            ani2.SetBool("chays", false);
            ani2.SetBool("chayw", false);
        }

        // 🟡 Tách phần xử lý tấn công riêng:
        if (Input.GetMouseButtonDown(0))
        {
            is_attack = true;
            source.PlayOneShot(hit);

            // Ưu tiên đánh theo hướng đang di chuyển
            if (ngang > 0)
            {
                ani2.SetTrigger("danhad");
                hit_right.SetActive(true);
            }
            else if (ngang < 0)
            {
                ani2.SetTrigger("danhad");
                hit_right.SetActive(true);
            }
            else if (doc > 0)
            {
                ani2.SetTrigger("danhw");
                hit_up.SetActive(true);
            }
            else if (doc < 0)
            {
                ani2.SetTrigger("danhs");
                hit_down.SetActive(true);
            }

            timer = 0;
        }

        // Tắt hit box sau thời gian
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



    void HandleShooting()
    {
        float currentSpeed = move;
    if (Input.GetKey(KeyCode.LeftShift) && nangLuongHienTai > 0)
    {
        currentSpeed = fastMove;
        nangLuongHienTai -= tocDoTieuHaoNangLuong * Time.deltaTime;
        if (nangLuongHienTai <= 0)
        {
            nangLuongHienTai = 0;
            coTheChayNhanh = false;
        }
        thanhNangLuong.CapNhatThanhNangLuong(nangLuongHienTai, nangLuongToiDa);
    }


        ngang = Input.GetAxisRaw("Horizontal");
        doc = Input.GetAxisRaw("Vertical");
        Play.velocity = new Vector2(ngang * currentSpeed, doc * currentSpeed);

        if (ngang > 0)
        {
            ani2.SetBool("chayad", true);
            transform.localScale = new Vector3(5, 5, 5);
        }
        else if (ngang < 0)
        {
            ani2.SetBool("chayad", true);
            transform.localScale = new Vector3(-5, 5, 5);
        }
        else
        {
            ani2.SetBool("chayad", false);
        }

        if (doc > 0)
        {
            ani2.SetBool("chayw", true);
            ani2.SetBool("chays", false);
        }
        else if (doc < 0)
        {
            ani2.SetBool("chays", true);
            ani2.SetBool("chayw", false);
        }
        else
        {
            ani2.SetBool("chayw", false);
            ani2.SetBool("chays", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Bắn đạn!");
        }
    }
    void HandleBow()
    {
        float currentSpeed = move;
        if (Input.GetKey(KeyCode.LeftShift) && nangLuongHienTai > 0)
        {
            currentSpeed = fastMove;
            nangLuongHienTai -= tocDoTieuHaoNangLuong * Time.deltaTime;
            if (nangLuongHienTai <= 0)
            {
                nangLuongHienTai = 0;
                coTheChayNhanh = false;
            }
            thanhNangLuong.CapNhatThanhNangLuong(nangLuongHienTai, nangLuongToiDa);
        }

        ngang = Input.GetAxisRaw("Horizontal");
        doc = Input.GetAxisRaw("Vertical");
        Play.velocity = new Vector2(ngang * currentSpeed, doc * currentSpeed);

        // ⚡ Flip mặt Player giống như cận chiến
        if (ngang > 0)
        {
            ani2.SetBool("chayad", true);
            transform.localScale = new Vector3(5, 5, 5);
        }
        else if (ngang < 0)
        {
            ani2.SetBool("chayad", true);
            
            transform.localScale = new Vector3(-5, 5, 5);
        }
        else
        {
            ani2.SetBool("chayad", false);
        }

        if (doc > 0)
        {
            ani2.SetBool("chayw", true);
            ani2.SetBool("chays", false);
        }
        else if (doc < 0)
        {
            ani2.SetBool("chays", true);
            ani2.SetBool("chayw", false);
        }
        else
        {
            ani2.SetBool("chayw", false);
            ani2.SetBool("chays", false);
        }
    }





    // void HandleMelee()
    // {
    //     ngang = Input.GetAxisRaw("Horizontal");
    //     doc = Input.GetAxisRaw("Vertical");
    //     Play.velocity = new Vector2(ngang * move, doc * move);

    //     if (Input.GetAxisRaw("Horizontal") > 0)
    //     {

    //         ani2.SetBool("chayad", true);
    //         transform.localScale = new Vector3(5, 5, 5);

    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             is_attack = true;
    //             source.PlayOneShot(hit);
    //             ani2.SetTrigger("danhad");
    //             hit_right.SetActive(is_attack);

    //         }
    //         timer = 0;
    //     }
    //     if (Input.GetAxisRaw("Horizontal") < 0)
    //     {
    //         ani2.SetBool("chayad", true);

    //         transform.localScale = new Vector3(-5, 5, 5);
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             is_attack = true;
    //             source.PlayOneShot(hit);
    //             ani2.SetTrigger("danhad");
    //             hit_right.SetActive(is_attack);

    //         }
    //         timer = 0;
    //     }
    //     if (Input.GetAxisRaw("Horizontal") == 0)
    //     {
    //         ani2.SetBool("chayad", false);

    //     }
    //     if (Input.GetAxisRaw("Vertical") > 0)
    //     {
    //         ani2.SetBool("chayw", true);

    //         ani2.SetBool("chays", false);
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             is_attack = true;
    //             source.PlayOneShot(hit);
    //             ani2.SetTrigger("danhw");
    //             hit_up.SetActive(is_attack);

    //         }
    //         timer = 0;
    //     }

    //     if (Input.GetAxisRaw("Vertical") < 0)
    //     {
    //         ani2.SetBool("chays", true);

    //         ani2.SetBool("chayw", false);
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             is_attack = true;
    //             source.PlayOneShot(hit);
    //             ani2.SetTrigger("danhs");
    //             hit_down.SetActive(is_attack);

    //         }
    //         timer = 0;
    //     }
    //     if (Input.GetAxisRaw("Vertical") == 0)
    //     {
    //         ani2.SetBool("chays", false);
    //         ani2.SetBool("chayw", false);

    //     }
    //     if (is_attack)
    //     {
    //         timer += Time.deltaTime;
    //         if (timer > 0.1f)
    //         {
    //             is_attack = false;
    //             hit_right.SetActive(is_attack);
    //             hit_up.SetActive(is_attack);
    //             hit_down.SetActive(is_attack);
    //         }
    //     }

    // }

    // void HandleShooting()
    // {
    //     // Animation di chuyển khi cầm súng
    //     if (Input.GetAxisRaw("Horizontal") > 0)
    //     {
    //         ani2.SetBool("chayad", true);
    //     }
    //     else if (Input.GetAxisRaw("Horizontal") < 0)
    //     {
    //         ani2.SetBool("chaya", true);
    //         ani2.SetBool("chayad", false);
    //     }
    //     else
    //     {
    //         ani2.SetBool("chaya", false);
    //         ani2.SetBool("chayad", false);
    //     }

    //     if (Input.GetAxisRaw("Vertical") > 0)
    //     {
    //         ani2.SetBool("chayw", true);
    //         ani2.SetBool("chays", false);
    //     }
    //     else if (Input.GetAxisRaw("Vertical") < 0)
    //     {
    //         ani2.SetBool("chays", true);
    //         ani2.SetBool("chayw", false);
    //     }
    //     else
    //     {
    //         ani2.SetBool("chays", false);
    //         ani2.SetBool("chayw", false);
    //     }

    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         // Gọi code bắn đạn của bạn ở đây
    //         Debug.Log("Bắn đạn!");
    //     }
    // }
    public void TakeDamage(int damage)
    {
        mauhientai -= damage;
        thanhmau.Capnhatthanhmau(mauhientai, mautoida);

        if (mauhientai <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
