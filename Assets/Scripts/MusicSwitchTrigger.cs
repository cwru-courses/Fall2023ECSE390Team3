// using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitchTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip track;
    [SerializeField] private Collider2D trig;
    [SerializeField] private AmbientSystem theAS;

    [SerializeField] private bool boss = false;
    [SerializeField] private CameraControl cam;
    [SerializeField] public Vector3 roomCenterPosition;    
    void Start()
    {
        // theAS = FindObjectOfType<AmbientSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("start fight");
        if(other == trig)
        {
            if(track!=null)
            {
                theAS.changeMusic(track);
                if(boss)
                {
                    cam.SwitchToBossRoom(roomCenterPosition);
                }
            }
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log("end fight");
        if(other ==trig)
        {
            if(track!=null)
            {
                theAS.playOG();
                if(boss)
                {
                    cam.SwitchToPlayerFocus();
                }
            }
            
        }
    }
}
