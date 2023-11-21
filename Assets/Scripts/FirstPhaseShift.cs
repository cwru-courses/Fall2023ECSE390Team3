using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPhaseShift : MonoBehaviour
{
 
   //  [SerializeField] private GameObject rift;  // made pickedUpObject a serialized field for testing

    private Collider2D objInRadius;
    private float pickUpRadius  = 3.0f;
    [SerializeField] private LayerMask layerMask;  

    [SerializeField] private TextMeshProUGUI shiftText;

    bool collided = false; 

    void Start()
    {
    }

    // Update is called once per frame
    void Update() 
     {
    //   objInRadius = Physics2D.OverlapCircle(transform.position, pickUpRadius);

    //   if (objInRadius)
    //     {
    //             shiftText.text = "Press Q to Interact"; //change P to user input
    //     }

    //     else
    //     {
    //         shiftText.text = string.Empty;
    //     }
    }

  public void OnTriggerEnter2D(Collider2D other){
    if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0) {
        Debug.Log("Hit with Layermask");
        shiftText.text = "Press Q to Interact"; //change P to user input
    }
  }

  public void OnTriggerExit2D(Collider2D other){
     shiftText.text = ""; //change P to user input
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