using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;          
    [SerializeField] private int maxEnemies = 2;      
    [SerializeField] private float spawnInterval = 2f;

    private GameObject[] enemies;     // array to store enemies
    private float nextSpawnTime;
    private bool startedCountDown;
    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new GameObject[maxEnemies];
        position = gameObject.transform.position;
        SpawnEnemies();
        startedCountDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedCountDown && AllEnemiesKilled())
        {
            Debug.Log("1");
            if (Time.time > nextSpawnTime)
            {
                SpawnEnemies();
                startedCountDown = false;
            }
        }
        else if (!startedCountDown && AllEnemiesKilled())
        {
            Debug.Log("2");
            nextSpawnTime = Time.time + spawnInterval;
            startedCountDown = true;
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            Vector3 newPosition = new Vector3(position.x + i, position.y, position.z);
            enemies[i] = Instantiate(enemyPrefab, newPosition, Quaternion.identity);
        }
    }

    private bool AllEnemiesKilled()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                // if enemy is still alive, return false
                return false;
            }
        }
        // all enemies are killed, return true
        return true;
    }

}
