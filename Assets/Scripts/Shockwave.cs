using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float minRadius;
    public float maxRadius;
    public float speed;
    public int damage ;
    public float hitRadius;
    public LayerMask targetLayer;
    private bool hasHitPlayer =false;
    private float radius= 1f;

    // Start is called before the first frame update
    void Start()
    {
    minRadius = 1f;
    maxRadius = 30f;
    hitRadius = 2f;
    speed = 15f;
    damage = 20;
    hasHitPlayer = false;
    radius = minRadius;
    targetLayer = 15;
}

    void FixedUpdate()
    {
        // update radius
        radius += speed * Time.fixedDeltaTime;
        transform.localScale = new Vector3(radius / 2f, radius / 2f, 1);

        if (radius > maxRadius)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("shockwave hit");
        float dist = (collision.transform.position - transform.position).magnitude;
        //print("dist " + dist);
        //print("radius" + radius);
        //print("hitRad " + hitRadius);
        //print("correct layer "+ (collision.gameObject.layer == targetLayer));
        //print(dist > radius - hitRadius);
        //print(!hasHitPlayer);
        if (collision.gameObject.layer==targetLayer && !hasHitPlayer)
        {
            print("entered if");
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            hasHitPlayer = true;
        }

    }
}
