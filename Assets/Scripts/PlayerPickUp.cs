using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{

    private Transform playerTransform;
    private GameObject pickedUpObject;
    private float pickUpDist  = 1000.0f;
    private LayerMask layerMask; //layer of objects that can be picked up

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        layerMask = LayerMask.GetMask("PickUpable");
    }

    // Update is called once per frame
    void Update()
    {
        //if button is pressed, pick up object
        if (Input.GetKeyDown(KeyCode.P))
        {
            PickUp();
        }  
    }

    //pick up an object
    void PickUp()
    {
        //debug that allows to track the raycast
        Debug.DrawRay(playerTransform.position, playerTransform.forward * pickUpDist, Color.red);

        //get the first object hit in the PickUpable layer within the pickupable distance
        RaycastHit2D hit2D = Physics2D.Raycast(playerTransform.position, playerTransform.forward, pickUpDist, layerMask, 0);

        //if the collider of object is not null
        if (hit2D.collider != null)
        {
            //store a reference to the picked object
            pickedUpObject = hit2D.collider.gameObject;

            //disable the collider of the picked object
            Collider2D objectCollider = pickedUpObject.GetComponent<Collider2D>();
            if (objectCollider != null)
            {
                objectCollider.enabled = false;
            }

            //attach the object to player by making it a child
            pickedUpObject.transform.parent = playerTransform;

            //move the object to the player location
            pickedUpObject.transform.position = playerTransform.position;
        }
    }
}
