using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject[] enemies;
    [SerializeField] Text numEnemiesTxt;
    private int numEnemies;
    [SerializeField] int vida;
    void Start()
    {

        enemies = GameObject.FindGameObjectsWithTag("Enemigo");
        numEnemies = enemies.Length;

    }


    // Update is called once per frame
   void Update()

    {
       
        Debug.Log("La cantidad de enemigos es: " + numEnemies);

        if (numEnemies == 0)
        {
            SceneManager.LoadScene("win");
        }

        TextoNumEnemies();
    }

    public void Startgame()
    {
        SceneManager.LoadScene(1);
    }
    public void Menu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void ReducirNumEnemigos()
    {
        numEnemies = numEnemies - 1;
    }

    void TextoNumEnemies()
    {

        numEnemiesTxt.text = numEnemies.ToString();

    }
   
   
}




