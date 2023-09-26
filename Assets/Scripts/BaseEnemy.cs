

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{

	  protected int health;
	  protected int movementSpeed;
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


    void Start()
    {
        health = maxHealth;
        alive = true;
        rb2d = GetComponent<Rigidbody2D>();
    }

    public bool isAlive() { return alive; }


    public virtual void ReactToHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            alive = false;
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    protected IEnumerator Die()
    {
        float timeToDestroy = 2.5f; // default value for the time objects which appear after death will last for

		//wait before initiating smoke etc
		yield return new WaitForSeconds(deathAnimLength);

		//Create smoke cloud and post death animal to appear at the position of the enemy
		GameObject smokeCloudObject = null;
		GameObject postDeathEntityObject = null;

		//check if there is a prefab for the smoke cloud to instantiate
		if (smokeCloudPrefab)
		{
			smokeCloudObject = Instantiate(smokeCloudPrefab) as GameObject;
			smokeCloudObject.transform.position = transform.position;
		}

		//check if there is a prefab for the post death entity
		if (postDeathEntityPrefab)
		{
			postDeathEntityObject = Instantiate(postDeathEntityPrefab) as GameObject;
			postDeathEntityObject.transform.position = transform.position;

			//set time to destroy based on lifetime of post death object
			PostDeathEntity postDeathEntityComponent = postDeathEntityObject.GetComponent<PostDeathEntity>();
			if (postDeathEntityComponent) { timeToDestroy = postDeathEntityComponent.getLifetime(); }
		}

		//make this enemy invisible
		SpriteRenderer SR = GetComponent<SpriteRenderer>();
		if (SR != null) { SR.color = Color.clear; }

		//wait for smoke and post death entity to do their thing
		yield return new WaitForSeconds(timeToDestroy);

		//destroy everything
		Destroy(postDeathEntityObject);
		Destroy(smokeCloudObject);
		Destroy(this.gameObject);
    }

    protected abstract void move();
    protected abstract void attack();
}

