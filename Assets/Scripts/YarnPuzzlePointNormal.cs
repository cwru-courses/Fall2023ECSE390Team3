using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzlePointNormal : MonoBehaviour
{
    [SerializeField] private GameObject lastPoint;
    [SerializeField] private GameObject nextPoint;
    [SerializeField] private GameObject nextNextPoint;
    [SerializeField] private GameObject puzzleControllerObject;
    [SerializeField] private GameObject lineControllerObject;
    [SerializeField] private bool needToEndYarnTrailInNormalWorld = false;
    public CollisionDialogue collisionDialogue;
    [SerializeField] GameObject dialogueBox;
    private SpriteRenderer spriteRenderer;
    private Animator childAnimator;
    private YarnPuzzleController puzzleController;
    private YarnLineController lineController;
    public bool isFirstPoint;
    private bool firstTimeTrigger = true;

    /*
     * stage = 0: this point untriggered
     * stage = 1: this point triggered. 
     */
    private int stage = 0;

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
            stage = 1;
            childAnimator.SetBool("Shining", false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // get SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (puzzleControllerObject != null)
        {
            puzzleController = puzzleControllerObject.GetComponent<YarnPuzzleController>();
        }
        if (lineControllerObject != null)
        {
            lineController = lineControllerObject.GetComponent<YarnLineController>();
        }
        

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
        if (lineController != null)
        {
            lineController.ConnectPoints(lastPoint.transform.position, transform.position);
        }
        
    }

    private void RevealPoint(GameObject point)
    {
        point.SetActive(true);
    }

    public void HidePoint(GameObject point)
    {
        point.SetActive(false);
    }

    // increase stage by 1 (from 0 to 1 only)
    public void NextStage()
    {
        if (stage == 0)
        {
            // disable shinning on the child object animator
            childAnimator.SetBool("Shining", false);
            // update the onPoint attribute in YarnPuzzleController if first time triggered
            if (puzzleController != null && firstTimeTrigger)
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

            if (needToEndYarnTrailInNormalWorld)
            {
                YarnTrail._instance.exceptionForPointTwo = false;
            }
        }
    }

    public void DecreaseStage()
    {
        if (stage == 1)
        {
            // enable shinning on the child object animator
            childAnimator.SetBool("Shining", true);
            // hide next point in flipped world
            HidePoint(nextPoint);
            // hide next point in flipped world
            HidePoint(nextNextPoint);
            stage--;
        }
    }

    public int GetStage()
    {
        return stage;
    }

    // increase stage of last point in flipped world from stage 1 to 2
    public void IncreaseStageOfLastPoint()
    {
        if (lastPoint != null)
        {
            if (lastPoint.GetComponent<YarnPuzzlePointFlipped>().GetStage() == 1)
            {
                lastPoint.GetComponent<YarnPuzzlePointFlipped>().NextStage();
            }
        }
    }
}
