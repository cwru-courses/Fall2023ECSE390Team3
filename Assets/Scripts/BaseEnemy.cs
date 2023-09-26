using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int movementSpeed;

    protected int health;
    protected bool alive;
    protected Rigidbody2D rb2d;

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
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }

    protected abstract void move();
    protected abstract void attack();
}