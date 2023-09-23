using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player _instance;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        currentHealth = maxHealth;
        //set player health to maxHealth to start
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        //testing health bar - can delete later
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
