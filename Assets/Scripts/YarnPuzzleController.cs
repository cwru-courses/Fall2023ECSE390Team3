using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzleController : MonoBehaviour
{
    //public static YarnPuzzleController _instance;
    [SerializeField] private YarnPuzzlePointFlipped[] pointsArray;
    [SerializeField] private GameObject yarnLineControllerObject;
    [SerializeField] private int totalPoints;
    [SerializeField] private GameObject wallToRemove1;
    [SerializeField] private GameObject wallToRemove2;
    [SerializeField] private AudioSource wallUp;
    [SerializeField] private GameObject scratch1;
    [SerializeField] private GameObject scratch2;
    [SerializeField] private CameraControl came;
    private bool puzzleActive = true;
    private int onPoint = -1;
    private Animator scratch1_ani;
    private Animator scratch2_ani;

    // Start is called before the first frame update
    void Start()
    {
        if (scratch1 != null)
        {
            scratch1_ani = scratch1.GetComponent<Animator>();
        }
        if (scratch2 != null)
        {
            scratch2_ani = scratch2.GetComponent<Animator>();
        }

        Debug.Log(wallToRemove1.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseOnPoint()
    {
        onPoint++;
        if (onPoint == totalPoints - 1)
        {
            puzzleActive = false;
            Debug.Log("reached the end of the puzzle");
            StartCoroutine(PuzzleReward());
        }
    }

    public bool PuzzleActive()
    {
        Debug.Log(puzzleActive);
        return puzzleActive;
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

    private IEnumerator PuzzleReward()
    {
        yield return new WaitForSeconds(2f);
        if (came != null)
        {
            Transform scratchTrans = scratch1_ani.transform;
            Vector3 watchScratch = new Vector3(scratchTrans.position.x, scratchTrans.position.y, 0);
            
            // watch scratch to close first
            came.SwitchToBossRoom(watchScratch);
            if (scratch1_ani != null)
            {
                scratch1_ani.SetBool("closed", true);
            }
            if (scratch2_ani != null)
            {
                scratch2_ani.SetBool("closed", true);
            }
            yield return new WaitForSeconds(0.5f);

            Transform wallTrans = wallToRemove1.transform;
            Vector3 watchWall = new Vector3(wallTrans.position.x, wallTrans.position.y, 0);
            Debug.Log(watchWall);
            // watch door to open next
            came.SwitchToBossRoom(watchWall);
            if (wallToRemove1 != null)
            {
                wallToRemove1.SetActive(false);
            }
            if (wallToRemove2 != null)
            {
                wallToRemove2.SetActive(false);
            }
            if (wallUp != null)
            {
                wallUp.Play();
            }
            yield return new WaitForSeconds(5f);

            // get back to player
            came.SwitchToPlayerFocus();
        }

    }
}
