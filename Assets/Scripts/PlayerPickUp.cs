using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    private GameObject pickedUpObject;
    private float pickUpRadius  = 3.0f;
    [SerializeField] private LayerMask layerMask; //layer of objects that can be picked up

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //button is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pickedUpObject == null)
            {
                PickUp();

            }
            else
            {
                PutDown();
            }  
        }
    }

    //pick up an object
    void PickUp()
    {
        //array of objects that are in the PickUpable layer within the pickupable radius
        Collider2D objInRadius= Physics2D.OverlapCircle(transform.position, pickUpRadius, layerMask);

        //if the object exists
        if (objInRadius != null)
        {
            //store a reference to the picked object
            pickedUpObject = objInRadius.gameObject;

            //disable the collider of the picked object
            Collider2D objectCollider = pickedUpObject.GetComponent<Collider2D>();
            if (objectCollider != null)
            {
                objectCollider.enabled = false;
            }

            //attach the object to player by making it a child
            pickedUpObject.transform.parent = transform;

            //move the object to the player location
            pickedUpObject.transform.position = transform.position;
        }
    }

    //drop an object
    void PutDown()
    {
        //enable the collider of the picked object
        Collider2D objectCollider = pickedUpObject.GetComponent<Collider2D>();
        if (objectCollider == null)
        {
            objectCollider.enabled = true;
        }

        //detach the object to player by making it a child
        pickedUpObject.transform.parent = null;
    }
}
