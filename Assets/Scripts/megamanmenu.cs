using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class megamanmenu : MonoBehaviour
{
    private GameObject canvas;

   

    [SerializeField] Text vidaTxt;
    [SerializeField] int vida;

    [SerializeField] float speed;
    [SerializeField] float dashForce;

    float pasoAni = 0;

    Animator myAnimator;
    Rigidbody2D rb;
    [SerializeField] float jumpSpeed;
    [SerializeField] float fireRate;
    [SerializeField] float dashRate;


    BoxCollider2D myCollider;

    [SerializeField] BoxCollider2D misPies;
    [SerializeField] GameObject disparo;


    [SerializeField] AudioClip sfxHit;
    [SerializeField] AudioClip sfxDeath;
    [SerializeField] AudioClip sfxBullet;
    [SerializeField] AudioClip sfxLanding;
    [SerializeField] AudioClip sfxJump;
    [SerializeField] AudioClip sfxDash;

    // bool enSuelo=false;

    float nextFire = 0;
    //float nextDash = 0;
    float startDashTime;
    [SerializeField] float dashTime;

    bool enDash = false;
    bool Doblesalto;
    public bool mirandoIzquierda = false;


    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("IsRunning", false);
        myAnimator.SetBool("Falling", false);
        myAnimator.SetBool("Dashing", false);
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Suelo() && myAnimator.GetBool("Falling"))
        {
            AudioSource.PlayClipAtPoint(sfxLanding, Camera.main.transform.position);
        }

        if (!myAnimator.GetBool("Dashing"))
        {
            Correr();
        }



        Suelo();
        SueloEnemigo();

        if (SueloEnemigo())
        {
            vida = 0;
            this.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(sfxDeath, Camera.main.transform.position);
        }

        if (Suelo())
        {
            Saltar();
        }
        else if (Doblesalto)
        {
            DObleSaltar();
        }

        if (Suelo())
        {
            Doblesalto = true;
        }
        Cayendo();
        Disparar();
        Dash();

    }

    

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Suelo()/* && Time.time >= nextDash && mirandoIzquierda==false*/)
        {

            /* rb.velocity = new Vector2(dashForce,0);

             enDash = true;
             nextDash = Time.time + dashRate;
             myAnimator.SetBool("Dashing", true);*/
            startDashTime = Time.time;
            myAnimator.SetBool("Dashing", true);

        }

        /* else if (Input.GetKeyDown(KeyCode.LeftShift) && enSuelo == true && Time.time >= nextDash && mirandoIzquierda == true)
         {

             myAnimator.SetBool("Dashing", true);
             rb.velocity = new Vector2(-dashForce, 0); ;


             enDash = true;
             nextDash = Time.time + dashRate;
         }*/

        if (Input.GetKey(KeyCode.LeftShift) && Suelo() /*&& mirandoIzquierda == true*/)
        {
            if (startDashTime + dashTime >= Time.time)
            {
                rb.velocity = new Vector2(dashForce * transform.lossyScale.x, 0);
                AudioSource.PlayClipAtPoint(sfxDash, Camera.main.transform.position);
            }


            else
            {
                myAnimator.SetBool("Dashing", false);

            }
        }

        else
        {
            myAnimator.SetBool("Dashing", false);
            enDash = false;
        }
    }



    void Disparar()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            myAnimator.SetLayerWeight(1, 1);

        }
        else if (Time.time >= pasoAni)
        {
            myAnimator.SetLayerWeight(1, 0);
            pasoAni = Time.time + 1.8f;
        }

        if (Input.GetKeyDown(KeyCode.X) && Time.time >= nextFire && transform.localScale.x < 0)
        {
            Instantiate(disparo, transform.position - new Vector3(1f, 0, 0), transform.rotation);
            nextFire = Time.time + fireRate;
            AudioSource.PlayClipAtPoint(sfxBullet, Camera.main.transform.position);
        }

        else if (Input.GetKeyDown(KeyCode.X) && Time.time >= nextFire && transform.localScale.x > 0)
        {
            Instantiate(disparo, transform.position - new Vector3(-1f, 0, 0), transform.rotation);
            nextFire = Time.time + fireRate;
            AudioSource.PlayClipAtPoint(sfxBullet, Camera.main.transform.position);
        }
    }

    void FinishJump()
    {
        myAnimator.SetBool("Falling", true);
    }

    void Correr()
    {
        float movH = Input.GetAxis("Horizontal");
        if (enDash == false)

        {
            bool puedeCorrer = true;

            if (movH != 0)
            {

                if (movH < 0)
                {
                    transform.localScale = new Vector2(-1, 1);
                    mirandoIzquierda = true;
                }

                else
                {
                    transform.localScale = new Vector2(1, 1);
                    mirandoIzquierda = false;
                }

                if (MirandoPared() && movH < 0 && transform.localScale.x < 0)
                {
                    puedeCorrer = false;
                    myAnimator.SetBool("IsRunning", false);
                }


                else if (MirandoPared() && movH > 0 && transform.localScale.x > 0)
                {
                    puedeCorrer = false;
                    myAnimator.SetBool("IsRunning", false);
                }

                else
                {
                    puedeCorrer = true;
                    if (puedeCorrer)
                    {
                        myAnimator.SetBool("IsRunning", true);
                        rb.velocity = new Vector2(movH * speed, rb.velocity.y);
                    }

                }



                //transform.Translate(new Vector2(movH * Time.deltaTime * speed, 0));
            }

            else
            {

                rb.velocity = new Vector2(0, rb.velocity.y);
                myAnimator.SetBool("IsRunning", false);
            }
        }



    }

    void Saltar()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0, jumpSpeed));
            myAnimator.SetTrigger("Jumping");
            AudioSource.PlayClipAtPoint(sfxJump, Camera.main.transform.position);
        }


    }

    void DObleSaltar()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpSpeed));
            myAnimator.SetTrigger("Jumping");
            Doblesalto = false;
            AudioSource.PlayClipAtPoint(sfxJump, Camera.main.transform.position);
        }


    }

    bool SueloEnemigo()
    {
        RaycastHit2D hitdeRaycast = Physics2D.Raycast(myCollider.bounds.center, Vector2.down, myCollider.bounds.extents.y + 0.15f, LayerMask.GetMask("Enemigo"));
        Debug.DrawRay(myCollider.bounds.center, Vector2.down * (myCollider.bounds.extents.y + 0.15f), Color.red);

        return hitdeRaycast.collider != null;
    }

    bool Suelo()
    {
        /* if (misPies.IsTouchingLayers(LayerMask.GetMask("Ground")))
         {
             enSuelo=true;
         }

         else
         {
             enSuelo = false;
         }*/

        RaycastHit2D hitdeRaycast = Physics2D.Raycast(myCollider.bounds.center, Vector2.down, myCollider.bounds.extents.y + 0.15f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(myCollider.bounds.center, Vector2.down * (myCollider.bounds.extents.y + 0.15f), Color.red);

        return hitdeRaycast.collider != null;
    }

    bool MirandoPared()
    {


        RaycastHit2D hitdeRaycast = Physics2D.Raycast(myCollider.bounds.center, new Vector2(transform.localScale.x, 0), myCollider.bounds.extents.x + 0.1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(myCollider.bounds.center, new Vector2(transform.localScale.x, 0) * (myCollider.bounds.extents.x + 0.1f), Color.red);

        return hitdeRaycast.collider != null;
    }

    void Cayendo()
    {
        if (rb.velocity.y < 0 && !Suelo())
        {
            myAnimator.SetBool("Falling", true);
        }

        else if (Suelo())
        {

            myAnimator.SetBool("Falling", false);
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (myCollider.IsTouchingLayers(LayerMask.GetMask("BalaEnemigo")))
        {


            vida = vida - 1;


            AudioSource.PlayClipAtPoint(sfxHit, Camera.main.transform.position);




            if (vida == 0)
            {
                this.gameObject.SetActive(false);
                AudioSource.PlayClipAtPoint(sfxDeath, Camera.main.transform.position);
                canvas.SetActive(false);
            }
        }

        else if (myCollider.IsTouchingLayers(LayerMask.GetMask("Enemigo")))
        {
            vida = 0;
            this.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(sfxDeath, Camera.main.transform.position);
            canvas.SetActive(false);
        }

    }

}


