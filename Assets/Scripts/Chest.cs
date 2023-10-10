using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    // [SerializeField] private *list / data structure for items*
    [SerializeField] private SpriteRenderer chest;
    [SerializeField] private Sprite openChest;
    [SerializeField] private Sprite closedChest;


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    // Set the sprite to an open chest or a closed chest
    public void setChestOpen(bool boolean) { 
        if (boolean == true) {
            chest.sprite = openChest;
        }
        else {
            chest.sprite = closedChest;
        }

    }

}
