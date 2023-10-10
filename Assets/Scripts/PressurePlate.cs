using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerStay2D(Collider2D other) {
        // Check for stone and distance from center of pressure plate
        if (other.tag == "Stone") {
            float distance = Vector3.Distance(transform.position, other.transform.position);
            Debug.Log("Distance: " + distance);

            // If distance < 0.5f, then pressure plate triggered
            if (distance < 0.5f) {
                Debug.Log("Pressure Plate Triggered!");
                // Open door/chest
            }  
        } 
    }

}
