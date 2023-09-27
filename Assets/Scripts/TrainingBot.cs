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
    private bool isAttacking;
    private GameObject weaponObject; // used for temporary attack animation

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
        isAttacking = false;
        
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
            if (isAttacking && weaponObject)
            {
                // rotate weapon object
                float swingPercent = (Time.deltaTime) / basicAttackSwingTime;
                weaponObject.transform.Rotate(0f, 0f, swingPercent * basicAttackRangeAngle);
                weaponObject.transform.position = transform.position;
            }
        }
        
    }

    public override void ReactToHit(int damage)
    {
        if (alive)
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
        isAttacking = true;
        StartCoroutine(attackFX()); // attack visual effects
    }
    //temporary implementation until animations are made
    private IEnumerator attackFX()
    {
        //Plays SFX
        if (basicAttackSFX) { basicAttackSFX.Play(); }

        //check if there is a weapon prefab to instantiate
        if (weaponPrefab)
        {
            //creates object to be rotated
            weaponObject = Instantiate(weaponPrefab) as GameObject;
            weaponObject.transform.position = transform.position;

            //rotate weapon to be towards the player
            Vector3 rot = weaponObject.transform.rotation.eulerAngles;

            // division of basicAttackRange is to keep two numbers below 1 to avoid an error message saying Assertion failed on expression
            float xDirection = (playerObject.transform.position.x - transform.position.x) / basicAttackRange;
            float yDirection = (playerObject.transform.position.y - transform.position.y) / basicAttackRange;
            //Debug.Log("xDirection: " + xDirection + "; yDirection: " + yDirection);
            Vector2 moveDir = new Vector2(xDirection, yDirection);
            float offset = basicAttackRangeAngle / 2f;
            rot.z = Mathf.Acos(Vector2.Dot(Vector2.up, moveDir)) * Mathf.Rad2Deg;
            if (moveDir.x > 0) { rot.z *= -1f; }
            rot.z -= offset;
            weaponObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }

        //wait for attack duration
        yield return new WaitForSeconds(basicAttackSwingTime);

        if (weaponObject)
        {
            //after swing time delete the weapon object
            Destroy(weaponObject);
            isAttacking = false;
        }

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
