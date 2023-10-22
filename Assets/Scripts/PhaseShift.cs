using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PhaseShift : MonoBehaviour
{
    public static PhaseShift _instance;

    [SerializeField] private float shiftCD;
    [SerializeField] private float shiftPrecastTime;
    [SerializeField] private Slider shiftProgressBar;
    [SerializeField] private GameObject rift;

    [SerializeField] private Image uiImg;

    private float precastingTimer;

    //private DefaultInputAction playerInputAction;

    private float shiftCDTimer;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        //playerInputAction = new DefaultInputAction();
        //playerInputAction.Player.PhaseShift.performed += StartPhaseShift;

        shiftProgressBar.gameObject.SetActive(false);

        uiImg.fillAmount = 0;
    }

    //private void OnEnable()
    //{
    //    playerInputAction.Player.PhaseShift.Enable();
    //}

    //private void OnDisable()
    //{
    //    playerInputAction.Player.PhaseShift.Disable();
    //}

    void FixedUpdate()
    {
        shiftCDTimer = Mathf.Max(shiftCDTimer - Time.fixedDeltaTime, 0);
    }

    void LateUpdate()
    {
        uiImg.fillAmount = shiftCDTimer / shiftCD;
    }

    public void StartPhaseShift(InputAction.CallbackContext ctx)
    {
        if (shiftCDTimer <= 0 && precastingTimer <= 0) {
            StartCoroutine("PhaseShiftPrecast");
            //OnDisable();    // Disable input to avoid more than one click
        }
    }

    private void ToPhaseShift()
    {
        // Move character into the alternate world
        Vector3 currentLocation = transform.position;
        
        // target position
        Vector3 targetPosition = new Vector3(currentLocation.x, -currentLocation.y, 0);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPosition, 0.5f);
        bool willCollide = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag.Equals("AvoidOverlap"))
            {
                willCollide = true;
            }
        }
        if (willCollide == true)
        {
            Vector3 foundLocation = findNearestLocation(targetPosition);
            if (foundLocation != Vector3.zero)
            {
                targetPosition = foundLocation;
                Debug.Log("shifted");
            }
            else
            {
                Debug.Log("fail to found neareast location");
            }
        }
        transform.position = targetPosition;


        shiftCDTimer = shiftCD;

        //OnEnable();     // Enable input again

        // Added this line to toggle emission of yarn trail -- Jing
        AmbientSystem.Instance.OnPhaseShift();
        YarnTrail._instance.toggleEmission();

    }

    Vector3 findNearestLocation(Vector3 originalLocation)
    {
        Vector3 nearestLocation = Vector3.zero;
        bool foundNearest = false;
        for (float i = 0.5f; i < 4f && foundNearest == false; i += 0.5f)
        {
            if (!isMapCollisionNearby(new(originalLocation.x + i, originalLocation.y, 0)))
            {
                nearestLocation = new(originalLocation.x + i, originalLocation.y, 0);
                foundNearest = true;
            }
            else if (!isMapCollisionNearby(new(originalLocation.x - i, originalLocation.y, 0)))
            {
                nearestLocation = new(originalLocation.x - i, originalLocation.y, 0);
                foundNearest = true;
            }
            else if (!isMapCollisionNearby(new(originalLocation.x, originalLocation.y - i, 0)))
            {
                nearestLocation = new(originalLocation.x, originalLocation.y - i, 0);
                foundNearest = true;
            }
            else if (!isMapCollisionNearby(new(originalLocation.x, originalLocation.y + i, 0)))
            {
                nearestLocation = new(originalLocation.x, originalLocation.y + i, 0);
                foundNearest = true;
            }
        }
        return nearestLocation;
    }

    bool isMapCollisionNearby(Vector3 location)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(location, 0.5f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag.Equals("AvoidOverlap"))
            {
                return true;
            }
        }
        return false;
    }


    IEnumerator PhaseShiftPrecast()
    {
        shiftProgressBar.gameObject.SetActive(true);

        precastingTimer = shiftPrecastTime;
        //create precast rift
        GameObject riftObject = Instantiate<GameObject>(rift);
        riftObject.transform.parent = transform;
        riftObject.transform.position = transform.position;
        while (precastingTimer > 0)
        {
            shiftProgressBar.value = precastingTimer / shiftPrecastTime;
            precastingTimer -= Time.deltaTime;
            yield return null;
        }
        Destroy(riftObject);
        shiftProgressBar.gameObject.SetActive(false);

        ToPhaseShift();
    }

    //public void OnPause(bool paused)
    //{
    //    if (paused)
    //    {
    //        playerInputAction.Player.PhaseShift.Disable();
    //    }
    //    else
    //    {
    //        playerInputAction.Player.PhaseShift.Enable();
    //    }
    //}
}
