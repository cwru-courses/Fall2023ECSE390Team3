using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    [SerializeField] private GameObject chestToUnlock;
    [SerializeField] private GameObject tilemapToDisable;
    [SerializeField] private Vector3 tilemapPosition;   // Set in inspector

    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Start() {
        mainCamera = Camera.main;
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
            if (distance < 1.5f) {
                Debug.Log("Pressure Plate Triggered!");

                // Open chest
                if (chestToUnlock != null) {
                    // This calls object focus within chest script
                    chestToUnlock.GetComponent<Chest>().setChestOpen(true);
                    // If there's also a tilemap to disable, disable it
                    if (tilemapToDisable != null)
                            tilemapToDisable.SetActive(false);
                }
                else if (tilemapToDisable != null) {
                    // Call coroutine to object focus on tilemapToDisable and then disable it
                    StartCoroutine(tilemapFocus());
                }
                    
                
            }
        } 
    }


    private IEnumerator tilemapFocus() {
        mainCamera.GetComponent<CameraControl>().SwitchToBossRoom(tilemapPosition);
        yield return new WaitForSeconds(0.5f);

        tilemapToDisable.SetActive(false);

        yield return new WaitForSeconds(3.0f);
        mainCamera.GetComponent<CameraControl>().SwitchToPlayerFocus();
    }







    // TEST
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Stone") {
            Debug.Log("Colliding with stone");
        }
    }


}
