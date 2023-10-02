using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement _instance;

    [Range(0f, 1f)]
    [SerializeField] private float verticalSpeedMultiplier;
    [SerializeField] private float runSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCD;

    private DefaultInputAction playerInputAction;

    private Rigidbody2D rb2d;

    private bool onDash;
    private float dashTimer;
    private float lastDashTime;
    private Vector2 lastMovementDir;  // Tracking last direction player moved

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.Dash.started += Dash;

        rb2d = GetComponent<Rigidbody2D>();
        onDash = false;
        dashTimer = 0f;
        lastDashTime = -dashCD;
        lastMovementDir = Vector2.right;
    }

    private void OnEnable()
    {
        playerInputAction.Player.Movement.Enable();
        playerInputAction.Player.Dash.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Movement.Disable();
        playerInputAction.Player.Dash.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 inputDir = playerInputAction.Player.Movement.ReadValue<Vector2>().normalized;
        inputDir.y *= verticalSpeedMultiplier;

        //update lastMovementDir only if moving
        if (inputDir.magnitude != 0)
        {
            lastMovementDir = inputDir.normalized;
        }

        if (onDash)
        {
            rb2d.velocity = inputDir * dashSpeed;
            dashTimer += Time.fixedDeltaTime;

            if (dashTimer > dashDuration) { onDash = false; }
        }
        else
        {
            rb2d.velocity = inputDir * runSpeed;
        }
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        if (Time.time - lastDashTime > dashCD)
        {
            onDash = true;
            dashTimer = 0f;
            lastDashTime = Time.time;
        }
    }

    public Vector2 GetLastDirection()
    {
        return lastMovementDir;
    }

    public void OnPause(bool paused)
    {
        if (paused)
        {
            playerInputAction.Player.Movement.Disable();
            playerInputAction.Player.Dash.Disable();
        }
        else
        {
            playerInputAction.Player.Movement.Enable();
            playerInputAction.Player.Dash.Enable();
        }
    }
}
