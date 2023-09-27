using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSystem : MonoBehaviour
{
    public static AmbientSystem Instance;

    [SerializeField] private int startingPitch = 1;

    private int pitchshift = -1;
    private AudioSource audSource;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        audSource = GetComponent<AudioSource>();
        audSource.pitch = startingPitch;
    }

    public void OnPhaseShift()
    {
        audSource.pitch *= pitchshift;
    }
}
