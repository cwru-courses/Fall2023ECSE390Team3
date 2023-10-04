using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Basic Stats Settings")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int movementSpeed;
    [Header("Drop Rate Settings")]
    [SerializeField] protected GameObject healthPotionPrefab;
    [SerializeField] protected float yarnDropRate;
    [Header("Player Detection Settings")]
    [SerializeField] protected float detectRadius;
    [SerializeField] protected LayerMask whatIsTaget;

    protected int health;
    protected bool alive;
    protected CharacterController controller;
	protected float movementSpeedModifier = 1f;
	protected bool isStunned = false;

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

