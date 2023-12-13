using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodeMovePlayer : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position = targetPos;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(targetPos, 1f);
    }
}
