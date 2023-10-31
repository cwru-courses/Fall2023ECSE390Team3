using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    // [SerializeField] private *list / data structure for items*
    [SerializeField] private SpriteRenderer chest;
    [SerializeField] private Sprite openChest;
    [SerializeField] private Sprite closedChest;
    [SerializeField] protected GameObject lootSpawnerPrefab;
    [SerializeField] protected int healthPotionDroprate;
    [SerializeField] protected int yarnDroprate;
    [SerializeField] private AudioSource audSource;
    protected bool lootDropped = false;


    // Start is called before the first frame update
    void Wake() {
        audSource = GetComponent<AudioSource>();
        audSource.Pause();
        audSource.mute = false;
    }

    // Update is called once per frame
    void Update() {
        
    }

    // Set the sprite to an open chest or a closed chest
    public void setChestOpen(bool boolean) { 
        if (boolean == true) {
            
            audSource.Play();
            chest.sprite = openChest;
            
            if (lootDropped == false)
            {
                Instantiate(lootSpawnerPrefab, transform.position, Quaternion.identity)
                    .GetComponent<LootSpawner>().SpawnLoot(healthPotionDroprate, yarnDroprate);
                lootDropped = true;
                
            }
        }
        else {
            chest.sprite = closedChest;
        }

        // audSource.mute = true;
    }

}
