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
            Player.Instance.transform.position.x,
            Player.Instance.transform.position.y,
            Player.Instance.transform.position.z
        };
        playerHealth = Player.Instance.currentHealth;
    }
}
