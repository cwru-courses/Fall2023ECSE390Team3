using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMinibossRoom : MonoBehaviour
{
    [SerializeField] private wallOpenClose wall1;
    [SerializeField] private wallOpenClose wall2;
    [SerializeField] private AudioSource wallOff;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (wall1 != null)
            {
                Debug.Log("closing");
                wall1.toClose();
            }
            if (wall2 != null)
            {
                wall2.toClose();
            }
            if (wallOff != null)
            {
                wallOff.Play();
            }
            gameObject.SetActive(false);
        }
    }
}
