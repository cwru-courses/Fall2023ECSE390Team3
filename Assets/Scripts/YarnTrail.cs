using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnTrail : MonoBehaviour
{
    public static YarnTrail _instance;
    private TrailRenderer trailRranderer;
    private bool inFlipWorld;
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
        inFlipWorld = false;
        thisPosition = transform.position;
        lastPosition = thisPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // if player in flipped world
        if (inFlipWorld)
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
        if (trailRranderer.enabled == false)
        {
            trailRranderer.enabled = true;
            inFlipWorld = true;
        }
        else
        {
            trailRranderer.enabled = false;
            trailRranderer.Clear();
            inFlipWorld = false;
        }
    }

    private void decreaseYarn()
    {
        float toDecrease = yarnConsumptionRate * Time.deltaTime;
        PlayerStats._instance.UseYarn(toDecrease);
        Debug.Log("" + Time.deltaTime + "; " + toDecrease);
    }

}
