using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnTrail : MonoBehaviour
{
    public static YarnTrail _instance;
    private TrailRenderer trailRranderer;
    private Vector3 lastPosition;
    private Vector3 thisPosition;
    [SerializeField] private float yarnConsumptionRate;
    [SerializeField] private GameObject YarnPuzzleControllerObjectOne;
    [SerializeField] private GameObject YarnPuzzleControllerObjectTwo;
    [SerializeField] private GameObject YarnPuzzleControllerObjectThree;
    [SerializeField] private YarnTrailCollider trailCollider;
    private YarnPuzzleController puzzleControllerOne;
    private YarnPuzzleController puzzleControllerTwo;
    private YarnPuzzleController puzzleControllerThree;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        // YarnPuzzleControllerObjectOne is null in tutorial dungeon because there is no yarn puzzle
        if (YarnPuzzleControllerObjectOne != null)
        {
            puzzleControllerOne = YarnPuzzleControllerObjectOne.GetComponent<YarnPuzzleController>();
        }
        if (YarnPuzzleControllerObjectTwo != null)
        {
            puzzleControllerTwo = YarnPuzzleControllerObjectTwo.GetComponent<YarnPuzzleController>();
        }
        if (YarnPuzzleControllerObjectThree != null)
        {
            puzzleControllerThree = YarnPuzzleControllerObjectThree.GetComponent<YarnPuzzleController>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        trailRranderer = GetComponent<TrailRenderer>();
        thisPosition = transform.position;
        lastPosition = thisPosition;
        //add toggleEmission here to enable yarn trail when player loads into flipped side
        toggleEmission(); 
    }

    // Update is called once per frame
    void Update()
    {
        // if player in flipped world
        if (isInFlippedWorld())
        {
            thisPosition = transform.position;
            if (thisPosition != lastPosition)
            {
                decreaseYarn();
            }
            lastPosition = thisPosition;
        }
    }

    public void toggleEmission()
    {
        if(isInFlippedWorld()) {
            trailRranderer.enabled = true;
        } 
        else {
            trailRranderer.enabled = false;
            ClearYarnTrail();
            // puzzle needs to update
            if (puzzleControllerOne != null && puzzleControllerOne.PuzzleActive())
            {
                puzzleControllerOne.TrailCutted();
            }
            else if (puzzleControllerTwo != null && puzzleControllerTwo.PuzzleActive())
            {
                puzzleControllerTwo.TrailCutted();
            }
            else if (puzzleControllerThree != null && puzzleControllerThree.PuzzleActive())
            {
                puzzleControllerThree.TrailCutted();
            }
        }

        // if (trailRranderer.enabled == false)
        // {
        //     trailRranderer.enabled = true;
        //     inFlipWorld = true;
        // }
        // else
        // {
        //     trailRranderer.enabled = false;
        //     trailRranderer.Clear();
        //     inFlipWorld = false;
        // }
    }

    public void ClearYarnTrail()
    {
        // to clear yarn trail
        trailRranderer.Clear();
        // to destroy collider of yarn trail
        if (trailCollider != null)
        {
            trailCollider.ClearColliderPoints();
        }
    }

    private void decreaseYarn()
    {
        float toDecrease = yarnConsumptionRate * Time.deltaTime;
        PlayerStats._instance.UseYarn(toDecrease);
    }

    //add function to see if in flipped world based on player's position instead of onPhaseShift
    private bool isInFlippedWorld() {
        if(GameObject.FindWithTag("Player").transform.position.y < 0) {
            PlayerStats._instance.inFlippedWorld = true;
        } else {
            PlayerStats._instance.inFlippedWorld = false; 
        }
        return PlayerStats._instance.inFlippedWorld; 
    }

}
