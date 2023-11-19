using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzleController : MonoBehaviour
{
    //public static YarnPuzzleController _instance;
    [SerializeField] private GameObject[] pointsArray;
    [SerializeField] private int totalPoints;
    [SerializeField] private wallOpenClose wallToRemove1;
    [SerializeField] private wallOpenClose wallToRemove2;
    [SerializeField] private AudioSource wallUp;
    [SerializeField] private GameObject scratch1;
    [SerializeField] private GameObject scratch2;
    [SerializeField] private CameraControl came;
    [SerializeField] private Vector3 wallPosition;
    [SerializeField] private float timeForDoorOpen = 3f;
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
            Debug.Log(111111111);
            if (pointsArray[onPoint].GetComponent<YarnPuzzlePointNormal>() != null)
            {
                Debug.Log("Normal Decreased");
                pointsArray[onPoint].GetComponent<YarnPuzzlePointNormal>().DecreaseStage();
            }
            else if (pointsArray[onPoint].GetComponent<YarnPuzzlePointFlipped>() != null)
            {
                Debug.Log("Flipped Decreased");
                pointsArray[onPoint].GetComponent<YarnPuzzlePointFlipped>().DecreaseStage();
            }
        }
    }

    private IEnumerator PuzzleReward()
    {
        yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(1.5f);

            
            Vector3 watchWall = new Vector3(wallPosition.x, wallPosition.y, 0);
            // watch door to open next
            came.SwitchToBossRoom(watchWall);
            if (wallToRemove1 != null)
            {
                StartCoroutine(wallToRemove1.toOpen());
            }
            if (wallToRemove2 != null)
            {
                StartCoroutine(wallToRemove2.toOpen());
            }
            if (wallUp != null)
            {
                wallUp.Play();
            }
            yield return new WaitForSeconds(timeForDoorOpen + 0.5f);

            // get back to player
            came.SwitchToPlayerFocus();
        }

    }
}
