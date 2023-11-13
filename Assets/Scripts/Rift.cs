using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A script that would activate stitch/shift when a player makes contact with a rift.
 * I don't think it is going to end up finished and being used, but I figure I shouldn't
 * delete it. */


public class Rift : MonoBehaviour {

    // Player and collider of player
    private GameObject player;
    private Collider2D playerCollider;

    // Collider of this rift game object
    private Collider2D riftCollider;

    private DefaultInputAction playerInputAction;

    void Awake()
    {
        playerInputAction = new DefaultInputAction();
    }

    // Start is called before the first frame update
    void Start() {
        riftCollider = GetComponent<Collider2D>();
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<Collider2D>();
    }

    void Update() {

    }


    // If player makes contact with rift for 2 seconds, trigger shift/stitch
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            StartCoroutine(UseRift());
        }
    }


    private IEnumerator UseRift() {
        //if (playerInputAction.Player.Ability1.phase == phase.performed) {
            // trigger shift/stitch

        yield return new WaitForSeconds(2.0f);

        /*
        if (playerCollider.IsTouching(riftCollider)) {

        }
        else
            yield return null;
        */
    }


}
