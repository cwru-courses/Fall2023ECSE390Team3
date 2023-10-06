using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingGrow : MonoBehaviour
{

    private float startTime;
    public float growDuration = 0.5f;
    private float maxScale = 0.12f;
    
    // Start is called before the first frame update
    void Awake()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float size = Mathf.Min((Time.time - startTime) / growDuration, 1)*maxScale;
        transform.localScale = new Vector3(size, size, size);
    }
}
