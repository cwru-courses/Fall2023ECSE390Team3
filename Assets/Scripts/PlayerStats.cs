using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats _instance;

    public int maxHealth = 100;
    public int yarncooldown = 0;
    public int currentHealth;
    [SerializeField] private int maxYarn = 100; //start at 100%
    public int currentYarnCount;
    public HealthBar yarnTracker; //use Healthbar as yarn tracker 
    public HealthBar healthBar;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        currentHealth = maxHealth;
        currentYarnCount = maxYarn;
        //set player health to maxHealth to start
        healthBar.SetMaxHealth(maxHealth);
        yarnTracker.SetMaxHealth(maxYarn);
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
        yarncooldown = yarncooldown+1;
        if(yarncooldown>3600*15){
            yarncooldown = 0;
            this.GainYarn(20);
        }


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
        }else{
        currentYarnCount -= amount; 
        yarnTracker.SetHealth(currentYarnCount);
        }
    }

    //increase yarn count
    public void GainYarn(int amount) {
        if(currentYarnCount+amount>maxYarn){
            amount = maxYarn-currentYarnCount;
        }
        currentYarnCount += amount; 
        yarnTracker.SetHealth(currentYarnCount); 
    }
}
