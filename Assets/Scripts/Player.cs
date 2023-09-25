using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private int maxYarn = 100; //start at 100%
    public int currentYarnCount;

    public HealthBar healthBar;
    public HealthBar yarnTracker; //use Healthbar as yarn tracker 

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        currentHealth = maxHealth;
        currentYarnCount = maxYarn; 
        //set player health to maxHealth to start
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        //testing health bar - can delete later
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     TakeDamage(10);
        // }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    //decrease yarn count
    public void UseYarn(int amount) {
        if(currentYarnCount < 0) {
            Debug.Log("player out of yarn"); 
        }
        currentYarnCount -= amount; 
        yarnTracker.SetHealth(currentYarnCount);
    }

    //increase yarn count
    public void GainYarn(int amount) {
        currentYarnCount += amount; 
        yarnTracker.SetHealth(currentYarnCount); 
    }

}
