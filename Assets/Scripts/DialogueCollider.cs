using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollider : MonoBehaviour
{
    public CollisionDialogue collisionDialogue;
    [SerializeField] GameObject dialogueBox;

    bool collided;

    void Start(){
        collided = false;
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player") == true && !collided){
            dialogueBox.SetActive(true);
            collisionDialogue.StartRunning(dialogueBox);
            collided = true;
        }
    }
}
