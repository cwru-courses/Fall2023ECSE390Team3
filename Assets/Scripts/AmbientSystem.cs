using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSystem : MonoBehaviour
{
    public static AmbientSystem Instance;

    [SerializeField] private int startingPitch = 1;
    [SerializeField] private AudioClip mainClip;
    [SerializeField] private BoxCollider2D fight;


    private int pitchshift = -1;
    [SerializeField] private AudioSource audSource;

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
    
    public void changeMusic(AudioClip next_track){
        if(next_track.name==audSource.clip.name)
        {
            return;
        }
        audSource.Stop();
        audSource.clip = next_track;
        audSource.Play();
        
    }
    public void playOG(){
        if(mainClip.name==audSource.clip.name)
        {
            return;
        }
        audSource.Stop();
        audSource.clip = mainClip;
        audSource.Play();
        
    }
	
}
