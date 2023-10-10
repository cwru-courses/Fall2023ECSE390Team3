using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    public float attackDuration = 0.1f;
    public float attackRangeAngle = 45f;
    private float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        float swingPercent = (Time.deltaTime) / attackDuration;
        transform.Rotate(0f, 0f, swingPercent * attackRangeAngle);
        transform.position = transform.position;

    }
}
