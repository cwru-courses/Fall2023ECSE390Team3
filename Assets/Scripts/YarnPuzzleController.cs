using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzleController : MonoBehaviour
{
    //public static YarnPuzzleController _instance;
    [SerializeField] private YarnPuzzlePointFlipped[] pointsArray;
    [SerializeField] private GameObject yarnLineControllerObject;
    private bool puzzleActive = true;
    private int onPoint = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseOnPoint()
    {
        onPoint++;
        if (onPoint == pointsArray.Length - 1)
        {
            puzzleActive = false;
        }
    }

    /*
     * this happens when enemy collide with yarn trail and take player back to normal world
     * current point that is trigged needs to be reset to untriggered so player needs to activate it again
    */
    public void TrailCutted()
    {
        if (onPoint >= 0 && puzzleActive)
        {
            // if trail is cutted or shifted back spontaneously, next point(normal) of current point(flipped) needs to decrease from stage 1 to 0
            if (pointsArray[onPoint].GetStage() == 2)
            {
                pointsArray[onPoint].GetNextPoint().GetComponent<YarnPuzzlePointNormal>().DecreaseStage();
            }
        }
    }
}
