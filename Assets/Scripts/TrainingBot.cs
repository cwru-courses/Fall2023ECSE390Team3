using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TrainingBot :  BaseEnemy
{
    [SerializeField] private List<Vector3> patrolPoints;
    private int patrolIndex;
    private SpriteRenderer spRender;
    private Color[] colorList;
    private int maxHP;
    // Start is called before the first frame update
    void Start()
    {
        health = 3;
        movementSpeed = 2;
        alive = true;
        rb2d = GetComponent<Rigidbody2D>();
        patrolIndex = 0;
        patrolPoints.Add(transform.position);
        spRender = GetComponent<SpriteRenderer>();
        maxHP = health;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            move();
            // do attack if player is in attack range and cd is ready
            if (playerInAttackRange() && isBasicAttackReady())
            {
                attack();
            }
            // update cd if it is not ready yet
            else if (!isBasicAttackReady())
            {
                basicAttackCDLeft -= Time.deltaTime;
            }
        }
        
    }

    public override void ReactToHit(int damage)
    {
        health -= damage;
        float healthPercent = (float)health / (float)maxHP;
        spRender.color = new Color(healthPercent, healthPercent, healthPercent, 1);
        if (health <= 0)
        {
            alive = false;
            StartCoroutine(Die());
        }
    }
    protected override void move()
    {
        if (patrolPoints.Count > 1)
        {
            Vector3 currPos = transform.position;
            Vector3 movementDir = patrolPoints[patrolIndex] - currPos;
            movementDir = movementDir / movementDir.magnitude;
            Vector3 nextPos = currPos + (movementDir * movementSpeed * Time.deltaTime);
            transform.position = nextPos;
            if ((currPos - nextPos).magnitude > (currPos - patrolPoints[patrolIndex]).magnitude)
            {
                patrolIndex++;
                if (patrolIndex == patrolPoints.Count) { patrolIndex = 0; }
            }
        }
    }
    protected override void attack()
    {
        playerObject.GetComponent<Player>().TakeDamage(10);
        basicAttackCDLeft = basicAttackCD;
    }
    // Check if the player within the basic attack range of the enemy
    protected bool playerInAttackRange()
    {
        float playerEnemyDistance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);
        if (playerEnemyDistance < basicAttackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Check if the basic attack cooling down is ready
    protected bool isBasicAttackReady()
    {
        return basicAttackCDLeft <= 0;
    }
}
