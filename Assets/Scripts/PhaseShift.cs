using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PhaseShift : MonoBehaviour
{
    public static PhaseShift _instance;

    [SerializeField] private float shiftCD;
    [SerializeField] private float shiftPrecastTime;
    [SerializeField] private float shiftInAnimDuration;
    [SerializeField] private Slider shiftProgressBar;
    [SerializeField] private GameObject rift;
    [SerializeField] private GameObject foot; // to detect nearby puzzle points
    [SerializeField] private Image uiImg;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject FirstRift; 
    [SerializeField] private FirstPhaseShift firstPhaseShift;
    [SerializeField] private TextMeshProUGUI PickUpIcon;
    [SerializeField] private AudioSource stitchInSFX;
    [SerializeField] private AudioSource stitchOutSFX;


    private PlayerPickUp playerPickUp;

    // rockPortal script of rock
    private RockPortal rockPortal;

    private bool firstRiftDone = false; 
    private bool inFirstRift = false; 

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

        //if loading from saved version
        if (SaveSystem.listSavedFiles.Contains(SaveSystem.currentFileName)) {
            firstRiftDone = PlayerStats._instance.firstRiftDone;
        }
        if(FirstRift != null && !firstRiftDone) {
            FirstRift.SetActive(true); 
        }
    }

    void Start() {
        if(string.Compare(SceneManager.GetActiveScene().name, "Tutorial Level") != 0) {
            firstRiftDone = true; 
        }

        playerPickUp = GetComponent<PlayerPickUp>();

        /*
        // Get reference to rockPortal script if rock exists (i.e. in tutorial level)
        rock = GameObject.Find("Stone");
        if (rock != null) {
            rockPortal = rock.GetComponent<RockPortal>();
        }
        */

        // Get rockPortal script if in tutorial level scene
        if (string.Compare(SceneManager.GetActiveScene().name, "Tutorial Level") == 0) {
            rockPortal = GameObject.Find("Stone").GetComponent<RockPortal>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("FirstRift") == true && !firstRiftDone)
        {
            inFirstRift = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        inFirstRift = false; 
    }

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
        if(inFirstRift){
            if (shiftCDTimer <= 0 && precastingTimer <= 0) {
                StartCoroutine("PhaseShiftPrecast");
                firstRiftDone = true;
                PlayerStats._instance.firstRiftDone = true; 
            }
        }
        else if(firstRiftDone){
             if (shiftCDTimer <= 0 && precastingTimer <= 0) {
                StartCoroutine("PhaseShiftPrecast");
                //OnDisable();    // Disable input to avoid more than one click
            }
        }
    }

    // this is for enemy trigger
    public void StartPhaseShiftByEnemy()
    {
        // don't need to check cd, because enemy trigger it whatever cd is ready or not
        if (precastingTimer <= 0)
        {
            StartCoroutine("PhaseShiftPrecast");
        }
    }

    public void ToPhaseShift()
    {
        // trigger nearby puzzle point
        bool cutted = TriggerNearbyYarnPuzzlePoints();

        // Move character into the alternate world
        // Added subtraction to make sure the shift is based on the foot's position
        Vector3 currentLocation = transform.position;
        currentLocation.y = -currentLocation.y + 2f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentLocation, 0.5f);
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
            Vector3 foundLocation = findNearestLocation(currentLocation);
            if (foundLocation != Vector3.zero)
            {
                currentLocation = foundLocation;
                Debug.Log("shifted");
            }
            else
            {
                Debug.Log("fail to found neareast location");
            }
        }
        transform.position = currentLocation;

        //OnEnable();     // Enable input again
        
        // Added this line to toggle emission of yarn trail -- Jing
        AmbientSystem.Instance.OnPhaseShift();
        YarnTrail._instance.toggleEmission(cutted);
        Debug.Log("Cutted is: " + cutted);
    }

    private bool TriggerNearbyYarnPuzzlePoints()
    {
        bool toReturn = true;

        // search all puzzle points
        GameObject[] puzzlePoints = GameObject.FindGameObjectsWithTag("YarnPuzzlePoints");

        // increase stage by 1 for nearby point
        foreach (GameObject point in puzzlePoints)
        {
            Vector3 targetPosition = point.transform.position;
            Vector3 myPosition = foot.transform.position;
            // only calculate distance on x and y
            float distanceX = Mathf.Abs(targetPosition.x - myPosition.x);
            float distanceY = Mathf.Abs(targetPosition.y - myPosition.y);
            float distance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);
            //Debug.Log(distance);

            // if the point nearby(either in flipped or normal world) is in stage 0, increase it to stage 1
            if (distance < 0.5f)
            {
                if (point.GetComponent<YarnPuzzlePointNormal>() != null && point.GetComponent<YarnPuzzlePointNormal>().GetStage() == 0)
                {
                    point.GetComponent<YarnPuzzlePointNormal>().NextStage();
                    toReturn = false;
                }
                else if (point.GetComponent<YarnPuzzlePointFlipped>() != null && point.GetComponent<YarnPuzzlePointFlipped>().GetStage() == 0)
                {
                    point.GetComponent<YarnPuzzlePointFlipped>().NextStage();
                    toReturn = false;
                }
            }
        }

        return toReturn;
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
        // If holding rock when phase shift, call rock fade method
        if ((playerPickUp.pickedUpObject != null) && (playerPickUp.pickedUpObject.tag == "Stone"))
            rockPortal.CallRockFade();

        if(stitchInSFX) {
            stitchInSFX.Play();
        }
        anim.Play("Stitch_In_Player");
        PlayerMovement._instance.OnPause(true);
        PlayerAttack._instance.EnableAttack(false);
        shiftProgressBar.gameObject.SetActive(true);

        precastingTimer = shiftPrecastTime;
        GameObject riftObject = null;
        //create precast rift
        if(firstRiftDone) {
            riftObject = Instantiate<GameObject>(rift);
            riftObject.transform.parent = transform;
            riftObject.transform.position = transform.position;
            riftObject.transform.rotation = Quaternion.Euler(0, 0, 90f); 
            riftObject.transform.position = new Vector3(riftObject.transform.position.x, riftObject.transform.position.y-1, riftObject.transform.position.z);
        }
        bool shifted = false;
        while (precastingTimer > 0)
        {
            if (!shifted && shiftPrecastTime - precastingTimer >= shiftInAnimDuration)
            {
                shifted = true;
                ToPhaseShift();
                if(!firstRiftDone || inFirstRift) {
                    riftObject = Instantiate<GameObject>(rift);
                    riftObject.transform.parent = transform;
                    riftObject.transform.position = transform.position;
                    riftObject.transform.rotation = Quaternion.Euler(0, 0, 90f); 
                    riftObject.transform.position = new Vector3(riftObject.transform.position.x, riftObject.transform.position.y-1, riftObject.transform.position.z);
                }
                if(stitchOutSFX) {
                    stitchOutSFX.Play();
                }
                anim.Play("Stitch_Out_Player");
            }
            shiftProgressBar.value = precastingTimer / shiftPrecastTime;
            precastingTimer -= Time.deltaTime;
            yield return null;
        }
        Destroy(riftObject);
        shiftProgressBar.gameObject.SetActive(false);
        PlayerMovement._instance.OnPause(false);
        PlayerAttack._instance.EnableAttack(true);

        anim.Play("IdleTree");
        shiftCDTimer = shiftCD;
        if(FirstRift != null) {
            FirstRift.SetActive(false); 
        }
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
