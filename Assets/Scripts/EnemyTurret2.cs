using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurret2 : MonoBehaviour
{
    BoxCollider2D myCollider;
    Animator myAnimator;

    [SerializeField] GameManager gm;

    [SerializeField] GameObject bala2;
    [SerializeField] GameObject sbala;
    public bool izq = false;

    [SerializeField] float fireRate;
    float nextFire = 0;

    [SerializeField] int vida;
    [SerializeField] GameObject torretaDestruida;

    [SerializeField] AudioClip sfxEnemyDestroy;

    // [SerializeField] Text vidaTxt;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("Disparando", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Serguirmegamanizq())
        {
            //bala dirigida hacia la izqauierda
            bala2.transform.localScale = (new Vector2((transform.localScale.x * -1), transform.localScale.y));

            if (Time.time >= nextFire)
            {
                Instantiate(bala2, transform.position - new Vector3(1, -1, 0), transform.rotation);
                nextFire = Time.time + fireRate;
                Instantiate(sbala, transform.position - new Vector3(-1, -1, 0), transform.rotation);
                nextFire = Time.time + fireRate;
            }
            Debug.Log("izq");
            izq = true;
            myAnimator.SetBool("Disparando", true);
        }

        else if (Serguirmegamander())
        {
            //bala dirigida a la derecha
            bala2.transform.localScale = (new Vector2((transform.localScale.x * 1), transform.localScale.y));
            //torreta mirando a la derecha
            transform.localScale = new Vector2((transform.localScale.x * -1), transform.localScale.y);

            if (Time.time >= nextFire)
            {
                Instantiate(bala2, transform.position - new Vector3(-1, 1, 0), transform.rotation);
                nextFire = Time.time + fireRate;
                Instantiate(sbala, transform.position - new Vector3(-1, -1, 0), transform.rotation);
                nextFire = Time.time + fireRate;
            }
            Debug.Log("deerecha");
            izq = false;
            myAnimator.SetBool("Disparando", true);
        }

        else if (!Serguirmegamander() && !Serguirmegamanizq())
        {
            myAnimator.SetBool("Disparando", false);
        }

    }
    public bool Serguirmegamanizq()
    {
        RaycastHit2D hitdeRaycast = Physics2D.Raycast(myCollider.bounds.center, new Vector2(transform.localScale.x, 0), myCollider.bounds.extents.x - 15f, LayerMask.GetMask("Player"));
        Debug.DrawRay(myCollider.bounds.center, new Vector2(transform.localScale.x, 0) * (myCollider.bounds.extents.x - 15f), Color.red);
        return hitdeRaycast.collider != null;
    }
    public bool Serguirmegamander()
    {
        Debug.Log("Mgaman esta a la derecha");
        RaycastHit2D hitdeRaycast = Physics2D.Raycast(myCollider.bounds.center, new Vector2(transform.localScale.x, 0), myCollider.bounds.extents.x + 15f, LayerMask.GetMask("Player"));
        Debug.DrawRay(myCollider.bounds.center, new Vector2(transform.localScale.x, 0) * (myCollider.bounds.extents.x + 15f), Color.red);
        return hitdeRaycast.collider != null;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Bullet")))
        {
            vida--;

            if (vida == 0)
            {
                AudioSource.PlayClipAtPoint(sfxEnemyDestroy, Camera.main.transform.position);
                myAnimator.SetTrigger("Destruido");

            }


        }

    }
   
    



public void Destruido()
    {
        gm.ReducirNumEnemigos();
        Instantiate(torretaDestruida, transform.position, transform.rotation);
        this.gameObject.SetActive(false);

    }

}
