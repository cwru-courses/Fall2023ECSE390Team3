using System;

[Serializable]
public class SaveUnit
{
    public float[] playerPosition;
    public float playerHealth;
    public float playerMana;

    public SaveUnit()
    {
        // Load data to be saved

        // Test
        playerPosition = new float[3] { 10.0f, 10.0f, 0.0f };
        playerHealth = 100.0f;
        playerMana = 10.0f;
    }
}
