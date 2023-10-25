using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPuzzlePoint : MonoBehaviour
{
    [SerializeField] private Color colorWhenUntrigged = Color.white;
    [SerializeField] private Color colorWhenTrigged = Color.blue;
    [SerializeField] private Color colorWhenFixed = Color.red;
    [SerializeField] private GameObject lastPoint;
    [SerializeField] private GameObject nextPoint;
    [SerializeField] private GameObject playerFoot;
    [SerializeField] private GameObject puzzleControllerObject;
    [SerializeField] private GameObject lineControllerObject;
    private SpriteRenderer spriteRenderer;
    // trigger when player have walked on it, but haven't reached the next point
    private bool triggered = false;
    // fixed when player have reached the next point
    private bool fixation = false;
    private YarnPuzzleController puzzleController;
    private YarnLineController lineController;

    // Start is called before the first frame update
    void Start()
    {
        // get SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        puzzleController = puzzleControllerObject.GetComponent<YarnPuzzleController>();
        lineController = lineControllerObject.GetComponent<YarnLineController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerFoot != null)
        {
            if (!triggered)
            {
                Vector3 targetPosition = playerFoot.transform.position;
                Vector3 myPosition = transform.position;
                // only calculate distance on x and y
                float distanceX = Mathf.Abs(targetPosition.x - myPosition.x);
                float distanceY = Mathf.Abs(targetPosition.y - myPosition.y);
                float distance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

                if (distance <= 0.2f)
                {
                    // change color of the circle
                    spriteRenderer.color = colorWhenTrigged;
                    // clear yarn trail
                    YarnTrail._instance.ClearYarnTrail();
                    // update trigged attribute
                    triggered = true;
                    // update the onPoint attribute in YarnPuzzleController
                    puzzleController.increaseOnPoint();
                    // if there is a last point, connect to it
                    if (lastPoint != null)
                    {
                        ConnectToLastPoint();
                    }
                    // if there is a next point, reveal(activate) it
                    if (nextPoint != null)
                    {
                        RevealNextPoint();
                    }
                    else
                    {
                        // if it is the final point, it should be marked as fixed directly
                        SetFixation(true);
                    }
                }
            }
            // this happens when player first activate it then sent back to normal world by enemy cutting trail, then player needs to activate it again
            if (!triggered && (nextPoint != null && nextPoint.activeSelf))
            {
                // set inactive the next point
                nextPoint.SetActive(false);
                // reset color of the circle to untrigged
                spriteRenderer.color = colorWhenUntrigged;
            }
            // if it is fixed, and color hasn't been changed
            if (fixation && spriteRenderer.color != colorWhenFixed)
            {
                spriteRenderer.color = colorWhenFixed;
            }
        }
        
    }

    private void ConnectToLastPoint()
    {
        lineController.ConnectPoints(lastPoint.transform.position, transform.position);
        lastPoint.GetComponent<YarnPuzzlePoint>().SetFixation(true);
    }

    private void RevealNextPoint()
    {
        nextPoint.SetActive(true);
    }

    public void Untrigger()
    {
        triggered = false;
        Debug.Log("untriggered");
    }

    public void SetFixation(bool value)
    {
        fixation = value;
    }

    public bool GetFixation()
    {
        return fixation;
    }

}
