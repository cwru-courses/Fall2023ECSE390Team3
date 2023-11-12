using UnityEngine; 
using System;
using System.Collections;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveUnit
{
    public float[] playerPosition;
    public int playerHealth;
    public float playerYarn; 
    public int potions; 
    public string currentSceneName; 
    public bool[] levelsReached; 
    //public float playerMana;

    public SaveUnit()
    {
        // Load data to be saved
        playerPosition = new float[3] {
            PlayerStats._instance.transform.position.x,
            PlayerStats._instance.transform.position.y,
            PlayerStats._instance.transform.position.z
        };
        playerHealth = PlayerStats._instance.currentHealth;
        playerYarn = PlayerStats._instance.currentYarnCount; 
        potions = PlayerStats._instance.potions; 
        currentSceneName = SceneManager.GetActiveScene().name;
        levelsReached =  PlayerStats._instance.levelsReached; 
    }
}
