using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public Rigidbody2D Play;
    public float move = 4f;
    public float ngang, doc;
    public Animator ani2;
    [SerializeField] private GameObject hit_ringht, hit_up, hit_dow;
    private bool is_attack;
    private float timer;
    [SerializeField]private AudioClip hit;
    [SerializeField]private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
                hit_ringht.SetActive(is_attack);

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
                hit_ringht.SetActive(is_attack);

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
                hit_dow.SetActive(is_attack);

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
            if (timer > 0.01f)
            {
                is_attack = false;
                hit_ringht.SetActive(is_attack);
                hit_up.SetActive(is_attack);
                hit_dow.SetActive(is_attack);
            }
        }
            
    }
}
