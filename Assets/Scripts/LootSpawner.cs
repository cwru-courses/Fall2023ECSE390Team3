using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthPotionPrefab;
    [SerializeField] private GameObject yarnDropPrefab;
    [SerializeField][Min(0)] private float horizontalPopForce;
    [SerializeField][Min(0)] private float verticalPopForce;

    // Temp
    [SerializeField][Min(0)] private int healthPotionDropAmount;
    [SerializeField][Min(0)] private int yarnDropAmount;

    //void Awake()
    //{
    //    //SpawnLoot(healthPotionDropAmount, yarnDropAmount);
    //}

    private void Update()
    {
        if (transform.childCount < 1)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnLoot(int numHealthPotion, int numYarn)
    {
        for (int i = 0; i < numHealthPotion; i++)
        {
            GameObject loot = Instantiate(healthPotionPrefab, transform);
            loot.transform.position = transform.position;
            loot.GetComponent<LootController>().Init(
                Random.Range(-1f, 1f) * horizontalPopForce,
                Random.Range(0.5f, 1f) * verticalPopForce
            );
        }
        for (int i = 0; i < numYarn; i++)
        {
            GameObject loot = Instantiate(yarnDropPrefab, transform);
            loot.transform.position = transform.position;
            loot.GetComponent<LootController>().Init(
                Random.Range(-1f, 1f) * horizontalPopForce,
                Random.Range(0.5f, 1f) * verticalPopForce
            );
        }
    }
}
