using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrainingBotController : BaseEnemy
{
    [Header("Attack Settings")]
    [SerializeField] protected float attackRadius;
    [SerializeField] private float attackCD;
    [SerializeField] protected float attackAnimDuration;
    [Header("Patrol Path Settings")]
    [SerializeField] private List<Vector3> patrolPoints;
    [SerializeField] private float patrolCD;
    [Header("Graphics Settings")]
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] protected Animator anim;
    [SerializeField] private Color colorOnDeath;

    private float lastAttackTime;
    private int patrolTargetIndex;
    private float patrolCDTimer;
    protected Transform targetTransform;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        alive = true;
        rb2d = GetComponent<Rigidbody2D>();

        lastAttackTime = 0;
        patrolTargetIndex = 0;
        patrolPoints.Add(transform.position);
        patrolCDTimer = 0;
        targetTransform = null;
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
                    rb2d.velocity = targetDist.normalized * movementSpeed * movementSpeedModifier;
                }
                else
                {
                    if (Time.time - lastAttackTime > attackCD)
                    {
                        StartCoroutine(Attack());
                        lastAttackTime = Time.time;
                    }
                }
                //if too close run away
                if (targetDist.magnitude < attackRadius*0.7f)
                {
                    rb2d.velocity = targetDist.normalized * movementSpeed * movementSpeedModifier * -1f;
                }

            }
            else
            {
                targetDist = patrolPoints[patrolTargetIndex] - transform.position;
                if (patrolCDTimer == 0)
                {
                    if (targetDist.magnitude > 0.1f)
                    {
                        rb2d.velocity = Vector3.ClampMagnitude(
                                targetDist.normalized * movementSpeed * Time.fixedDeltaTime * movementSpeedModifier,
                                targetDist.magnitude
                        );
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
                StopAllCoroutines();
                StartCoroutine(Die());
            }
        }
    }

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

    protected virtual IEnumerator Attack()
    {
        if (anim)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(attackAnimDuration);
            anim.ResetTrigger("Attack");
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
    }
}
