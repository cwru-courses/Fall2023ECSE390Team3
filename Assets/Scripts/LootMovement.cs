using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootMovement : MonoBehaviour
{
    [SerializeField] private int lootType; // Jing - 1 for potion, 2 for yarn
    [SerializeField] private float yarnRewardIfYarn;
    [SerializeField] private float detectionDistance = 2f;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float destroyDistance = 0.3f;
    [SerializeField] private LootController lootController;
    private Rigidbody2D rb2d;
    GameObject target;
    private bool detectedTarget = false;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        lootController.enabled = false;
        rb2d.gravityScale = 0f;
        rb2d.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            Vector3 myPosition = transform.position;
            float distance = Vector3.Distance(targetPosition, myPosition);

            if (distance < destroyDistance)
            {
                GiveReward();
                Destroy(gameObject);
            }
            else if (detectedTarget == false && distance < detectionDistance)
            {
                detectedTarget = true;
            }
            else if (detectedTarget == true)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(myPosition, targetPosition, step);
            }

        }
    }

    private void GiveReward()
    {
        if (lootType == 1)
        {
            PlayerStats._instance.GainPotion();
        }
        else if (lootType == 2)
        {
            PlayerStats._instance.GainYarn(yarnRewardIfYarn);
        }
    }
}


