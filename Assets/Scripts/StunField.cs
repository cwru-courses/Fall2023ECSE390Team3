using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunField : MonoBehaviour
{
    public float duration = 5f;
    public float speedMultiplier = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("trigger Entered");
        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy)
        {
            print("enemy in trigger");
            enemy.stun(duration, speedMultiplier);
        }
    }
}
