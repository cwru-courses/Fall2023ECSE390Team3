using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats _instance;

    public int maxHealth = 100;
    public int yarncooldown = 0;
    public int currentHealth;
    [SerializeField] private int maxYarn = 100; //start at 100%
    public float currentYarnCount;
    public HealthBar yarnTracker; //use Healthbar as yarn tracker 
    public HealthBar healthBar;
    public int potions = 0;
    public int healthFromPotion = 20;
    public TextMeshProUGUI potionUI;

    public bool blocking = false;// is block ability activated currently

    private DefaultInputAction playerInputAction;

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

        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.UsePotion.started += UsePotion;

    }

    private void OnEnable()
    {
        playerInputAction.Player.UsePotion.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.UsePotion.Disable();
    }

    public void OnPause(bool paused)
    {
        if (paused)
        {
            playerInputAction.Player.UsePotion.Disable();
        }
        else
        {
            playerInputAction.Player.UsePotion.Disable();
        }
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
             SceneManager.LoadSceneAsync("Game Over Screen");
        }
        yarncooldown = yarncooldown+1;
        if(yarncooldown>3600*1){
            yarncooldown = 0;
            this.GainYarn(20);
        }
    }

    public void TakeDamage(int damage)
    {
        // check if player is blocking, only take damage if not blocking
        if (!blocking)
        {
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);
        }
    }
     //decrease yarn count
    public void UseYarn(float amount) {
        if(currentYarnCount < 0) {
            Debug.Log("player out of yarn"); 
        }else{
        currentYarnCount -= amount;

        // Added this to round the actual yarn enable to display using yarnTracker which only accept int -- Jing
        int yarnToDisplay = Mathf.RoundToInt(currentYarnCount);
        yarnTracker.SetHealth(yarnToDisplay);
        }
    }

    //increase yarn count
    public void GainYarn(float amount) {
        if(currentYarnCount+amount>maxYarn){
            amount = maxYarn-currentYarnCount;
        }
        currentYarnCount += amount;

        // Added this to round the actual yarn enable to display using yarnTracker which only accept int -- Jing
        int yarnToDisplay = Mathf.RoundToInt(currentYarnCount);
        yarnTracker.SetHealth(yarnToDisplay);
    }

    public void UsePotion(InputAction.CallbackContext ctx)
    {
        if (potions > 0)
        {
            potions -= 1;
            currentHealth += healthFromPotion;
            potionUI.text = potions.ToString();
        }
        else
        {
            Debug.Log("No Potion");
        }
    }

    public void GainPotion()
    {
        potions += 1;
        potionUI.text = potions.ToString();
    }

}
