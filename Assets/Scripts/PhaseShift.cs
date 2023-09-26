using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PhaseShift : MonoBehaviour
{
    public static PhaseShift Instance;

    [SerializeField] private KeyCode switchKey = KeyCode.Q;
    [SerializeField] private float shiftCD;
    [SerializeField] private float shiftPrecastTime;
    [SerializeField] private Slider shiftProgressBar;
    [SerializeField] private float precastingTimer;

    private DefaultInputAction playerInputAction;

    private float shiftCDTimer;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.PhaseShift.performed += StartPhaseShift;

        shiftProgressBar.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerInputAction.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Disable();
    }

    void FixedUpdate()
    {
        shiftCDTimer = Mathf.Max(shiftCDTimer - Time.fixedDeltaTime, 0);
    }

    private void StartPhaseShift(InputAction.CallbackContext ctx)
    {
        if (shiftCDTimer <= 0) StartCoroutine("PhaseShiftPrecast");
    }

    private void ToPhaseShift()
    {
        // Move character into the alternate world
        Vector3 currentLocation = transform.position;
        currentLocation.y *= -1;
        transform.position = currentLocation;

        shiftCDTimer = shiftCD;
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

    public float GetCDPercentage()
    {
        return shiftCDTimer / shiftCD;
    }
}
