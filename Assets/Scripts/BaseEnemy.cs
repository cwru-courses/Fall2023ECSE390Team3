using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
	protected int health;
	protected int movementSpeed;
	protected bool alive;
	protected Rigidbody2D rb2d;
	[SerializeField] protected GameObject playerObject;

	void Start()
	{
		
	}

	public bool isAlive()
	{
		return alive;
	}
	public virtual void ReactToHit(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			alive = false;
			StartCoroutine(Die());
		}
	}

	protected IEnumerator Die()
	{
		yield return new WaitForSeconds(2.5f);
		Destroy(this.gameObject);
	}

	protected abstract void move();
	protected abstract void attack();
}