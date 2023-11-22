using System.Collections;
using UnityEngine;

public class wallOpenClose : MonoBehaviour
{
    [SerializeField] Vector3 toOpenedPosition;
    [SerializeField] float openDuration = 3.0f;
    Vector3 lockedPosition;
    Vector3 openedPosition;
    float closeDuration = 0.5f;

    [SerializeField] private bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        lockedPosition = transform.position;
        openedPosition = lockedPosition + toOpenedPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator toOpen()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < openDuration)
        {
            transform.position = Vector3.Lerp(startingPosition, openedPosition, elapsedTime / openDuration);
            yield return null; // wait for one frame
            elapsedTime += Time.deltaTime;
        }

        transform.position = openedPosition; // make sure the position at the end is the openedPosition

        opened = true;
    }

    public void toClose()
    {
        transform.position = lockedPosition;

        /*
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < closeDuration)
        {
            Debug.Log("close");
            transform.position = Vector3.Lerp(startingPosition, lockedPosition, elapsedTime / closeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // wait for one frame
        }

        transform.position = lockedPosition; // make sure the position at the end is the openedPosition
        */

        opened = false;
    }

    public bool isOpened()
    {
        return opened;
    }

    public void SetDoorsOnLoad(bool toOpen)
    {
        if (toOpen)
        {
            Debug.Log("Door Opened On Loading Save");
            StartCoroutine("toOpen");
        }
        else
        {
            Debug.Log("Door Close On Loading Save");
            toClose();
        }
    }
}
