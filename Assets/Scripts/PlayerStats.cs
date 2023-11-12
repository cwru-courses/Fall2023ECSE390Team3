using System; 
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats _instance;

    public PlayerInput playerInput;

    public Vector3 initialPosition;
    public int maxHealth = 100;
    public int yarncooldown = 0;
    public int currentHealth;
    [SerializeField] private int maxYarn = 100; //start at 100%
    public float currentYarnCount;
    public HealthBar yarnTracker; //use Healthbar as yarn tracker 
    public HealthBar healthBar;
    public int potions = 0;
    public int healthFromPotion = 20;
    //public int maxPotion = 8;
    public TextMeshProUGUI potionUI;
    public bool inFlippedWorld;  //keep track of what world player is in

    public bool blocking = false;// is block ability activated currently
    [SerializeField] private SpriteRenderer spRender;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource potionSFX;

    [Min(0f)]
    [SerializeField] private float iFrameTime;

    //private DefaultInputAction playerInputAction;

    private float yarnGainPerSecond = 20f;
    public bool canRegenYarn = true;

    private float iFrameTimer = 0f;
    private bool invincible = false;

    public bool[] levelsReached = {false, false, false};

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        
        if (SaveSystem.listSavedFiles.Contains(SaveSystem.currentFileName))
        {
            SaveSystem.LoadSave();
            healthBar.SetHealth(currentHealth);
            yarnTracker.SetHealth(Mathf.RoundToInt(currentYarnCount));
            Debug.Log("Saved version loaded");
        }
        else
        {
            Debug.Log("New game started");
            currentHealth = maxHealth;
            currentYarnCount = maxYarn;
            //set player health to maxHealth to start
            healthBar.SetMaxHealth(maxHealth);
            yarnTracker.SetMaxHealth(maxYarn);
        }

        //playerInputAction = new DefaultInputAction();
        //playerInputAction.Player.UsePotion.started += UsePotion;
        inFlippedWorld = false;
    }

    private void Start()
    {
        int currentLevel = -1; 
        
        if(String.Compare(SceneManager.GetActiveScene().name, "Tutorial Level") == 0) {
            initialPosition = new Vector3(18.8f, 23.1f, -1f); 
            currentLevel = 0; 
        }
        else if(String.Compare(SceneManager.GetActiveScene().name, "Sanctuary") == 0) {
            initialPosition = new Vector3(-75.3f, 46.7f, 0f); 
            currentLevel = 1; 
        } else if(String.Compare(SceneManager.GetActiveScene().name, "Second Level") == 0) {
            initialPosition = new Vector3(32.6f, 27.3f, -1f); 
            currentLevel = 2; 
        }

        // try to prevent out of map spawn
        //if current level has not been reached yet, set initial position to correct position
        if (initialPosition != null && !levelsReached[currentLevel])
        {
            transform.position = initialPosition; 
            levelsReached[currentLevel] = true; 
        } 
        //else if already reached, then the position is set by the SaveSystem
    }

    //private void OnEnable()
    //{
    //    playerInputAction.Player.UsePotion.Enable();
    //}

    //private void OnDisable()
    //{
    //    playerInputAction.Player.UsePotion.Disable();
    //}

    //public void OnPause(bool paused)
    //{
    //    if (paused)
    //    {
    //        playerInputAction.Player.UsePotion.Disable();
    //    }
    //    else
    //    {
    //        playerInputAction.Player.UsePotion.Enable();
    //    }
    //}

    void Update()
    {
        //once player health gets below 0, go back to home screen (or load screen with saved checkpoints)
        if (currentHealth <= 0)
        {
            SceneManager.LoadSceneAsync("Game Over Screen");
        }
        // only count up yarncooldown by 1 in normal world -- Jing
        if (GameObject.FindWithTag("Player").transform.position.y > 0)
        {
            yarncooldown = yarncooldown + 1;
        }
        if (canRegenYarn)
        {
            yarncooldown = 0;
            this.GainYarn(yarnGainPerSecond*Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (invincible)
        {
            iFrameTimer += Time.fixedDeltaTime;
            if (iFrameTimer > iFrameTime)
            {
                iFrameTimer = 0f;
                invincible = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // check if player is blocking, only take damage if not blocking
        if (!invincible && !blocking)
        {
            currentHealth -= damage;

            invincible = true;

            healthBar.SetHealth(currentHealth);
            if (hitSFX)
            {
                hitSFX.Play();
            }
            if (spRender)
            {
                StartCoroutine(FlashColor(new Color(1f,0.5f,0.5f)));
            }
        }
    }
    //decrease yarn count
    public void UseYarn(float amount)
    {
        if (currentYarnCount < 0)
        {
            Debug.Log("player out of yarn");
        }
        else
        {
            currentYarnCount -= amount;

            // Added this to round the actual yarn enable to display using yarnTracker which only accept int -- Jing
            int yarnToDisplay = Mathf.RoundToInt(currentYarnCount);
            yarnTracker.SetHealth(yarnToDisplay);
        }
    }

    //increase yarn count
    public void GainYarn(float amount)
    {
        if (currentYarnCount + amount > maxYarn)
        {
            amount = maxYarn - currentYarnCount;
        }
        currentYarnCount += amount;

        // Added this to round the actual yarn enable to display using yarnTracker which only accept int -- Jing
        int yarnToDisplay = Mathf.RoundToInt(currentYarnCount);
        yarnTracker.SetHealth(yarnToDisplay);
    }

    public void UsePotion(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (potions > 0)
            {
                potions -= 1;
                currentHealth += healthFromPotion;
                potionUI.text = potions.ToString();
                healthBar.SetHealth(currentHealth);
                if (potionSFX)
                {
                    potionSFX.Play();
                }
            }
            else
            {
                Debug.Log("No Potion");
            }
        }
            
    }

    public void GainPotion()
    {
        // no more upper limit now
        potions += 1;
        potionUI.text = potions.ToString();
        /*
        if (potions < maxPotion)
        {
            potions += 1;
            potionUI.text = potions.ToString();
        }
        */
    }

    private IEnumerator FlashColor(Color color)
    {
        bool originalColor = false;

        while (invincible)
        {
            spRender.color = originalColor ? Color.white : color;
            originalColor = !originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        spRender.color = Color.white;
    }

}
