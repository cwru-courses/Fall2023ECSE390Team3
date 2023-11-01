using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzlePointFlipped : MonoBehaviour
{
    [SerializeField] private Color colorWhenUntrigged = Color.white;
    [SerializeField] private Color colorWhenTrigged = Color.blue;
    [SerializeField] private GameObject lastPoint;
    [SerializeField] private GameObject nextPoint;
    [SerializeField] private GameObject puzzleControllerObject;
    [SerializeField] private GameObject lineControllerObject;

    [SerializeField] private GameObject dialogueCollider; //for enabling the collider on the other side
    public bool isFirstPoint;
    private SpriteRenderer spriteRenderer;
    private Animator childAnimator;
    /*
     * stage = 0: this point untriggered
     * stage = 1: this point triggered, but next point(normal) is untriggered
     * stage = 2: this point and next point(normal) are triggered. 
     * not: In stage = 2, if puzzle controller is on this point, and if trail is cutted, next point (in normal world) of this point will be untriggered
     */
    private int stage = 0;
    private YarnPuzzleController puzzleController;
    private YarnLineController lineController;

    // Start is called before the first frame update
    void Start()
    {
        // get SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = colorWhenUntrigged;
        puzzleController = puzzleControllerObject.GetComponent<YarnPuzzleController>();
        lineController = lineControllerObject.GetComponent<YarnLineController>();

        // Get Animator of child object
        childAnimator = GetComponentInChildren<Animator>();

        if (childAnimator == null)
        {
            Debug.LogError("Child Animator not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void ConnectToLastPoint()
    {
        lineController.ConnectPoints(lastPoint.transform.position, transform.position);
        //lastPoint.GetComponent<YarnPuzzlePointFlipped>().SetFixation(true);
    }

    private void RevealNextPoint()
    {
        if(isFirstPoint){
            dialogueCollider.SetActive(true);
            isFirstPoint = false;
        }
        nextPoint.SetActive(true);
        Debug.Log("Set active");
    }
    
    public int GetStage()
    {
        return stage;
    }
    
    public void NextStage()
    {
        if (stage == 0)
        {
            // change color of the circle
            spriteRenderer.color = colorWhenTrigged;
            // disable shinning on the child object animator
            childAnimator.SetBool("Shining", false);
            // update the onPoint attribute in YarnPuzzleController
            puzzleController.IncreaseOnPoint();
            // if there is a last point, connect to it
            if (lastPoint != null)
            {
                ConnectToLastPoint();
            }
            // if there is a next point, reveal(activate) it
            if (nextPoint != null)
            {
                RevealNextPoint();
            }
            stage++;
        }
        else if (stage == 1)
        {
            stage++;
        }
    }

    public GameObject GetNextPoint()
    {
        return nextPoint;
    }

}
