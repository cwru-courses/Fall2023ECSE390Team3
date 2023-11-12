using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveCollide : MonoBehaviour
{
    [SerializeField] GameObject autoSaveText;

    bool collided;

    void Start()
    {
        collided = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") == true && !collided)
        {
            // autoSaveText.SetActive(true);
            SaveSystem.CreateSave(); 
            StartCoroutine(ShowAutoSave()); 
            collided = true;
        }
    }

    private IEnumerator ShowAutoSave() {
        autoSaveText.SetActive(true);
        yield return new WaitForSeconds(2f); 
        autoSaveText.SetActive(false);
    }
}
