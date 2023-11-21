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
    [SerializeField] private bool needToStartYarnTrailInNormalWorld = false;

    public CollisionDialogue collisionDialogue;
    [SerializeField] GameObject dialogueBox;
    public bool isFirstPoint;
    private SpriteRenderer spriteRenderer;
    private Animator childAnimator;
    private bool firstTimeTrigger = true;

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
        // get SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        puzzleController = puzzleControllerObject.GetComponent<YarnPuzzleController>();
        lineController = lineControllerObject.GetComponent<YarnLineController>();

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
        }
    }

    public GameObject GetNextPoint()
    {
        return nextPoint;
    }

}
