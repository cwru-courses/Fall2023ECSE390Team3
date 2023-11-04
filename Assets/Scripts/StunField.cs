using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunField : MonoBehaviour
{
    public float duration = 5f;
    public float speedMultiplier = 0.1f;
    private List<BaseEnemy> trappedEnemies;
    public float damageCD = 1.5f;
    private float lastDamageTime;
    // Start is called before the first frame update
    void Start()
    {
        lastDamageTime = Time.time-damageCD;
        trappedEnemies = new List<BaseEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastDamageTime >= damageCD)
        {
            lastDamageTime = Time.time;
            foreach(BaseEnemy enemy in trappedEnemies){
                enemy.ReactToHit(1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy)
        {
            trappedEnemies.Add(enemy);
            enemy.stun(duration, speedMultiplier);
        }
    }
}
