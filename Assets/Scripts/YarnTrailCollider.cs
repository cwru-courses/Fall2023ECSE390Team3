using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class YarnTrailCollider : MonoBehaviour
{
    TrailRenderer yarnTrail;
    EdgeCollider2D yarnTrailCollider;

    private void Awake()
    {
        yarnTrail = this.GetComponent<TrailRenderer>();
        GameObject colliderGameObject = new GameObject("TrailCollider", typeof(EdgeCollider2D), typeof(YarnTrailEnemyDetection));
        colliderGameObject.layer = 15;
        yarnTrailCollider = colliderGameObject.GetComponent<EdgeCollider2D>();
        yarnTrailCollider.isTrigger = true;
        //colliderGameObject.AddComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetColliderPointsFromTrail(yarnTrail, yarnTrailCollider);
    }

    void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        for (int position = 0; position < trail.positionCount; position++)
        {
            // ignore z axis when translating vector3 to vector2
            points.Add(trail.GetPosition(position));
        }
        collider.SetPoints(points);
    }

    // technically not really clear it, but just set the collider to somewhere player never reaches
    public void ClearColliderPoints()
    {
        // add two points that the player is never going to hit
        List<Vector2> points = new List<Vector2>();
        Vector2 point1 = new Vector2(1000, 1000); 
        points.Add(point1);
        Vector2 point2 = new Vector2(1001, 1001); 
        points.Add(point2);
        yarnTrailCollider.SetPoints(points);
    }
}
