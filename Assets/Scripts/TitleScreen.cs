using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//
public class TitleScreen : MonoBehaviour
{
    public void Play(){
        //if have saved 3 files, should go to load screen
        int numSaved = SaveSystem.GetNumberSavedFiles();
        if(numSaved >= 3) {
            SceneManager.LoadSceneAsync("Load Screen");
        } else {
            //load next scene
            SaveSystem.currentFileName = "save" + numSaved; 
            SceneManager.LoadSceneAsync("Tutorial Level 1");
        }
    }

    public void NavigateToLoadScreen() {
        SceneManager.LoadSceneAsync("Load Screen");
    }

    public void NavigateToCreditsScreen()
    {
        SceneManager.LoadSceneAsync("Credits Screen");
    }

    public void Quit(){
        Application.Quit();
        //make sure it works without having to quit.
        Debug.Log("Player Has Quit The Game");
    }

}
