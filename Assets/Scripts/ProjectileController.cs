using UnityEngine;

enum ProjectileState
{
    Available = 0, Launched, Attached, Dropped
}

public class ProjectileController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] LayerMask whatIsTarget;

    private ProjectileState projectileState = ProjectileState.Available;
    private Transform targetTransform;
    private Vector3 targetOffset;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        targetOffset = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (projectileState)
        {
            case ProjectileState.Available:
                gameObject.SetActive(false);
                break;
            case ProjectileState.Launched:
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, speed * Time.fixedDeltaTime, whatIsTarget);
                if (hit.collider)
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        targetTransform = hit.transform;
                        targetOffset = new Vector3(hit.point.x, hit.point.y, hit.transform.position.z) - hit.transform.position;

                        projectileState = ProjectileState.Attached;
                    }
                    else
                    {
                        projectileState = ProjectileState.Dropped;
                    }
                    transform.position = hit.point;
                }
                else
                {
                    transform.position = transform.position + direction * speed * Time.fixedDeltaTime;
                }
                break;
            case ProjectileState.Attached:
                if (targetTransform) { transform.position = targetTransform.position + targetOffset; }
                else { projectileState = ProjectileState.Dropped; }
                break;
            case ProjectileState.Dropped:
                break;
        }
    }

    public void Launch(Vector3 startingPos, Vector2 dir)
    {
        transform.position = startingPos;
        direction = dir;

        projectileState = ProjectileState.Launched;

        gameObject.SetActive(true);
    }

    public bool IsAvailable()
    {
        return projectileState == ProjectileState.Available;
    }

    public Transform GetTargetTransform()
    {
        return targetTransform;
    }

    public void Detach()
    {
        projectileState = ProjectileState.Dropped;
    }
}
