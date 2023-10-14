using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LootController : MonoBehaviour
{
    [SerializeField] private LootMovement lootMovement;
    private float maxHorizontalPopForce = 0f;
    private float maxVerticalPopForce = 0f;
    private float verticalStartH = 0f;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < verticalStartH)
        {
            transform.position = new Vector3(transform.position.x, verticalStartH, transform.position.z);
            maxHorizontalPopForce *= 0.5f;
            maxVerticalPopForce *= 0.5f;
            if (maxVerticalPopForce < 0.1f)
            {
                rb2d.velocity = Vector2.zero;
                lootMovement.enabled = true;
            }
            else
            {
                rb2d.velocity = new Vector2(maxHorizontalPopForce, maxVerticalPopForce);
            }
        }
    }

    public void Init(float hPopForce, float vPopForce)
    {
        rb2d.velocity = new Vector2(hPopForce, vPopForce);
        maxHorizontalPopForce = hPopForce;
        maxVerticalPopForce = vPopForce;
        verticalStartH = transform.position.y;
    }
}
