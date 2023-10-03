using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected float detectRadius;
    [SerializeField] protected LayerMask whatIsTaget;

    protected int health;
    protected bool alive;
    protected CharacterController controller;
    protected int health;
	protected float movementSpeed;
	protected float movementSpeedModifier = 1f;
	protected bool alive;
	protected bool isStunned = false;
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

    void Awake()
    {
        health = maxHealth;
        alive = true;
        controller = GetComponent<CharacterController>();
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

        //wait for smoke and post death entity to do their thing
        yield return new WaitForSeconds(timeToDestroy);

        Destroy(gameObject);
    }
}

