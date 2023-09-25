using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAttack : MonoBehaviour
{
    [Range(0f, 180f)]
    [SerializeField] private float attackRangeAngle;
    [Min(0)]
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackRecoveryTime;  // Time between swings
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private float attackSwingTime;  // Duration of swing animation(temporary until real animation exists)
    [SerializeField] private GameObject weaponPrefab;

    [SerializeField] private AudioSource attackSFX;

    private DefaultInputAction playerInputAction;

    private Vector2 lookAtDir;  // Note that this direction is NOT normalized. Vector length = distance from the target point
    private float attackAngleCosVal;
    private float lastAttackTime;
    private bool isAttacking;
    private GameObject weaponObject;  // Used for temporary attack animation

    private Player player; 

    void Awake()
    {
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); 
        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.Attack.started += Attack;

        lookAtDir = Vector2.right;  // Default starting direction
        attackAngleCosVal = Mathf.Cos(attackRangeAngle / 2f);
        lastAttackTime = 0f;
        isAttacking = false;
        weaponObject = null;
    }

    private void OnEnable()
    {
        playerInputAction.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastAttackTime > attackRecoveryTime) { isAttacking = false; }
        //Temporary attack effects
        if (isAttacking && weaponObject)
        {
            // rotate weapon object
            float swingPercent = (Time.deltaTime) / attackSwingTime;
            weaponObject.transform.Rotate(0f, 0f, swingPercent * attackRangeAngle);
            weaponObject.transform.position = transform.position;
        }

        // Direction Based on last movement direction

        lookAtDir = PlayerMovement.Instance.GetLastDirection();

        // Direction Based on cursor Position

        //Ray viewRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //lookAtDir = viewRay.origin + viewRay.direction * Mathf.Abs(viewRay.origin.z / viewRay.direction.z) - transform.position;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        // can only swing if you have waited for the recovery time since last swing
        if (Time.time - lastAttackTime > attackRecoveryTime)
        {
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
            isAttacking = true;
            lastAttackTime = Time.time; // update last Attack time
            StartCoroutine(AttackFX()); // attack visual effects
            player.UseYarn(5); 
        }
    }

    //temporary implementation until animations are made
    private IEnumerator AttackFX()
    {
        //Plays SFX
        if (attackSFX) { attackSFX.Play(); }

        //check if there is a weapon prefab to instantiate
        if (weaponPrefab)
        {
            //creates object to be rotated
            weaponObject = Instantiate(weaponPrefab) as GameObject;
            weaponObject.transform.position = transform.position;

            //rotate weapon to be infront of player
            Vector3 rot = weaponObject.transform.rotation.eulerAngles;
            Vector2 moveDir = PlayerMovement.Instance.GetLastDirection();
            float offset = attackRangeAngle / 2f;
            rot.z = Mathf.Acos(Vector2.Dot(Vector2.up, moveDir)) * Mathf.Rad2Deg;
            if (moveDir.x > 0) { rot.z *= -1f; }
            rot.z -= offset;
            weaponObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }

        //wait for attack duration
        yield return new WaitForSeconds(attackSwingTime);

        if (weaponObject)
        {
            //after swing time delete the weapon object
            Destroy(weaponObject);
            isAttacking = false;
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

    //private void OnDrawGizmos()  // Remove when release.
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawLine(transform.position, lookAtPoint);
    //}
}
