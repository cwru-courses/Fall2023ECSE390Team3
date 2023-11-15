using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    [SerializeField] private GameObject chestToUnlock;
    [SerializeField] private GameObject tilemapToDisable;
    [SerializeField] private Vector3 tilemapPosition;   // Set in inspector

    private bool chestUnlocked = false;
    private bool tilemapDisabled = false;

    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Start() {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerStay2D(Collider2D other) {
        // If colliding with stone
        if (other.tag == "Stone") {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            // If distance is less than some value, pressure plate triggered
            if (distance < 1.5f) {

                // If there is a chest to unlock, do so
                if ((chestToUnlock != null) & (chestUnlocked == false)) {
                    // This calls object focus within chest script
                    chestToUnlock.GetComponent<Chest>().setChestOpen(true);
                    chestUnlocked = true;
                    // If there's also a tilemap to disable, disable it
                    if (tilemapToDisable != null) {
                        tilemapToDisable.SetActive(false);
                        tilemapDisabled = true;
                    }
                }
                    
                // Else, if there is a tilemap to disable, do so
                else if ((tilemapToDisable != null) & (tilemapDisabled == false)) {
                    // Call coroutine to object focus on tilemapToDisable and then disable it
                    StartCoroutine(tilemapFocus());
                    tilemapDisabled = true;
                }
                        
            }
        } 
    }


    private IEnumerator tilemapFocus() {
        mainCamera.GetComponent<CameraControl>().SwitchToBossRoom(tilemapPosition);
        yield return new WaitForSeconds(0.75f);

        tilemapToDisable.SetActive(false);

        yield return new WaitForSeconds(2.0f);
        mainCamera.GetComponent<CameraControl>().SwitchToPlayerFocus();
    }







    // TEST
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Stone") {
            Debug.Log("Colliding with stone");
        }
    }


}
