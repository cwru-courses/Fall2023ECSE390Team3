

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{

	protected int health;
	protected float movementSpeed;
	protected float movementSpeedModifier = 1;
	protected bool alive;
	protected Rigidbody2D rb2d;
	protected float basicAttackCDLeft;
	[SerializeField] protected GameObject playerObject;
    [SerializeField] protected GameObject weaponPrefab;
    [SerializeField] protected float basicAttackRange;
    [SerializeField] protected float basicAttackCD;
    [Range(0f, 180f)]
    [SerializeField] protected float basicAttackRangeAngle;
    [SerializeField] protected float basicAttackSwingTime; //duration of swing animation(temporary until real animation exists)
    [SerializeField] protected AudioSource basicAttackSFX;
    [SerializeField] private int maxHealth;
    [SerializeField] protected GameObject smokeCloudPrefab;
	  [SerializeField] protected GameObject postDeathEntityPrefab;
	  [SerializeField] protected float deathAnimLength;
	[SerializeField] protected GameObject healthPotionPrefab;
	[SerializeField] protected float yarnGainByPlayer;



    void Start()
    {
        health = maxHealth;
        alive = true;
        rb2d = GetComponent<Rigidbody2D>();
    }

    public bool isAlive() { return alive; }


    public virtual void ReactToHit(int damage)
    {
        if (alive)
        {
			health -= damage;
			if (health <= 0)
			{
				print("alive: " + alive + " health: " + health);
				alive = false;
				StopAllCoroutines();
				StartCoroutine(Die());
			}
		}
        
    }

	public virtual void stun(float duration, float speedMultiplier)
    {
		StartCoroutine(stunEffect(duration, speedMultiplier));
    }

	private IEnumerator stunEffect(float duration, float speedMultiplier)
    {
		movementSpeedModifier *= speedMultiplier;
		print(movementSpeedModifier);
		yield return new WaitForSeconds(duration);
		movementSpeedModifier /= speedMultiplier;
	}
	

    protected IEnumerator Die()
    {
		int numPostDeathEntities = 3;
        float timeToDestroy = 2.5f; // default value for the time objects which appear after death will last for

		//wait before initiating smoke etc
		yield return new WaitForSeconds(deathAnimLength);

		//Create smoke cloud and post death animal to appear at the position of the enemy
		GameObject smokeCloudObject = null;
		GameObject[] postDeathEntityObjects = new GameObject[numPostDeathEntities];
		//Create health potion object to appear at the position of the enemy -- Jing
		GameObject healthPotion = null;

		//check if there is a prefab for the smoke cloud to instantiate
		if (smokeCloudPrefab)
		{
			smokeCloudObject = Instantiate(smokeCloudPrefab) as GameObject;
			smokeCloudObject.transform.position = transform.position;
		}

		//check if there is a prefab for the post death entity
		if (postDeathEntityPrefab && numPostDeathEntities>0)
		{
			for(int i = 0; i < numPostDeathEntities; i++)
            {
				postDeathEntityObjects[i] = Instantiate(postDeathEntityPrefab) as GameObject;
				postDeathEntityObjects[i].transform.position = transform.position;
			}
			//set time to destroy based on lifetime of post death object
			PostDeathEntity postDeathEntityComponent = postDeathEntityObjects[0].GetComponent<PostDeathEntity>();
			if (postDeathEntityComponent) { timeToDestroy = postDeathEntityComponent.getLifetime(); }
		}

		//check if there is a prefab for the health position -- Jing
		if (healthPotionPrefab)
        {
			healthPotion = Instantiate(healthPotionPrefab) as GameObject;
			healthPotion.transform.position = transform.position;
        }

		//give the player some yarn -- Jing
		PlayerStats._instance.GainYarn(yarnGainByPlayer);

		//make this enemy invisible
		SpriteRenderer SR = GetComponent<SpriteRenderer>();
		if (SR != null) { SR.color = Color.clear; }
		transform.localScale = Vector3.zero;


		//wait for smoke and post death entity to do their thing
		yield return new WaitForSeconds(timeToDestroy);

		//destroy everything
		for (int i = 0; i < numPostDeathEntities; i++)
        {
			Destroy(postDeathEntityObjects[i]);
		}
		Destroy(smokeCloudObject);
		Destroy(this.gameObject);
    }

    protected abstract void move();
    protected abstract void attack();

	
}

