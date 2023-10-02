using System;

[Serializable]
public class SaveUnit
{
    public float[] playerPosition;
    public int playerHealth;
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
    }
}
