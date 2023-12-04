using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack _instance;

    [Range(0f, 180f)]
    [SerializeField] private float attackRangeAngle;
    [Min(0)]
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackCD;  // Time between swings
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private Transform weaponHolder;

    [Min(0)]
    [SerializeField] private int projectileNum;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioSource swingSFX;
    [SerializeField] private Animator anim;

    //private DefaultInputAction playerInputAction;

    private Vector2 lookAtDir;  // Note that this direction is NOT normalized. Vector length = distance from the target point
    private float attackAngleCosVal;
    private float lastAttackTime;
    private bool attackEnabled = true;

    [SerializeField] private GameObject weaponHolderAnim;

    private ProjectileController[] projectiles;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        //playerInputAction = new DefaultInputAction();
        //playerInputAction.Player.Attack.started += Attack;
        //playerInputAction.Player.LaunchProjectile.started += LaunchProjectile;

        lookAtDir = Vector2.left;  // Default starting direction
        attackAngleCosVal = Mathf.Cos(attackRangeAngle / 2f);
        lastAttackTime = -attackCD;

        //weaponHolderAnim = weaponHolder.GetComponent<Animation>();

        projectiles = new ProjectileController[projectileNum];
        for (int i = 0; i < projectileNum; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectiles[i] = projectile.GetComponent<ProjectileController>();
            projectile.SetActive(false);
        }
    }

    //private void OnEnable()
    //{
    //    playerInputAction.Player.Attack.Enable();
    //    playerInputAction.Player.LaunchProjectile.Enable();
    //}

    //private void OnDisable()
    //{
    //    playerInputAction.Player.Attack.Disable();
    //    playerInputAction.Player.LaunchProjectile.Disable();
    //}

    // Update is called once per frame
    void Update()
    {
        // Direction Based on cursor Position
        Ray viewRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        lookAtDir = viewRay.origin + viewRay.direction * Mathf.Abs(viewRay.origin.z / viewRay.direction.z) - transform.position;
        lookAtDir = lookAtDir.normalized;

        float lookAtDirAngle = Mathf.Acos(Vector2.Dot(Vector2.up, lookAtDir)) * 57.3f;
        if (lookAtDir.x < 0) lookAtDirAngle = 360f - lookAtDirAngle;
        weaponHolder.rotation = Quaternion.Euler(0f, 0f, -lookAtDirAngle);
    }

    public void EnableAttack(bool enabled)
    {
        attackEnabled = enabled;
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (attackEnabled && Time.time - lastAttackTime > attackCD)
        {
            RaycastHit2D[] inRangeColliderHits = Physics2D.CircleCastAll(
                transform.position,
                attackRadius,
                Vector2.zero,
                0f,
                whatIsEnemy
            );

            if(inRangeColliderHits.Length == 0)
            {
                if (swingSFX)
                {
                    swingSFX.Play();
                }
            }

            foreach (RaycastHit2D hit in inRangeColliderHits)
            {
                if (Vector2.Angle((hit.point - hit.centroid).normalized, lookAtDir.normalized) < attackRangeAngle)
                {
                    BaseEnemy enemyController = hit.collider.GetComponent<BaseEnemy>();
                    if (enemyController)
                    {
                        enemyController.ReactToHit(1);

                        for (int i = 0; i < projectileNum; i++)
                        {
                            if (hit.transform == projectiles[i].GetTargetTransform()) { projectiles[i].Detach(); }
                        }
                    }
                }
            }
            //weaponHolderAnim.Play();
            lastAttackTime = Time.time; // update last Attack time
            StartCoroutine(AttackFX()); // attack visual effects
            PlayerStats._instance.UseYarn(5);
        }
    }

    //temporary implementation until animations are made
    private IEnumerator AttackFX()
    {
        GameObject WeaponAnimation;
        if (weaponHolderAnim)
        {
            WeaponAnimation = Instantiate<GameObject>(weaponHolderAnim);
            WeaponAnimation.transform.parent = this.transform;
            WeaponAnimation.transform.position = this.transform.position;
            WeaponAnimation.transform.rotation = weaponHolder.rotation;
        }
        else
        {
            WeaponAnimation = new GameObject("temp_attack_effect");
            WeaponAnimation.transform.parent = this.transform;
            WeaponAnimation.transform.position = this.transform.position;
        }
        weaponHolder.gameObject.SetActive(false);
        if (anim) {
            //print("set attack trigger");
            anim.SetBool("isAttacking",true);
        }
        yield return new WaitForSeconds(attackCD);
        Destroy(WeaponAnimation);
        if (anim) {
            //print("attack trigger reset");
            anim.SetBool("isAttacking",false);
        }
        weaponHolder.gameObject.SetActive(true);
    }

    public void LaunchProjectile(InputAction.CallbackContext ctx)
    {
        for (int i = 0; i < projectileNum; i++)
        {
            if (projectiles[i].IsAvailable())
            {
                projectiles[i].Launch(transform.position, lookAtDir);
                break;
            }
        }
    }

    // For gizmos & handle only. Remove when release.
    public Vector3 GetLookAtDir()
    {
        return lookAtDir;
    }

    // For gizmos & handle only. Remove when release.
    public float GetAttackAngleH()
    {
        return attackRangeAngle / 2f;
    }

    // For gizmos & handle only. Remove when release.
    public float GetAttackRad()
    {
        return attackRadius;
    }

    //public void OnPause(bool paused)
    //{
    //    if (paused)
    //    {
    //        playerInputAction.Player.Attack.Disable();
    //        playerInputAction.Player.LaunchProjectile.Disable();
    //    }
    //    else
    //    {
    //        playerInputAction.Player.Attack.Enable();
    //        playerInputAction.Player.LaunchProjectile.Enable();
    //    }
    //}
}
