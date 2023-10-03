using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSystem : MonoBehaviour
{
    public static AmbientSystem Instance;

    [Header("World Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float verticalDistMultiplier;
    [Header("Music Settings")]
    [SerializeField] private int startingPitch = 1;

    private int pitchshift = -1;
    private AudioSource audSource;

    void Awake()
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

    public float GetVerticalDistMultiplier()
    {
        return verticalDistMultiplier;
    }
}
