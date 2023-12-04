using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed=1f;
    public int damage = 1;
    public int attackLayer = 15;
    public int ignoreLayer = 20;
    public bool triggerBossPull = false;
    public bool triggerBossPull2 = false;

    const int playerLayer = 15;
    const int enemyLayer = 20;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        // check if hit intended layer
        if (hit.layer == attackLayer)
        {
            // if player player takes damage
            if(attackLayer == playerLayer)
            {
                PlayerStats player = hit.GetComponent<PlayerStats>();
                if (player)
                {
                    if(triggerBossPull){
                        StartCoroutine(BossController._instance.TriggerPull());
                    }
                    if(triggerBossPull2){
                        StartCoroutine(Boss2Controller._instance.TriggerPull());
                    }
                    player.TakeDamage(damage);
                }
            }
            //if enemy enemy takes damage
            else if(attackLayer == enemyLayer)
            {
                BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
                if (enemy)
                {
                    enemy.ReactToHit(damage);
                }
            }
           
        }
        // unless the hit layer is the ignore layer destroy projectile on impact
        if(hit.layer != ignoreLayer)
        {
            Destroy(this.gameObject);
        }
        
    }
}
