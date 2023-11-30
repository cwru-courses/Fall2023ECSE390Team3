using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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

    [SerializeField] protected GameObject postDeathEntityPrefab;
    [SerializeField] protected GameObject smokeCloudPrefab;
    [SerializeField] protected float deathAnimLength;

    void Awake()
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

        //make enemy stay still and not collide with player
        rb2d.bodyType = RigidbodyType2D.Static;

		//wait before initiating smoke etc
		yield return new WaitForSeconds(deathAnimLength);

        // int numPostDeathEntities = 3;
        // Between 1 and 3 post - death entities
        int numPostDeathEntities = Random.Range(1, 4);


        //Create smoke cloud and post death animal to appear at the position of the enemy
        GameObject[] postDeathEntityObjects = new GameObject[numPostDeathEntities];
        GameObject smokeCloudObject;

        //check if there is a prefab for the smoke cloud
        smokeCloudObject = Instantiate<GameObject>(smokeCloudPrefab);
        smokeCloudObject.transform.position = transform.position;

        

        //check if there is a prefab for the post death entity
        /*
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
        */


        if (postDeathEntityPrefab && numPostDeathEntities > 0) {

            // Pick (numPostDeathEntitites) numbers from this array
            List<int> numbers = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

            int number1 = numbers[Random.Range(0, numbers.Count - 1)];
            numbers.Remove(number1);

            int number2 = numbers[Random.Range(0, numbers.Count - 1)];
            numbers.Remove(number2);

            int number3 = numbers[Random.Range(0, numbers.Count - 1)];
            numbers.Remove(number3);


            // Resources.Load<Sprite>("folderInsideresourcesfolder/char1_0");
            
         
            for (int i = 0; i < numPostDeathEntities; i++) {
                postDeathEntityObjects[i] = Instantiate(postDeathEntityPrefab) as GameObject;

                // If melee enemy (red cat)
                if (this.GetComponent("Melee Enemy") != null) {
                    postDeathEntityObjects[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bobble/Bobbles Death Pieces Red_" + numbers[i]);
                    Debug.Log("Bobble/Bobbles Death Pieces Red_" + numbers[i]);
                }
                // Else, if ranged enemy (blue cat)
                else if (this.GetComponent("Ranged Enemy") != null) {
                    postDeathEntityObjects[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bobble/Bobbles Death Pieces Blue_" + numbers[i]);
                }

                var positionVary = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
                postDeathEntityObjects[i].transform.position = transform.position + positionVary;
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

		// destroy everything
		for (int i = 0; i < numPostDeathEntities; i++)
        {
			Destroy(postDeathEntityObjects[i]);
		}

        Destroy(smokeCloudObject);

        Destroy(gameObject);
    }
}

