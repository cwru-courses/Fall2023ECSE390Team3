using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{

    public void BackToHome()
    {
        SceneManager.LoadSceneAsync("Home_screen_scene");
    }

}