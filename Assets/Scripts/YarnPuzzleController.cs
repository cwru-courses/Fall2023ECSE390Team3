using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzleController : MonoBehaviour
{
    public static YarnPuzzleController _instance;
    [SerializeField] private YarnPuzzlePoint[] pointsArray;
    [SerializeField] private GameObject yarnLineControllerObject;
    private bool puzzleActive = true;
    private int onPoint = -1;
    private YarnLineController lineController;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        lineController = yarnLineControllerObject.GetComponent<YarnLineController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseOnPoint()
    {
        onPoint++;
        if (onPoint == pointsArray.Length - 1)
        {
            puzzleActive = false;
        }
        Debug.Log("increase to " + onPoint);
    }

    /*
     * this happens when enemy collide with yarn trail and take player back to normal world
     * current point that is trigged needs to be reset to untriggered so player needs to activate it again
    */
    public void decreaseOnPoint()
    {
        if (onPoint >= 0 && puzzleActive)
        {
            if (pointsArray[onPoint].GetFixation() == false)
            {
                pointsArray[onPoint].Untrigger();
                onPoint--;
                lineController.removeLastLine();
                Debug.Log("decrease to " + onPoint);
            }
        }
    }
}
