using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Basic Stats Settings")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int movementSpeed;
    [Header("Drop Rate Settings")]
    [SerializeField] protected int healthPotionDroprate;
    [SerializeField] protected int yarnDroprate;
    [SerializeField] protected GameObject lootSpawnerPrefab;
    [Header("Player Detection Settings")]
    [SerializeField] protected float detectRadius;
    [SerializeField] protected LayerMask whatIsTaget;

    protected int health;
    protected bool alive;
    protected Rigidbody2D rb2d;
    protected float movementSpeedModifier = 1f;
    protected bool isStunned = false;

    [SerializeField] protected GameObject[] postDeathEntityPrefabs;
    [SerializeField] protected GameObject smokeCloudPrefab;
    [SerializeField] protected float deathAnimLength;
    [SerializeField] int numPostDeathEntities;
    protected GameObject[] postDeathEntityObjects;
    void Awake()
    {
        health = maxHealth;
        alive = true;
        rb2d = GetComponent<Rigidbody2D>();
        postDeathEntityObjects = new GameObject[numPostDeathEntities];
    }

    public bool isAlive() { return alive; }

    public virtual void ReactToHit(int damage)
    {
        if (alive)
        {
            health = Mathf.Max(health - damage, 0);

            if (health == 0)
            {
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

	protected IEnumerator stunEffect(float duration, float speedMultiplier)
    {
		isStunned = true;
		movementSpeedModifier *= speedMultiplier;
		yield return new WaitForSeconds(duration);
		movementSpeedModifier /= speedMultiplier;
		isStunned = false;
	}

    protected IEnumerator Die()
    {
        float timeToDestroy = 2.5f; // default value for the time objects which appear after death will last for
        postDeathEntityObjects = new GameObject[numPostDeathEntities];
        //make enemy stay still and not collide with player
        rb2d.bodyType = RigidbodyType2D.Static;


		//wait before initiating smoke etc
		yield return new WaitForSeconds(deathAnimLength);

		//Create smoke cloud and post death animal to appear at the position of the enemy
		// GameObject[] postDeathEntityObjects = new GameObject[numPostDeathEntities];
        GameObject smokeCloudObject;

        //check if there is a prefab for the smoke cloud
        smokeCloudObject = Instantiate<GameObject>(smokeCloudPrefab);
        smokeCloudObject.transform.position = transform.position;
        

		//check if there is a prefab for the post death entity
        
		if (numPostDeathEntities>0)
		{
            float spawningRadius = 1.5f;
			for(int i = 0; i < numPostDeathEntities; i++)
            {
                print(postDeathEntityObjects);
                print(postDeathEntityPrefabs.Length);
				postDeathEntityObjects[i] = Instantiate(postDeathEntityPrefabs[i]) as GameObject;
                Vector3 postDeathPos = transform.position;
                postDeathPos.x += (Random.value * spawningRadius) - (spawningRadius / 2);
                postDeathPos.y += (Random.value * spawningRadius) - (spawningRadius / 2);
                postDeathEntityObjects[i].transform.position = postDeathPos;
			}
			//set time to destroy based on lifetime of post death object
			PostDeathEntity postDeathEntityComponent = postDeathEntityObjects[0].GetComponent<PostDeathEntity>();
			if (postDeathEntityComponent) { timeToDestroy = postDeathEntityComponent.getLifetime(); }
		}
        

        //make enemy invisible:
        transform.localScale = Vector3.zero;
        GetComponent<CircleCollider2D>().enabled = false;
        Instantiate(lootSpawnerPrefab, transform.position, Quaternion.identity)
            .GetComponent<LootSpawner>().SpawnLoot(healthPotionDroprate, yarnDroprate);

        //wait for smoke and post death entity to do their thing
        yield return new WaitForSeconds(timeToDestroy);

		//destroy everything
        
		for (int i = 0; i < numPostDeathEntities; i++)
        {
			Destroy(postDeathEntityObjects[i]);
		}
        
        Destroy(smokeCloudObject);

        Destroy(gameObject);
    }
}

