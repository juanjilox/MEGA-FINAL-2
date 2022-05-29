using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTurret2 : MonoBehaviour
{
    Animator myAnimator;

    public float speed;
    [SerializeField] AudioClip sfxBulletDestroy;
    Rigidbody2D rb;

    bool destruido = false;
    bool fin = false;
    public bool izq = false;
    private GameObject torreta;



    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        torreta = GameObject.Find("Torreta");

    }

    // Update is called once per frame
    void Update()
    {


        rb.velocity = (transform.up + (transform.right*transform.localScale.x)) * 20f;
        Destruir();

    }



    public void Fin()
    {
        fin = true;
        if (fin == true)
        {
            Destroy(this.gameObject);
        }
    }
    public void Destruir()
    {
        destruido = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {


        if (destruido == true)
        {
            myAnimator.SetTrigger("Destroy");
            speed = 0;

        }
        AudioSource.PlayClipAtPoint(sfxBulletDestroy, Camera.main.transform.position);
    }


}