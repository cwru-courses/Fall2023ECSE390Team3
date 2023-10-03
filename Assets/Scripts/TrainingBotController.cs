using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SpriteRenderer))]
public class TrainingBotController : BaseEnemy
{
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackCD;
    [SerializeField] private List<Vector3> patrolPoints;
    [SerializeField] private float patrolCD;
    [SerializeField] private Color colorOnDeath;

    private float lastAttackTime;
    private int patrolTargetIndex;
    private float patrolCDTimer;
    private Transform targetTransform;
    private SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        alive = true;
        controller = GetComponent<CharacterController>();

        lastAttackTime = 0;
        patrolTargetIndex = 0;
        patrolPoints.Add(transform.position);
        patrolCDTimer = 0;
        targetTransform = null;
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alive)
        {
            Vector3 targetDist;
            if (targetTransform)
            {
                targetDist = targetTransform.position - transform.position;
                if (targetDist.magnitude > attackRadius)
                {
                    controller.Move(targetDist.normalized * movementSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    if (Time.time - lastAttackTime > attackCD)
                    {
                        // atack
                    }
                }
            }
            else
            {
                targetDist = patrolPoints[patrolTargetIndex] - transform.position;
                if (patrolCDTimer == 0)
                {
                    if (targetDist.magnitude > 0.1f)
                    {
                        controller.Move(Vector3.ClampMagnitude(
                                targetDist.normalized * movementSpeed * Time.fixedDeltaTime,
                                targetDist.magnitude
                        ));
                    }
                    else
                    {
                        patrolTargetIndex = (patrolTargetIndex + 1) % patrolPoints.Count;
                        patrolCDTimer = patrolCD;
                    }
                }
                else
                {
                    patrolCDTimer = Mathf.Max(patrolCDTimer - Time.fixedDeltaTime, 0);
                }

                Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, detectRadius, whatIsTaget);
                if (targetCollider) targetTransform = targetCollider.transform;
            }
        }

    }

    public override void ReactToHit(int damage)
    {
        if (alive)
        {
            health = Mathf.Max(health - damage, 0);

            spriteRender.color = Color.Lerp(Color.white, colorOnDeath, health / (float)maxHealth);
            if (health == 0)
            {
                alive = false;
                StartCoroutine(Die());
            }
        }

    }

    //protected override void attack()
    //{
    //    playerObject.GetComponent<PlayerStats>().TakeDamage(10);
    //    basicAttackCDLeft = basicAttackCD;
    //    isAttacking = true;
    //    StartCoroutine(attackFX()); // attack visual effects
    //}
    ////temporary implementation until animations are made
    //private IEnumerator attackFX()
    //{
    //    //Plays SFX
    //    if (basicAttackSFX) { basicAttackSFX.Play(); }

    //    //check if there is a weapon prefab to instantiate
    //    if (weaponPrefab)
    //    {
    //        //creates object to be rotated
    //        weaponObject = Instantiate(weaponPrefab) as GameObject;
    //        weaponObject.transform.position = transform.position;

    //        //rotate weapon to be towards the player
    //        Vector3 rot = weaponObject.transform.rotation.eulerAngles;

    //        // division of basicAttackRange is to keep two numbers below 1 to avoid an error message saying Assertion failed on expression
    //        float xDirection = (playerObject.transform.position.x - transform.position.x) / basicAttackRange;
    //        float yDirection = (playerObject.transform.position.y - transform.position.y) / basicAttackRange;
    //        //Debug.Log("xDirection: " + xDirection + "; yDirection: " + yDirection);
    //        Vector2 moveDir = new Vector2(xDirection, yDirection);
    //        float offset = basicAttackRangeAngle / 2f;
    //        rot.z = Mathf.Acos(Vector2.Dot(Vector2.up, moveDir)) * Mathf.Rad2Deg;
    //        if (moveDir.x > 0) { rot.z *= -1f; }
    //        rot.z -= offset;
    //        weaponObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    //    }

    //    //wait for attack duration
    //    yield return new WaitForSeconds(basicAttackSwingTime);

    //    if (weaponObject)
    //    {
    //        //after swing time delete the weapon object
    //        Destroy(weaponObject);
    //        isAttacking = false;
    //    }

    //}

    // For editor only
    public float GetDetectRadius() { return detectRadius; }
    public float GetAttackRadius() { return attackRadius; }
    public List<Vector3> GetPatrolPoints() { return patrolPoints; }
    public void SetPatrolPoints(List<Vector3> updatedPatrolPoints) { patrolPoints = updatedPatrolPoints; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        if (Application.isPlaying)
        {
            if (patrolPoints.Count > 1) Gizmos.DrawLine(patrolPoints[0], patrolPoints[patrolPoints.Count - 1]);
        }
        else
        {
            if (patrolPoints.Count > 0) Gizmos.DrawLine(transform.position, patrolPoints[0]);
        }
        for (int i = 0; i < patrolPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(patrolPoints[i], patrolPoints[i + 1]);
        }
    }
}
