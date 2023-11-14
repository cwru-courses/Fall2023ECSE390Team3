using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    [SerializeField] private SpriteRenderer chest;
    [SerializeField] private Sprite openChest;
    [SerializeField] private Sprite closedChest;
    [SerializeField] protected GameObject lootSpawnerPrefab;
    [SerializeField] protected int healthPotionDroprate;
    [SerializeField] protected int yarnDroprate;
    [SerializeField] private AudioSource audSource;
    protected bool lootDropped = false;
    // [SerializeField] private Animation anim;
    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Wake() {
        audSource = GetComponent<AudioSource>();
        audSource.Pause();
        audSource.mute = false;

        // anim = gameObject.GetComponent<Animation>();
        mainCamera = Camera.main;
    }


    // Set the sprite to an open chest or a closed chest
    public void setChestOpen(bool boolean) {
        // Start coroutine to open chest
        if (boolean == true) {
            StartCoroutine(OpenChest());
        }
        // Else, close chest
        else {
            chest.sprite = closedChest;
        }

        // audSource.mute = true;
    }



    // Coroutine to do stuff when open a chest
    private IEnumerator OpenChest() {
        audSource.Play();

        // Camera focus on chest
        mainCamera.GetComponent<CameraControl>().SwitchToObjectFocus(this.transform.position);
        Debug.Log("Should focus on chest");

        chest.sprite = openChest;
        // anim.Play("Chest_open");

        if (lootDropped == false) {
            Instantiate(lootSpawnerPrefab, transform.position, Quaternion.identity)
                .GetComponent<LootSpawner>().SpawnLoot(healthPotionDroprate, yarnDroprate);
            lootDropped = true;
        }

        yield return new WaitForSeconds(3.0f);
        mainCamera.GetComponent<CameraControl>().SwitchToPlayerFocus();
    }




  





}
