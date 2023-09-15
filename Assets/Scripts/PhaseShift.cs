using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseShift : MonoBehaviour
{
    public Camera[] cameras;
    public KeyCode switchKey = KeyCode.Q;
    private GameObject player;

    private int currentCameraIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        //shift character to alternate world
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            cameras[currentCameraIndex].enabled = false;
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            cameras[currentCameraIndex].enabled = true;
        }
    }
}