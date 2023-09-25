using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PostDeathEntity : MonoBehaviour
{
    [SerializeField] private float lifetime;
    [SerializeField] private float speed;

    private SpriteRenderer SR;
    private float startTime;
    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        //initial values
        SR = GetComponent<SpriteRenderer>();
        startTime = Time.time;
        moveDir = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f);
        moveDir = moveDir.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //move entity to the left
        Vector3 currPos = transform.position;
        currPos += (moveDir * speed * Time.deltaTime);
        transform.position = currPos;

        //fade opacity
        float lifePercent = (Time.time - startTime) / lifetime;
        float currOpacity = 1f - lifePercent;
        SR.color = new Color(1, 1, 1, currOpacity);
    }

    public float getLifetime()
    {
        return lifetime;
    }
}
