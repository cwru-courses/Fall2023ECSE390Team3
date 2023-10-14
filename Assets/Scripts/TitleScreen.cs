using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//
public class TitleScreen : MonoBehaviour
{
    public void Play(){
        //load next scene
        SceneManager.LoadSceneAsync("Tutorial Level 1");
    }

    public void NavigateToLoadScreen() {
        SceneManager.LoadSceneAsync("Load Screen");
    }

    public void Quit(){
        Application.Quit();
        //make sure it works without having to quit.
        Debug.Log("Player Has Quit The Game");
    }

}
