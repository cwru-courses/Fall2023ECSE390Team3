using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PhaseShift : MonoBehaviour
{
    public static PhaseShift _instance;

    [SerializeField] private float shiftCD;
    [SerializeField] private float shiftPrecastTime;
    [SerializeField] private float precastingTimer;
    [SerializeField] private Slider shiftProgressBar;

    [SerializeField] private Image uiImg;

    private DefaultInputAction playerInputAction;

    private float shiftCDTimer;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.PhaseShift.performed += StartPhaseShift;

        shiftProgressBar.gameObject.SetActive(false);

        uiImg.fillAmount = 0;
    }

    private void OnEnable()
    {
        playerInputAction.Player.PhaseShift.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.PhaseShift.Disable();
    }

    void FixedUpdate()
    {
        shiftCDTimer = Mathf.Max(shiftCDTimer - Time.fixedDeltaTime, 0);
    }

    void LateUpdate()
    {
        uiImg.fillAmount = shiftCDTimer / shiftCD;
    }

    private void StartPhaseShift(InputAction.CallbackContext ctx)
    {
        if (shiftCDTimer <= 0) {
            StartCoroutine("PhaseShiftPrecast");
            OnDisable();    // Disable input to avoid more than one click
        }
    }

    private void ToPhaseShift()
    {
        // Move character into the alternate world
        Vector3 currentLocation = transform.position;
        currentLocation.y *= -1;
        transform.position = currentLocation;

        shiftCDTimer = shiftCD;

        OnEnable();     // Enable input again

        // Added this line to toggle emission of yarn trail -- Jing
        AmbientSystem.Instance.OnPhaseShift();
        YarnTrail._instance.toggleEmission();

    }

    IEnumerator PhaseShiftPrecast()
    {
        shiftProgressBar.gameObject.SetActive(true);

        precastingTimer = shiftPrecastTime;
        while (precastingTimer > 0)
        {
            shiftProgressBar.value = precastingTimer / shiftPrecastTime;
            precastingTimer -= Time.deltaTime;
            yield return null;
        }

        shiftProgressBar.gameObject.SetActive(false);

        ToPhaseShift();
    }

    public void OnPause(bool paused)
    {
        if (paused)
        {
            playerInputAction.Player.PhaseShift.Disable();
        }
        else
        {
            playerInputAction.Player.PhaseShift.Enable();
        }
    }
}
