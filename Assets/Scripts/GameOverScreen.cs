using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//
public class GameOverScreen : MonoBehaviour
{
    public void Restart(){
        //load next scene
        SceneManager.LoadSceneAsync("Home_screen_scene");
    }

    public void Quit(){
        Application.Quit();
        //make sure it works without having to quit.
        Debug.Log("Player Has Quit The Game");
    }

}
