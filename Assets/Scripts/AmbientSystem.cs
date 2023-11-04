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
    [SerializeField] private AudioClip mainClip;
    [SerializeField] private AudioClip reversedClip;
    [SerializeField] private BoxCollider2D fight;
    [SerializeField] private CameraControl cam;
    [SerializeField] private MusicSwitchTrigger boss;
    [SerializeField] private AudioSource forwardAudioSource;
    [SerializeField] private AudioSource reversedAudioSource;

    private float currentTime;
    private float totalTime;
    private bool isPlayingForward = true;
    private float crossfadeDuration = 1.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        forwardAudioSource.clip = mainClip;
        reversedAudioSource.clip = reversedClip;

        totalTime = mainClip.length;
    }

    public void OnPhaseShift()
    {
        Debug.Log("Restarting timer");
        boss.roomCenterPosition = new Vector3(boss.roomCenterPosition.x, -boss.roomCenterPosition.y, boss.roomCenterPosition.z);
        CameraControl.SwitchSide();

        if (isPlayingForward)
        {
            currentTime = forwardAudioSource.time;
            //StartCoroutine(SwitchAudioWithCrossfade(currentTime));
        }
        else
        {
            currentTime = totalTime - reversedAudioSource.time;
           // StartCoroutine(SwitchAudioWithCrossfade(currentTime));
        }

        SwitchAudioSource();

    }

    private void SwitchAudioSource()
    {
        if (isPlayingForward)
        {
            forwardAudioSource.Stop();
            reversedAudioSource.time = totalTime - currentTime;
            reversedAudioSource.Play();
        }
        else
        {
            reversedAudioSource.Stop();
            forwardAudioSource.time = currentTime;
            forwardAudioSource.Play();
        }

        isPlayingForward = !isPlayingForward;
    }

/* private IEnumerator SwitchAudioWithCrossfade(float targetTime)
 {
     Debug.Log("Target Time: " + targetTime);

     float startForwardVolume = forwardAudioSource.volume;
     float startReversedVolume = reversedAudioSource.volume;
     float timer = 0f;

     while (timer < crossfadeDuration)
     {
         timer += Time.deltaTime;

         // Crossfade by adjusting the volume of the audio sources
         forwardAudioSource.volume = Mathf.Lerp(startForwardVolume, 0, timer / crossfadeDuration);
         reversedAudioSource.volume = Mathf.Lerp(startReversedVolume, 0, timer / crossfadeDuration);

         yield return null;
     }

     // Stop the source that is not playing
     if (isPlayingForward)
     {
         forwardAudioSource.Stop();
         reversedAudioSource.time = totalTime - targetTime;
         reversedAudioSource.Play();
     }
     else
     {
         reversedAudioSource.Stop();
         forwardAudioSource.time = targetTime;
         forwardAudioSource.Play();
     }

     isPlayingForward = !isPlayingForward;
 }*/

public float GetVerticalDistMultiplier()
    {
        return verticalDistMultiplier;
    }

    // Function to switch between audio clips
    private void SwitchAudioClip(AudioClip nextClip)
    {
        if (nextClip == null || nextClip == forwardAudioSource.clip)
        {
            return;
        }

        forwardAudioSource.Stop();
        forwardAudioSource.clip = nextClip;
        forwardAudioSource.Play();
    }

    public void changeMusic(AudioClip nextTrack)
    {
        if (nextTrack == null || nextTrack == forwardAudioSource.clip)
        {
            return;
        }
        forwardAudioSource.Stop();
        forwardAudioSource.clip = nextTrack;
        forwardAudioSource.Play();
    }

    public void playOG()
    {
        if (mainClip == forwardAudioSource.clip)
        {
            return;
        }
        forwardAudioSource.Stop();
        forwardAudioSource.clip = mainClip;
        forwardAudioSource.Play();
    }
}
