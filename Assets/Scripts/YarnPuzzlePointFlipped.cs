using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzlePointFlipped : MonoBehaviour
{
    [SerializeField] private GameObject lastPoint;
    [SerializeField] private GameObject nextPoint;
    [SerializeField] private GameObject nextNextPoint;
    [SerializeField] private GameObject puzzleControllerObject;
    [SerializeField] private GameObject lineControllerObject;
    [SerializeField] private AudioSource canBeTriggeredSFX;
    [SerializeField] private AudioSource activateSFX;
    [SerializeField] private AudioSource deactivateSFX;
    [SerializeField] private bool needToStartYarnTrailInNormalWorld = false;

    public CollisionDialogue collisionDialogue;
    [SerializeField] GameObject dialogueBox;
    public bool isFirstPoint;
    private SpriteRenderer spriteRenderer;
    private Animator childAnimator;
    private bool firstTimeTrigger = true;
    private GameObject playerFoot;
    private bool triggerable = false;
    public Gradient gradient;

    /*
     * stage = 0: this point untriggered
     * stage = 1: this point triggered, but nextNext point is untriggered
     * note: In stage = 1, if puzzle controller is on this point, and if trail is cutted, this point will be untriggered
     */
    private int stage = 0;
    private YarnPuzzleController puzzleController;
    private YarnLineController lineController;

    private void Awake()
    {
        // Get Animator of child object
        childAnimator = GetComponentInChildren<Animator>();

        if (childAnimator == null)
        {
            Debug.LogError("Child Animator not found.");
        }

        // if there is no next point, activate itself
        if (nextPoint == null)
        {
            Debug.Log("no next point");
            stage = 1;
            childAnimator.SetBool("Shining", false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // get child
        Transform childTransform = transform.Find("Red Beacon");
        // get SpriteRenderer component
        spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            print("can't find sprite renderer");
        }
        puzzleController = puzzleControllerObject.GetComponent<YarnPuzzleController>();
        lineController = lineControllerObject.GetComponent<YarnLineController>();

        playerFoot = GameObject.FindGameObjectWithTag("PlayerFoot");
    }

    // Update is called once per frame
    void Update()
    {
        if (stage == 0 && childAnimator.GetBool("Shining") != true)
        {
            childAnimator.SetBool("Shining", true);
        }
        else if (stage == 1 && childAnimator.GetBool("Shining") != false)
        {
            childAnimator.SetBool("Shining", false);
        }

        if (stage == 0)
        {
            Vector3 targetPosition = playerFoot.transform.position;
            Vector3 myPosition = transform.position;
            // only calculate distance on x and y
            float distanceX = Mathf.Abs(targetPosition.x - myPosition.x);
            float distanceY = Mathf.Abs(targetPosition.y - myPosition.y);
            float distance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);
            print("distance: " + distance);
            if (!triggerable && distance <= 0.45f)
            {
                if (canBeTriggeredSFX != null)
                {
                    canBeTriggeredSFX.Play();
                }
                triggerable = true;
            }
            else if (distance <= 0.45f)
            {
                spriteRenderer.color = Color.Lerp(gradient.Evaluate(1f), Color.black, Mathf.PingPong(Time.time * 1.5f, 1));
            }
            else if (triggerable && distance > 0.45f)
            {
                triggerable = false;
            }
            else if (distance > 0.45f)
            {
                spriteRenderer.color = gradient.Evaluate(1f);
            }
        }
        else if (stage == 1 && spriteRenderer.color != gradient.Evaluate(1f))
        {
            spriteRenderer.color = gradient.Evaluate(1f);
        }
    }

    private void ConnectToLastPoint()
    {
        lineController.ConnectPoints(lastPoint.transform.position, transform.position);
    }

    private void RevealPoint(GameObject point)
    {
        point.SetActive(true);
        Debug.Log("Set active");
    }

    private void HidePoint(GameObject point)
    {
        point.SetActive(false);
    }
    
    public int GetStage()
    {
        return stage;
    }
    
    public void NextStage()
    {
        if (stage == 0)
        {
            spriteRenderer.color = gradient.Evaluate(1f);
            // disable shinning on the child object animator
            childAnimator.SetBool("Shining", false);
            // update the onPoint attribute in YarnPuzzleController if first time triggered
            if (firstTimeTrigger)
            {
                puzzleController.IncreaseOnPoint();
                firstTimeTrigger = false;
            }
            // if there is a last point, connect to it
            if (lastPoint != null)
            {
                ConnectToLastPoint();
            }
            // if there is a next point, reveal(activate) it
            if (nextPoint != null)
            {
                RevealPoint(nextPoint);
            }
            // if there is a next Next point, reveal(activate) it
            if (nextNextPoint != null)
            {
                RevealPoint(nextNextPoint);
            }
            stage++;
            if (isFirstPoint){
                dialogueBox.SetActive(true);
                collisionDialogue.StartRunning(dialogueBox);
                isFirstPoint = false;
            }
            if (deactivateSFX != null)
            {
                deactivateSFX.Play();
            }

            if (needToStartYarnTrailInNormalWorld)
            {
                YarnTrail._instance.exceptionForPointTwo = true;
            }
        }
    }

    public void DecreaseStage()
    {
        if (stage == 1)
        {
            // enable shinning on the child object animator
            childAnimator.SetBool("Shining", true);
            if (needToStartYarnTrailInNormalWorld)
            {
                YarnTrail._instance.exceptionForPointTwo = false;
            }
            // hide next point in normal world
            HidePoint(nextPoint);
            // hide nextNext point in normal world
            HidePoint(nextNextPoint);
            stage--;

            if (activateSFX != null)
            {
                activateSFX.Play();
            }
        }
    }

    public GameObject GetNextPoint()
    {
        return nextPoint;
    }

}
