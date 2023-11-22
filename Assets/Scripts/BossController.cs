using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
public class BossController : BaseEnemy
{
    [Header("Attack Settings")]
    [SerializeField] private float pullAttackRadius;
    [SerializeField] private float slamAttackRadius;
    [SerializeField] private float minAttackCD;
    [SerializeField] private int numPhases;
    [SerializeField] protected AudioSource takeDamageSFX;


    [Header("Spawning Settings")]
    [SerializeField] protected GameObject[] enemyPrefabs;
    [SerializeField] protected int numSpawnEnemies;
    [SerializeField] protected float spawnRadius;


    [Header("Shockwave Settings")]
    [SerializeField] private GameObject shockwavePrefab;
    [SerializeField] private int numWaves;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float slamAnimLength;
    [SerializeField] private int shockwaveDamage;
    [SerializeField] private float shockwaveSpeed;
    [SerializeField] private float shockwaveRange;
    [SerializeField] private AudioSource shockwaveSFX;

    [Header("Pull Settings")]
    [SerializeField] private GameObject pullPrefab;
    [SerializeField] private float windUpTimePull;
    [SerializeField] private float pullTime;
    [SerializeField] private float pullRange;
    [SerializeField] private float pullMinDist;

    [Header("Patrol Path Settings")]
    [SerializeField] private List<Vector3> patrolPoints;
    [SerializeField] private float patrolCD;
    [Header("Graphics Settings")]
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] protected Animator anim;
    [SerializeField] private Color colorOnDeath;
    [SerializeField] private GameObject bossHealthbar;
    [SerializeField] private GameObject riftPrefab;
    [SerializeField] private float riftDuration;
    [Header("Scene Load Settings")]
    [SerializeField] private string nextScene;

    private float lastAttackTime;
    private int patrolTargetIndex;
    private float patrolCDTimer;
    protected Transform targetTransform;
    private int currPhase;
    private bool pullingPlayer;
    private LineRenderer pullLine;
    private float healthbarInitScale;


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
        currPhase = 0;
        pullingPlayer = false;
        movementSpeedModifier = 1f;

        healthbarInitScale = bossHealthbar.transform.localScale.x;

        GameObject pathRenderObj = new GameObject("PullLineRenderer", typeof(LineRenderer));
        pathRenderObj.transform.position = Vector3.zero;
        pullLine = pathRenderObj.GetComponent<LineRenderer>();
        pullLine = pathRenderObj.GetComponent<LineRenderer>();
        pullLine.startWidth = 0.1f;
        pullLine.endWidth = 0.1f;
        pullLine.material = new Material(Shader.Find("Sprites/Default"));
        pullLine.startColor = Color.white;
        pullLine.endColor = Color.white;

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
                

                //if cooldown done then attack, note: attackCD reduces with HP
                if (Time.time - lastAttackTime > minAttackCD + ((float)health/(float)maxHealth)*minAttackCD && !isStunned)
                {
                    
                    //closer player is the more likely to slam instead of pull
                    if (Random.value > targetDist.magnitude / slamAttackRadius)
                    {
                        lastAttackTime = Time.time;
                        print("shockwave attack");
                        StartCoroutine(ShockwaveAttack());
                        
                    }
                    else
                    {
                        lastAttackTime = Time.time;
                        print("pull attack");
                        StartCoroutine(PullAttack());
                        
                    }
                    

                }

                // if pulling move player towards boss
                if (pullingPlayer)
                {
                    if (targetDist.magnitude < pullMinDist)
                    {
                        pullingPlayer = false;
                        pullLine.positionCount = 2;
                        pullLine.SetPositions(new Vector3[2]);
                    }
                    else
                    {
                        targetTransform.position -= 5* targetDist * Time.fixedDeltaTime / pullTime;
                        Vector3[] pullLinePoints = new Vector3[2];
                        pullLinePoints[0] = transform.position;
                        pullLinePoints[1] = targetTransform.position;

                        pullLine.positionCount = 2;
                        pullLine.SetPositions(pullLinePoints);
                    }
                    
                }

            }
            if (transform.position.y < 0)
            {
                targetDist = patrolPoints[patrolTargetIndex];
                targetDist.y *= -1f;
                targetDist -= transform.position;
                if (patrolCDTimer == 0)
                {
                    if (targetDist.magnitude > 0.1f)
                    {
                        rb2d.velocity = targetDist.normalized * movementSpeed * movementSpeedModifier;

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
            }
            else
            {
                targetDist = patrolPoints[patrolTargetIndex] - transform.position;
                if (patrolCDTimer == 0)
                {
                    if (targetDist.magnitude > 0.1f)
                    {
                        rb2d.velocity = targetDist.normalized * movementSpeed * movementSpeedModifier;

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
            }
            

            //check if player is in detect radius
            Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, detectRadius, whatIsTaget);
            if (targetCollider) targetTransform = targetCollider.transform;
            
            

        }
        else{
            SceneManager.LoadScene(nextScene);
        }

    }

    public override void ReactToHit(int damage)
    {
        if (alive)
        {
            health = Mathf.Max(health - damage, 0);
            bossHealthbar.transform.localScale = new Vector3(healthbarInitScale * ((float)health / (float)maxHealth), (float)8.15,1);
            takeDamageSFX.Play();
            if (spriteRender)
            {
                StartCoroutine(FlashColor(new Color(1f, 0.5f, 0.5f)));
            }
            if (health <= 0)
            {
                alive = false;
                StopAllCoroutines();
                StartCoroutine(Die());
            }
            else
            {
                // if we need to start a new phase
                print("health " + health);
                print("target health " + maxHealth * ((numPhases - currPhase - 1) / (float)numPhases));
                if (health < maxHealth * ((numPhases - currPhase - 1) / (float)numPhases))
                {
                    print("spawning enemies");
                    currPhase += 1;
                    SpawnEnemies();
                }
                else if (health < 1 + (maxHealth * ((numPhases - currPhase - 1) / (float)numPhases)) && health >= 2)
                {
                    StartCoroutine(switchRealities());
                }
            }
            
        }
    }

    // For editor only
    public float GetDetectRadius() { return detectRadius; }
    public float GetPullAttackRadius() { return pullAttackRadius; }
    public float GetSlamAttackRadius() { return slamAttackRadius; }
    public List<Vector3> GetPatrolPoints() { return patrolPoints; }
    public void SetPatrolPoints(List<Vector3> updatedPatrolPoints) { patrolPoints = updatedPatrolPoints; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.DrawWireSphere(transform.position, pullAttackRadius);
        Gizmos.DrawWireSphere(transform.position, slamAttackRadius);

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


    private void SpawnEnemies()
    {
        //print("spawn enemies triggered");
        for(int i = 0; i < numSpawnEnemies; i++)
        {
            //spawn random enemy at random location
            GameObject enemy = Instantiate<GameObject>(enemyPrefabs[(int)(Random.value * enemyPrefabs.Length)]);
            Vector3 enemyPos = transform.position;
            enemyPos.x += (Random.value * spawnRadius) - (spawnRadius / 2f);
            enemyPos.y += (Random.value * spawnRadius) - (spawnRadius / 2f);
            enemy.transform.position = enemyPos;
            TrainingBotController enemyController = enemy.GetComponent<TrainingBotController>();
            enemyController.setTargetTransform( targetTransform);
        }
    }

    private IEnumerator ShockwaveAttack()
    {
        

        

        //loop for number of shockwaves
        for(int i = 0; i < numWaves; i++) {
            // play sfx
            if (shockwaveSFX)
            {
                shockwaveSFX.Play();
            }
            if (anim)
            {
                anim.SetBool("isAttacking", true);
            }
            yield return new WaitForSeconds(slamAnimLength);
            if (anim)
            {
                anim.SetBool("isAttacking", false);
            }
            //spawn a shockwave
            GameObject shockwaveObject = Instantiate<GameObject>(shockwavePrefab);
            shockwaveObject.transform.parent = transform;
            shockwaveObject.transform.position = transform.position;
            
            Shockwave sw = shockwaveObject.GetComponent<Shockwave>();
            sw.damage = shockwaveDamage;
            sw.targetLayer = whatIsTaget;
            sw.speed = shockwaveSpeed;
            sw.hitRadius = 0.5f;
            yield return new WaitForSeconds(timeBetweenWaves-slamAnimLength);
        }

    }

    private IEnumerator PullAttack()
    {
        print("entered pull attack");
        // insert wind up animation here
        yield return new WaitForSeconds(windUpTimePull);

        //check if player is in range
        Collider2D collider = Physics2D.OverlapCircle(transform.position, pullRange, whatIsTaget);
        if (collider)
        {
            print("pull target in range");
            // check if can see player
            Vector3 direction = collider.transform.position - transform.position;
            direction = direction.normalized;
            Physics2D.queriesHitTriggers = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + (direction*5f), direction);
            Physics2D.queriesHitTriggers = true;
            print(hit.collider.gameObject.layer);
            // fix hit detection later
            if(hit.collider.gameObject.layer==15)
            {
                print("can see target");
                pullingPlayer = true;

                //indicate player is being pulled 
                GameObject pullObject = Instantiate<GameObject>(pullPrefab);
                pullObject.transform.parent = collider.gameObject.transform;
                pullObject.transform.position = collider.gameObject.transform.position;

                Vector3[] pullLinePoints = new Vector3[2];
                pullLinePoints[0] = transform.position;
                pullLinePoints[1] = collider.gameObject.transform.position;

                pullLine.positionCount = 2;
                pullLine.SetPositions(pullLinePoints);

                yield return new WaitForSeconds(pullTime);

                //done pulling
                Destroy(pullObject);
                pullingPlayer = false;
            }
        }
    }

    IEnumerator switchRealities()
    {
        GameObject riftObject = Instantiate<GameObject>(riftPrefab);
        Vector3 currPosition = transform.position;
        currPosition.y *= -1;
        riftObject.transform.position = transform.position;
        transform.position = currPosition;
        yield return new WaitForSeconds(riftDuration);
        Destroy(riftObject);
        
    }

    private IEnumerator FlashColor(Color color)
    {
        spriteRender.color = color;
        yield return new WaitForSeconds(0.3f);
        spriteRender.color = Color.white;
    }
}
