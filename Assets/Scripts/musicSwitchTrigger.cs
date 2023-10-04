using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitchTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip track;
    [SerializeField] private Collider2D trig;
    [SerializeField] private AmbientSystem theAS;

    // Start is called before the first frame update
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
        Debug.Log("start fight");
        if(other ==trig)
        {
            if(track!=null)
            {
                theAS.changeMusic(track);
            }
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("end fight");
        if(other ==trig)
        {
            if(track!=null)
            {
                theAS.playOG();
            }
            
        }
    }
}
