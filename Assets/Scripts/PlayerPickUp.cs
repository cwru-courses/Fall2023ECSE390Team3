using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUp : MonoBehaviour
{
 
    [SerializeField] private GameObject pickedUpObject;  // made pickedUpObject a serialized field for testing
    private float pickUpRadius  = 3.0f;
    [SerializeField] private LayerMask layerMask;  //layer of objects that can be picked up

    public void PickupInputCallback(InputAction.CallbackContext ctx)
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

    //pick up an object
    void PickUp() {
        //array of objects that are in the PickUpable layer within the pickupable radius
        Collider2D objInRadius= Physics2D.OverlapCircle(transform.position, pickUpRadius, layerMask);

        //if the object exists
        if (objInRadius != null) {
            // Don't pick up a pressure plate
            if (objInRadius.gameObject.tag == "PressurePlate")
                return;

            //store a reference to the picked object
            pickedUpObject = objInRadius.gameObject;

            //disable the collider of the picked object
            Collider2D objectCollider = pickedUpObject.GetComponent<Collider2D>();
            if (objectCollider != null) {
                objectCollider.enabled = false;

                // Disable rigid body of picked up object
                Destroy(pickedUpObject.GetComponent<Rigidbody2D>());

            }

            //attach the object to player by making it a child
            pickedUpObject.transform.parent = transform;

            //move the object to the player location
            pickedUpObject.transform.position = transform.position;
        }
    }


    //drop an object
    void PutDown() {
        // If an object has been picked up
        if (pickedUpObject != null) {

            // Store another reference to object
            GameObject objectToPutDown = pickedUpObject;
            
            // Store object's collider component
            Collider2D objectCollider = pickedUpObject.GetComponent<Collider2D>();

            // Verify that the pickedUpObject has had its collider disabled
            if (!objectCollider.enabled) {
                // Re-enable the object's collider
                objectCollider.enabled = true;

                // Re-enable rigid body of picked up object
                pickedUpObject.AddComponent<Rigidbody2D>();
                pickedUpObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                pickedUpObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                pickedUpObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;


                // Then detach the object to player by making it a child
                pickedUpObject.transform.parent = null;

                // Clear reference to pickedUpObject
                pickedUpObject = null;

            }
        } 
    }

}
