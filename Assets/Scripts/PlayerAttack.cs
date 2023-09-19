using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Range(0f, 180f)]
    [SerializeField] private float attackRangeAngle;
    [Min(0)]
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask whatIsEnemy;

    private DefaultInputAction playerInputAction;

    private Vector2 lookAtDir;  // Note that this direction is NOT normalized. Vector length = distance from the target point
    private float attackAngleCosVal;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.Attack.started += Attack;

        lookAtDir = Vector2.zero;
        attackAngleCosVal = Mathf.Cos(attackRangeAngle / 2f);
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
        Ray viewRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        lookAtDir = viewRay.origin + viewRay.direction * Mathf.Abs(viewRay.origin.z / viewRay.direction.z) - transform.position;
    }

    private void Attack(InputAction.CallbackContext ctx)
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
            Debug.Log(hit.centroid);
            if (Vector2.Dot((hit.point - hit.centroid).normalized, lookAtDir.normalized) > attackAngleCosVal)
            {
                BaseEnemy enemyController = hit.collider.GetComponent<BaseEnemy>();
                if (enemyController)
                {
                    enemyController.ReactToHit(1);
                }
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

    //private void OnDrawGizmos()  // Remove when release.
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawLine(transform.position, lookAtPoint);
    //}
}
