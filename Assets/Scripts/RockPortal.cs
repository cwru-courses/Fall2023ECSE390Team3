using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make rock fade out and in when doing stitch / phase shift
public class RockPortal : MonoBehaviour {

    private SpriteRenderer rockSR;

    private float startTime;
    [SerializeField] private float timeToFade;  // was lifetime in other script
    [SerializeField] private bool fadingIn = false;
    [SerializeField] private bool fadingOut = false;


    // Start is called before the first frame update
    void Start() {
        rockSR = GetComponent<SpriteRenderer>();
    }

    
    // Start CallRockFade coroutine
    public void CallRockFade() {
        Debug.Log("Called Rock Fade");
        StartCoroutine(RockFade());
    }


    // Update is called once per frame
    void Update() {

       if (fadingOut == true) {
            float lifePercent = (Time.time - startTime) / timeToFade;
            float currOpacity = 1f - lifePercent;
            rockSR.color = new Color(1, 1, 1, currOpacity);
       }

       if (fadingIn == true) {
            float lifePercent = (Time.time - startTime) / timeToFade;
            float currOpacity = 1f + lifePercent;
            rockSR.color = new Color(1, 1, 1, currOpacity);
       }

    }


    private IEnumerator RockFade() {
        startTime = Time.time;
        fadingOut = true;
        yield return new WaitForSeconds(timeToFade);
        fadingOut = false;

        // other delay if need be
        yield return new WaitForSeconds(1.0f);

        startTime = Time.time;
        fadingIn = true;
        yield return new WaitForSeconds(timeToFade);
        fadingIn = false;
    }


}
