using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzlePointNormal : MonoBehaviour
{
    [SerializeField] private Color colorWhenUntrigged = Color.white;
    [SerializeField] private Color colorWhenFixed = Color.red;
    [SerializeField] private GameObject lastPoint;
    [SerializeField] private GameObject nextPoint;
    private SpriteRenderer spriteRenderer;
    private Animator childAnimator;
    /*
     * stage = 0: this point untriggered
     * stage = 1: this point triggered. 
     * If puzzle controller is on Flipped point whose next point is this point, and if trail is cutted, stage of this point will go back to one
     */
    private int stage = 0;

    // Start is called before the first frame update
    void Start()
    {
        // get SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = colorWhenUntrigged;

        // Get Animator of child object
        childAnimator = GetComponentInChildren<Animator>();

        if (childAnimator == null)
        {
            Debug.LogError("Child Animator not found.");
        }

        // if there is no next point, then it's the last point of the puzzle
        if (nextPoint == null)
        {
            stage = 1;
            spriteRenderer.color = colorWhenFixed;
            childAnimator.SetBool("Shining", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RevealNextPoint()
    {
        nextPoint.SetActive(true);
    }

    public void HideNextPoint()
    {
        nextPoint.SetActive(false);
    }

    // increase stage by 1 (from 0 to 1 only)
    public void NextStage()
    {
        if (stage == 0)
        {
            // change color of the circle
            spriteRenderer.color = colorWhenFixed;
            // disable shinning on the child object animator
            childAnimator.SetBool("Shining", false);
            // reveal next point in flipped world
            RevealNextPoint();
            // increase stage of last point in flipped world from stage 1 to 2
            IncreaseStageOfLastPoint();
            stage++;
        }
    }

    public void DecreaseStage()
    {
        if (stage == 1)
        {
            // change color of the circle
            spriteRenderer.color = colorWhenUntrigged;
            // disable shinning on the child object animator
            childAnimator.SetBool("Shining", true);
            // hide next point in flipped world
            HideNextPoint();
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
