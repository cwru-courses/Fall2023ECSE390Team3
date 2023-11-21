using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPhaseShift : MonoBehaviour
{
 
   //  [SerializeField] private GameObject rift;  // made pickedUpObject a serialized field for testing

    private Collider2D objInRadius;
    private float pickUpRadius  = 0.5f;
    [SerializeField] private LayerMask layerMask;  

    [SerializeField] private TextMeshProUGUI shiftText;

    bool collided = false; 

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      //   objInRadius = Physics2D.OverlapCircle(transform.position, pickUpRadius, layerMask);

      // if (objInRadius)
      //   {
      //           
      //   }

      //   else
      //   {
      //       shiftText.text = string.Empty;
      //   }

      //   Vector2 playerPosition = transform.position;

      //   // Perform a physics overlap check in a circular area
      //   Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition, pickUpRadius);

      //   // Check each collider for the specified tag
      //   foreach (Collider2D collider in colliders)
      //   {
      //       if (collider.CompareTag("FirstRift"))
      //       {
      //           // The player is near an object with the specified tag
      //          //  Debug.Log("worked");
      //           shiftText.text = "Press Q to Interact"; 
      //           // Do something with the detected object
      //       } else {
      //          // shiftText.text = string.Eampty; 
      //       }
      //   }
    }


   //  private void OnTriggerEnter2D(Collider2D collider)
   //  {
   //      if (collider.gameObject.CompareTag("Player") == true && !collided)
   //      {
   //       Debug.Log("In rift"); 

   //       shiftText.text = "Press Q to Interact";
   //       collided = true; 
   //       // PickUpIcon.SetActive(true); 
   //          // autoSaveText.SetActive(true);
   //          // GameObject player = GameObject.FindGameObjectWithTag("Player"); 
   //          // player.GetComponent<PhaseShift>().StartPhaseShift();
   //          // Player._instance.StartPhaseShift(); 
   //          // collided = true; 
   //      } else {
   //          shiftText.text = string.Empty;
   //      }
   //  }


}