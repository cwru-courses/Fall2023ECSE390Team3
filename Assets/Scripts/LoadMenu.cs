using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    [SerializeField] GameObject loadFileSlotPrefab; 
    [SerializeField] GameObject newFileSlotPrefab; 

    //set correct button to show depending on if there is a version saved? 
    void Start() {
        if(SaveSystem.isVersionSaved) {
            newFileSlotPrefab.gameObject.SetActive(false); 
            loadFileSlotPrefab.gameObject.SetActive(true); 
        } else {
            newFileSlotPrefab.gameObject.SetActive(true); 
            loadFileSlotPrefab.gameObject.SetActive(false); 
        }
    }

    public void BackToHome() {
        SceneManager.LoadSceneAsync("Home_screen_scene");
    }

    public void StartNewGame() {
        SceneManager.LoadSceneAsync("Tutorial Level 1");
    }
}
