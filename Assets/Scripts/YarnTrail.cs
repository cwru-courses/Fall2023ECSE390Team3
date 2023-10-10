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

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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
            trailRranderer.Clear();
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

    private void decreaseYarn()
    {
        float toDecrease = yarnConsumptionRate * Time.deltaTime;
        PlayerStats._instance.UseYarn(toDecrease);
    }

    //add function to see if in flipped world based on player's position instead of onPhaseShift
    private bool isInFlippedWorld() {
        if(GameObject.FindWithTag("Player").transform.position.y < 0) {
            inFlipWorld = true;
        } else {
            inFlipWorld = false; 
        }
        return inFlipWorld; 
    }

}
