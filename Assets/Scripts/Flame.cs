using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {

    // This flame game object
    [SerializeField] private GameObject flame;

    // Collider of flame game object
    private Collider2D flameCollider;


    // Start is called before the first frame update
    void Start() {
        flameCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    // If player makes contact with flame collider, start coroutine to inflict increasing damage
    private void OnTriggerEnter2D(Collider2D other) { 
        if (other.tag == "Player") {
            StartCoroutine(Burn());
        }
    }


    // While player is in contact with flame collider, do increasing damage
    private IEnumerator Burn() {

        // just something for now
        yield return new WaitForSeconds(1.0f);
    }


}
