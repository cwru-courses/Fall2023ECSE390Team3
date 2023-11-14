using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement _instance;

    [SerializeField] private float runSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCD;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audSource;


    private DefaultInputAction playerInputAction;

    private Rigidbody2D rb2d;

    private float verticalSpeedMultiplier;
    private bool onDash;
    private float dashTimer;
    private float lastDashTime;
    private Vector2 lastMovementDir;  // Tracking last direction player moved
    private float speedMultiplier; // used to adjust player speed during abilities etc
    private bool movementLocked; // keep track of whether player movement is locked
    private bool isMoving = false;

    private Vector3 FLIPPEDROTATION = new Vector3(0, 180, 0);
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new DefaultInputAction();
        //playerInputAction.Player.Dash.started += Dash;

        rb2d = GetComponent<Rigidbody2D>();
        onDash = false;
        dashTimer = 0f;
        lastDashTime = -dashCD;
        lastMovementDir = Vector2.right;
        speedMultiplier = 1f;
        movementLocked = false;
        audSource = GetComponent<AudioSource>();
        audSource.mute = true;
        audSource.loop = true;
    }

    private void Update()
    {
        // if player in flipped world and run out of yarn and movement not yet locked, lock(pause) the movement -- Jing
        if (PlayerStats._instance.inFlippedWorld && PlayerStats._instance.currentYarnCount <= 0 && !movementLocked)
        {
            OnPause(true);
            movementLocked = true;
        }
        // else if the movement is still locked, unlock the movement
        else if (movementLocked)
        {
            OnPause(false);
            movementLocked = false;
        }
    }

    private void Start()
    {
        verticalSpeedMultiplier = AmbientSystem.Instance.GetVerticalDistMultiplier();
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
        if (inputDir.y > 0 || inputDir.x < 0)
        {
            //anim.transform.eulerAngles = FLIPPEDROTATION;
        }
        else
        {
           // anim.transform.eulerAngles = Vector3.zero;
        }

        //update lastMovementDir only if moving
        if (inputDir.magnitude != 0)
        {
            lastMovementDir = inputDir.normalized;
            if (audSource.mute == true)
            {
                audSource.mute = false;
                audSource.Play();
            }
            if (anim)
            {
                anim.SetFloat("xVel", inputDir[0]);
                anim.SetFloat("yVel", inputDir[1]);
                anim.SetFloat("lastXVel", inputDir[0]);
                anim.SetFloat("lastYVel", inputDir[1]);
                if (!isMoving)
                {
                    anim.SetBool("isMoving", true);
                }
            }
            isMoving = true;
        }
        else
        {
            if (anim)
            {
                anim.SetFloat("xVel", inputDir[0]);
                anim.SetFloat("yVel", inputDir[1]);
                if (isMoving)
                {
                    anim.SetBool("isMoving", false);
                }
            }
            isMoving = false;
            if (audSource.mute == false)
                audSource.mute = true;

        }

        if (onDash)
        {
            rb2d.velocity = inputDir * dashSpeed * speedMultiplier;
            dashTimer += Time.fixedDeltaTime;

            if (dashTimer > dashDuration)
            {
                onDash = false;
                PlayerStats._instance.blocking = false; ;
            }
        }
        else
        {
            rb2d.velocity = inputDir * runSpeed * speedMultiplier;
        }
    }

    public void Dash(InputAction.CallbackContext ctx)
    {
        if (Time.time - lastDashTime > dashCD)
        {
            PlayerStats._instance.blocking = true;
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

    public void MultiplySpeed(float multiplier)
    {
        speedMultiplier *= multiplier;
    }
}
