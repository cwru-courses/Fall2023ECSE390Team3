using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnTrailEnemyDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameObject player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("needle cutted the trail");
            PhaseShift._instance.StartPhaseShiftByEnemy();
        }
    }
}
