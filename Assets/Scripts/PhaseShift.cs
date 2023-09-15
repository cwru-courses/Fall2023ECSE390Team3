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
        // If player pressed key Q
        if (Input.GetKeyDown(switchKey))
        {
            // Move character into the alternate world
            Vector3 currentLocation = player.transform.position;
            Debug.Log(currentLocation);
            player.transform.position = new Vector3(currentLocation.x, currentLocation.y * -1, currentLocation.z);
            Debug.Log(player.transform.position);

            // Go to alternate camera
            cameras[currentCameraIndex].enabled = false;
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            cameras[currentCameraIndex].enabled = true;
        }
    }
}