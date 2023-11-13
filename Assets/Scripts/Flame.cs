using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {

    // Player and collider of player
    private GameObject player;
    private Collider2D playerCollider;

    // Collider of this flame game object
    private Collider2D flameCollider;

    // Start is called before the first frame update
    void Start() {
        flameCollider = GetComponent<Collider2D>();
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<Collider2D>();
    }


    // If player makes contact with flame collider, start coroutine to inflict increasing damage
    private void OnTriggerEnter2D(Collider2D other) { 
        if (other.tag == "Player") {
            StartCoroutine(Burn());
        }
    }

    // While player is in contact with flame collider, do damage
    private IEnumerator Burn() {
        int damage = 3;

        while (playerCollider.IsTouching(flameCollider)) {
            player.GetComponent<PlayerStats>().TakeDamage(damage);   // player takes damage
            yield return new WaitForSeconds(1.5f);                   // wait half a second
            // damage = damage + 2;                                     // increment damage
        }
        
        yield return null;
    }


    public IEnumerator Extinguish() {
        // Play flame going out animation
        // yield new WaitForSeconds(#);

        Debug.Log("Flame Extinguished");
        Destroy(gameObject);

        yield return null;
    }


    // Things to maybe do:
    // Have an enemy be able to create a flame on the fire side (when it stands still or is attacking)
    // Have a generated flame go through a life cycle and get smaller then go out
    // Have a flame disappear if the player attacks it

}
