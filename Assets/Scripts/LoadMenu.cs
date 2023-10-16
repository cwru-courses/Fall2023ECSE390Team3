using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    public GameObject[] startNewButtons;
    public GameObject[] loadButtons;  
    public GameObject[] deleteButtons; 

    void Start() {
        for(int i = 0; i < 3; i++) {
            //if the slot name is not empty, set load buttons to be true
            if(!string.Equals(SaveSystem.listSavedFiles[i], "")) {
                startNewButtons[i].gameObject.SetActive(false); 
                loadButtons[i].gameObject.SetActive(true); 
                deleteButtons[i].gameObject.SetActive(true); 
            } else {
                startNewButtons[i].gameObject.SetActive(true); 
                loadButtons[i].gameObject.SetActive(false); 
                deleteButtons[i].gameObject.SetActive(false); 
            }
        }
    }

    public void BackToHome() {
        SceneManager.LoadSceneAsync("Home_screen_scene");
    }

    public void StartNewGame() {
        SceneManager.LoadSceneAsync("Tutorial Level 1");
    }

    public void ChangeButtonsAfterDelete(int index) {
        //after delete, should go back to start screen
        startNewButtons[index].gameObject.SetActive(true); 
        loadButtons[index].gameObject.SetActive(false); 
        deleteButtons[index].gameObject.SetActive(false); 
    } 

    //will other levels be in different scenes? - load specific scene
    // public void LoadGame() {

    // }
}
