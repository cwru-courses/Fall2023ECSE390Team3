using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
public class Boss2Controller : BaseEnemy
{
    [Header("Attack Settings")]
    [SerializeField] private float pullAttackRadius;
    [SerializeField] private float pullAttackProjectileSpeed;
    [SerializeField] private int pullAttackProjectileDamage;
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
    [SerializeField] private Material pullLineMaterial;
    [SerializeField] private AudioSource pullSFX;

    [Header("Projectile Settings")]
    [SerializeField] private int numProjectiles;
    [SerializeField] private float totalProjectileAngle;
    [SerializeField] private int numProjectileWaves;
    [SerializeField] private float timeBetweenProjectileWaves;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamage;
    [SerializeField] private AudioSource projectileSFX;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Patrol Path Settings")]
    [SerializeField] private List<Vector3> patrolPoints;
    [SerializeField] private float patrolCD;

    [Header("Random Movement Settings")]
    [SerializeField] private float directionDuration;


    [Header("Graphics Settings")]
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] protected Animator anim;
    [SerializeField] private Color colorOnDeath;
    [SerializeField] private GameObject bossHealthbar;
    [SerializeField] private GameObject riftPrefab;
    [SerializeField] private float riftDuration;
    [SerializeField] private float stitchAnimLength;
    [SerializeField] private AudioSource stitchInSFX;
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
    private float lastDirectionChangeTime;
    private Vector3[] movementDirections;
    private Vector3 currMovementDirection;
    private GameObject weaponObject;

    public static Boss2Controller _instance;

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
        lastDirectionChangeTime = Time.time;

        healthbarInitScale = bossHealthbar.transform.localScale.x;

        GameObject pathRenderObj = new GameObject("PullLineRenderer", typeof(LineRenderer));
        pathRenderObj.transform.position = Vector3.zero;
        pullLine = pathRenderObj.GetComponent<LineRenderer>();
        pullLine = pathRenderObj.GetComponent<LineRenderer>();
        pullLine.startWidth = 0.1f;
        pullLine.endWidth = 0.1f;
        pullLine.material = pullLineMaterial;
        pullLine.startColor = Color.white;
        pullLine.endColor = Color.white;

        movementDirections = new Vector3[8];
        movementDirections[0] = Vector3.down;
        movementDirections[1] = Vector3.up;
        movementDirections[2] = Vector3.right;
        movementDirections[3] = Vector3.left;
        movementDirections[4] = (new Vector3(1, 1, 0)).normalized;
        movementDirections[5] = (new Vector3(1, -1, 0)).normalized;
        movementDirections[6] = (new Vector3(-1, 1, 0)).normalized;
        movementDirections[7] = (new Vector3(-1, -1, 0)).normalized;

        currMovementDirection = movementDirections[0];

        if(_instance == null){
            _instance = this;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (alive)
        {
            Vector3 targetDist;
            // Attack Scripts
            if (targetTransform)
            {
                targetDist = targetTransform.position - transform.position;


                //if cooldown done then attack, note: attackCD reduces with HP
                if (Time.time - lastAttackTime > minAttackCD + ((float)health / (float)maxHealth) * minAttackCD && !isStunned)
                {

                    //closer player is the more likely to slam instead of pull
                    if (targetDist.magnitude < slamAttackRadius)
                    {
                        if (Random.value > 0.5)
                        {
                            lastAttackTime = Time.time;
                            print("shockwave attack");
                            StartCoroutine(ShockwaveAttack());
                        }
                        else
                        {
                            lastAttackTime = Time.time;
                            print("projectile attack");
                            StartCoroutine(ProjectileAttack());
                        }
                        

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
                        targetTransform.position -= 5 * targetDist * Time.fixedDeltaTime / pullTime;
                        Vector3[] pullLinePoints = new Vector3[2];
                        pullLinePoints[0] = transform.position;
                        pullLinePoints[1] = targetTransform.position;

                        pullLine.positionCount = 2;
                        pullLine.SetPositions(pullLinePoints);
                    }

                }

            }
            // Movement Scripts
            if (targetTransform)
            {
                if(Time.time-lastDirectionChangeTime> directionDuration)
                {
                    lastDirectionChangeTime = Time.time;
                    targetDist = targetTransform.position - transform.position;
                    float[] inverseAngles = new float[movementDirections.Length];
                    float sumInverseAngles = 0;
                    float normalizingConst = 10;
                    for (int i = 0; i < movementDirections.Length; i++)
                    {
                        inverseAngles[i] = 1 / (normalizingConst + Mathf.Abs(Vector3.Angle(targetDist, movementDirections[i])));
                        sumInverseAngles += inverseAngles[i];
                    }
                    float randValue = Random.value * sumInverseAngles;
                    float currMin = 0;
                    for(int i = 0; i < movementDirections.Length; i++)
                    {
                        if (randValue < currMin + inverseAngles[i])
                        {
                            currMovementDirection = movementDirections[i];
                            print("new Movement Direction = "+ currMovementDirection.ToString());
                            rb2d.velocity = currMovementDirection.normalized * movementSpeed * movementSpeedModifier;
                            break;
                        }
                        currMin += inverseAngles[i];
                    }
                }
                
                
            }
            else
            {
                if (transform.position.y < 0)
                {
                    Vector3 movementTargetDist = patrolPoints[patrolTargetIndex];
                    movementTargetDist.y *= -1f;
                    movementTargetDist -= transform.position;
                    if (patrolCDTimer == 0)
                    {
                        if (movementTargetDist.magnitude > 0.1f)
                        {
                            rb2d.velocity = movementTargetDist.normalized * movementSpeed * movementSpeedModifier;

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
                    Vector3 movementTargetDist = patrolPoints[patrolTargetIndex] - transform.position;
                    if (patrolCDTimer == 0)
                    {
                        if (movementTargetDist.magnitude > 0.1f)
                        {
                            rb2d.velocity = movementTargetDist.normalized * movementSpeed * movementSpeedModifier;

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
            }
            


            //check if player is in detect radius
            Collider2D targetCollider = Physics2D.OverlapCircle(transform.position, detectRadius, whatIsTaget);
            if (targetCollider) 
            { 
                targetTransform = targetCollider.transform; 
            }
            else
            {
                targetTransform = null;
            }



        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }

    }

    public override void ReactToHit(int damage)
    {
        if (alive)
        {
            health = Mathf.Max(health - damage, 0);
            bossHealthbar.transform.localScale = new Vector3(healthbarInitScale * ((float)health / (float)maxHealth), (float)8.15, 1);
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
        for (int i = 0; i < numSpawnEnemies; i++)
        {
            //spawn random enemy at random location
            GameObject enemy = Instantiate<GameObject>(enemyPrefabs[(int)(Random.value * enemyPrefabs.Length)]);
            Vector3 enemyPos = transform.position;
            enemyPos.x += (Random.value * spawnRadius) - (spawnRadius / 2f);
            enemyPos.y += (Random.value * spawnRadius) - (spawnRadius / 2f);
            enemy.transform.position = enemyPos;
            TrainingBotController enemyController = enemy.GetComponent<TrainingBotController>();
            enemyController.setTargetTransform(targetTransform);
        }
    }

    private IEnumerator ShockwaveAttack()
    {
        //loop for number of shockwaves
        for (int i = 0; i < numWaves; i++)
        {
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
            //shockwaveObject.transform.parent = transform;
            shockwaveObject.transform.position = transform.position;

            Shockwave sw = shockwaveObject.GetComponent<Shockwave>();
            sw.damage = shockwaveDamage;
            sw.targetLayer = whatIsTaget;
            sw.speed = shockwaveSpeed;
            sw.hitRadius = 0.5f;
            yield return new WaitForSeconds(timeBetweenWaves - slamAnimLength);
        }

    }

    private IEnumerator PullAttack()
    {
        print("entered pull attack");
        // insert wind up animation here
        anim.SetBool("isPulling", true);
        pullSFX.Play();

        yield return new WaitForSeconds(windUpTimePull);
        anim.SetBool("isPulling", false);
        //check if player is in range
        Collider2D collider = Physics2D.OverlapCircle(transform.position, pullRange, whatIsTaget);
        if (collider)
        {
            print("FIRING");
            //Send projectile
            weaponObject = Instantiate(pullPrefab) as GameObject;
            weaponObject.transform.position = transform.position;

            //rotate weapon to be towards the player
            Vector3 rot = weaponObject.transform.rotation.eulerAngles;

            // division of basicAttackRange is to keep two numbers below 1 to avoid an error message saying Assertion failed on expression
            float xDirection = (PlayerStats._instance.transform.position.x - transform.position.x);
            float yDirection = (PlayerStats._instance.transform.position.y - transform.position.y);
            //Debug.Log("xDirection: " + xDirection + "; yDirection: " + yDirection);
            Vector2 moveDir = new Vector2(xDirection, yDirection);
            moveDir = moveDir.normalized;
            rot.z = Mathf.Acos(Vector2.Dot(Vector2.up, moveDir)) * Mathf.Rad2Deg;
            if(moveDir.x > 0) { rot.z *= -1f; }
            weaponObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);

            Projectile projectile = weaponObject.GetComponent<Projectile>();
            projectile.direction = moveDir;
            projectile.damage = pullAttackProjectileDamage;
            projectile.attackLayer = 15;
            projectile.triggerBossPull2 = true;
        }
    }

    //method to be called by the projectile created in PullAttack
    public IEnumerator TriggerPull()
    {
        print("PULLING TARGET");
        pullingPlayer = true;

        Collider2D collider = Physics2D.OverlapCircle(transform.position, pullRange, whatIsTaget);
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

    protected IEnumerator ProjectileAttack()

    {
        for(int i = 0; i < numProjectileWaves; i++)
        {
            // play sfx
            if (projectileSFX)
            {
                projectileSFX.Play();
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
            for(int j = 0; j < numProjectiles; j++)
            {
                //check if there is a weapon prefab to instantiate
                if (projectilePrefab)
                {

                    //print("needle instantiated");
                    //creates object to be rotated
                    GameObject weaponObject = Instantiate(projectilePrefab) as GameObject;
                    weaponObject.transform.position = transform.position;

                    //rotate weapon to be towards the player
                    Vector3 rot = weaponObject.transform.rotation.eulerAngles;

                    // division of basicAttackRange is to keep two numbers below 1 to avoid an error message saying Assertion failed on expression
                    float xDirection = (PlayerStats._instance.transform.position.x - transform.position.x);
                    float yDirection = (PlayerStats._instance.transform.position.y - transform.position.y);
                    //Debug.Log("xDirection: " + xDirection + "; yDirection: " + yDirection);
                    Vector2 moveDir = new Vector2(xDirection, yDirection);
                    moveDir = moveDir.normalized;
                    rot.z = Mathf.Acos(Vector2.Dot(Vector2.up, moveDir)) * Mathf.Rad2Deg;
                    if (moveDir.x > 0) { rot.z *= -1f; }
                    rot.z -= totalProjectileAngle/2;
                    rot.z += (totalProjectileAngle / numProjectiles - 1) * j;
                    weaponObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);

                    Projectile projectile = weaponObject.GetComponent<Projectile>();
                    Vector3 projectileDir = (targetTransform.position - transform.position).normalized;
                    float projAngle = Mathf.Rad2Deg*Mathf.Acos(projectileDir.x);
                    if (projectileDir.y < 0) { projAngle *= -1; }
                    projAngle -= totalProjectileAngle / 2;
                    projAngle += (totalProjectileAngle / numProjectiles - 1) * j;
                    projAngle *= Mathf.Deg2Rad;
                    projectileDir = new Vector3(Mathf.Cos(projAngle), Mathf.Sin(projAngle), 0);
                    projectile.direction = projectileDir;
                    projectile.damage = projectileDamage;
                    projectile.speed = projectileSpeed;
                    projectile.attackLayer = 15;
                }
            }
            yield return new WaitForSeconds(timeBetweenProjectileWaves - slamAnimLength);
        }
    }

    IEnumerator switchRealities()
    {
        //GameObject riftObject = Instantiate<GameObject>(riftPrefab);
        Vector3 currPosition = transform.position;
        currPosition.y *= -1;
        stitchInSFX.Play();
        anim.SetBool("isStitching", true);
        yield return new WaitForSeconds(stitchAnimLength);
        anim.SetBool("isStitching", false);
        //riftObject.transform.position = transform.position;
        transform.position = currPosition;
        yield return new WaitForSeconds(riftDuration);
        //Destroy(riftObject);

    }

    private IEnumerator FlashColor(Color color)
    {
        spriteRender.color = color;
        yield return new WaitForSeconds(0.3f);
        spriteRender.color = Color.white;
    }
}
