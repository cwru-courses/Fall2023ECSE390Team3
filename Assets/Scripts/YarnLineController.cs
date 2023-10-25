using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnLineController : MonoBehaviour
{
    [SerializeField] private Color colorAfterCompletion = Color.red;
    [SerializeField] private int maxLimit;
    private LineRenderer lineRenderer;
    private int onPosition;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        onPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer.positionCount == maxLimit)
        {
            lineRenderer.startColor = colorAfterCompletion;
            lineRenderer.endColor = colorAfterCompletion;
        }
    }

    public void ConnectPoints(Vector3 startPoint, Vector3 endPoint)
    {
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(onPosition, startPoint);
        lineRenderer.SetPosition(onPosition + 1, endPoint);
        onPosition++;
    }

    public void removeLastLine()
    {
        if (onPosition > 0)
        {
            lineRenderer.positionCount -= 1;
            onPosition--;
        }
        Debug.Log("Line Controller on: " + onPosition);
    } 
}
