using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        currentHealth = maxHealth;
        //set player health to maxHealth to start
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        //to test if health bar works- can press space bar to trigger
        // if(Input.GetKeyDown(KeyCode.Space)) {
        //     TakeDamage(10); 
        // }

        /* TO ADD:
            enemy attack -> causes player health to decrease
        */

        //once player health gets below 0, go back to home screen (or load screen with saved checkpoints)
        if(currentHealth <= 0) {
             SceneManager.LoadSceneAsync("home_screen_scene");
        }

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
