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

    [SerializeField] private float attackSwingTime;  // Duration of swing animation(temporary until real animation exists)
    [SerializeField] private Transform weaponHolder;

    private DefaultInputAction playerInputAction;

    private Vector2 lookAtDir;  // Note that this direction is NOT normalized. Vector length = distance from the target point
    private float attackAngleCosVal;
    private float lastAttackTime;

    private Animation weaponHolderAnim;
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.Attack.started += Attack;

        lookAtDir = Vector2.left;  // Default starting direction
        attackAngleCosVal = Mathf.Cos(attackRangeAngle / 2f);
        lastAttackTime = -attackCD;

        weaponHolderAnim = weaponHolder.GetComponent<Animation>();
    }

    private void OnEnable()
    {
        playerInputAction.Player.Attack.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Attack.Disable();
    }

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

    private void Attack(InputAction.CallbackContext ctx)
    {
        if (Time.time - lastAttackTime > attackCD)
        {
            weaponHolderAnim.Play();

            RaycastHit2D[] inRangeColliderHits = Physics2D.CircleCastAll(
                transform.position,
                attackRadius,
                Vector2.zero,
                0f,
                whatIsEnemy
            );

            foreach (RaycastHit2D hit in inRangeColliderHits)
            {
                if (Vector2.Dot((hit.point - hit.centroid).normalized, lookAtDir.normalized) > attackAngleCosVal)
                {
                    BaseEnemy enemyController = hit.collider.GetComponent<BaseEnemy>();
                    if (enemyController)
                    {
                        enemyController.ReactToHit(1);
                    }
                }
            }
            lastAttackTime = Time.time; // update last Attack time
            StartCoroutine(AttackFX()); // attack visual effects
            PlayerStats._instance.UseYarn(5);
        }   
    }

    //temporary implementation until animations are made
    private IEnumerator AttackFX()
    {
        yield return null;
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

    public void OnPause(bool paused)
    {
        if (paused)
        {
            playerInputAction.Player.Attack.Disable();
        }
        else
        {
            playerInputAction.Player.Attack.Enable();
        }
    }
}
