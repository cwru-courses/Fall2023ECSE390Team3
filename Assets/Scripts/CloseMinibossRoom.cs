using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMinibossRoom : MonoBehaviour
{
    [SerializeField] private GameObject wall1;
    [SerializeField] private GameObject wall2;
    [SerializeField] private AudioSource wallOff;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (wall1 != null)
            {
                wall1.SetActive(true);
            }
            if (wall2 != null)
            {
                wall2.SetActive(true);
            }
            if (wallOff != null)
            {
                wallOff.Play();
            }
            gameObject.SetActive(false);
        }
    }
}
