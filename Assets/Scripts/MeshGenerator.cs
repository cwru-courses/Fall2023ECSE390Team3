

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]

public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private PolygonCollider2D polyCollider;
    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        polyCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVertices(Vector3[] newVertices)
    {
        //cannot make mesh with less than 3 vertices
        if (newVertices.Length < 3) { return; }

        // makes choosing triangles much easier
        newVertices = RemoveConcaveVertices(newVertices);
        vertices = newVertices;
        triangles = GenerateTriangles(newVertices);
        UpdateMesh();

    }


    private int[] GenerateTriangles(Vector3[] verts)
    {
        List<int> newTriangles = new List<int>();
        // cant have triangles with less than 3 points
        if (verts.Length < 3)
        {
            return newTriangles.ToArray();
        }

        //determine if points are generated more clockwise or counterclockwise
        float clockwise = getClockwise(verts);

        for (int i = 2; i < verts.Length; i++)
        {
            // need to make sure points are added in clockwise direction otherwise they get culled
            if (clockwise < 0)
            {
                newTriangles.Add(0);
                newTriangles.Add(i - 1);
                newTriangles.Add(i);
            }
            else
            {
                newTriangles.Add(0);
                newTriangles.Add(i);
                newTriangles.Add(i - 1);
            }
        }
        return newTriangles.ToArray();
    }

    private float getClockwise(Vector3[] verts)
    {
        //determine if points are generated more clockwise or counterclockwise
        float clockwise = Vector3.SignedAngle(verts[1] - verts[0], verts[2] - verts[1], Vector3.forward); // - if clockwise, + if counter clockwise
        for (int i = 2; i < verts.Length; i++)
        {
            Vector3 nextPoint;
            if (i == verts.Length - 1)
            {
                nextPoint = verts[0];
            }
            else
            {
                nextPoint = verts[i + 1];
            }
            clockwise += Vector3.SignedAngle(verts[i] - verts[i - 1], nextPoint - verts[i], Vector3.forward);
        }
        clockwise = clockwise / Mathf.Abs(clockwise);
        return clockwise;
    }

    private Vector3[] RemoveConcaveVertices(Vector3[] verts)
    {
        // cant have concavity on less than 4 points
        if (verts.Length <= 3)
        {
            return verts;
        }
        List<Vector3> convexVerts = new List<Vector3>();
        convexVerts.Add(verts[0]);
        convexVerts.Add(verts[1]);
        convexVerts.Add(verts[2]);

        //determine if points are generated more clockwise or counterclockwise
        float clockwise = getClockwise( verts);

        //cull concave points
        Vector3 prevVector = verts[2] - verts[1];
        Vector3 testPoint = verts[3];
        for (int i = 3; i < verts.Length; i++)
        {
            //pick next point
            Vector3 nextPoint;
            if (i == verts.Length - 1)
            {
                nextPoint = verts[0];
            }
            else
            {
                nextPoint = verts[i + 1];
            }

            // find angle of direction change created from previous->curr->next points
            float angle = clockwise * Vector3.SignedAngle(prevVector, nextPoint - testPoint, Vector3.forward);
            // if conforms to clockwise or counterclockwise convention and isnt straight line
            if (angle > 0 && angle!=180)
            {
                convexVerts.Add(testPoint);
                prevVector = nextPoint - testPoint;
            }
            testPoint = nextPoint;
        }
        return convexVerts.ToArray();
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // convert to Vector2 array for collider
        Vector2[] colliderPath = new Vector2[vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            colliderPath[i] = vertices[i];
        }
        polyCollider.SetPath(0,colliderPath);

        //creates UVs
        float minX = float.PositiveInfinity;
        float minY = float.PositiveInfinity;
        float maxX = float.NegativeInfinity;
        float maxY = float.NegativeInfinity;
        for(int i = 0; i < colliderPath.Length; i++)
        {
            Vector2 currPoint = colliderPath[i];
            if (currPoint.x < minX)
            {
                minX = currPoint.x;
            }
            if (currPoint.y < minY)
            {
                minY = currPoint.y;
            }
            if (currPoint.x > maxX)
            {
                maxX = currPoint.x;
            }
            if (currPoint.y > maxY)
            {
                maxY = currPoint.y;
            }
        }
        float maxDist = Mathf.Max(maxX - minX, maxY - minY);
        Vector2[] uvPath = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvPath[i] = vertices[i];
            uvPath[i].x = (uvPath[i].x - minX) / maxDist;
            uvPath[i].y = (uvPath[i].y - minY) / maxDist;
        }

        mesh.uv = uvPath;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void createDefaultShape()
    {
        Vector3[] tempVertices = new Vector3[]
        {
            new Vector3 (1,0,0),
            new Vector3 (1,1,0),
            new Vector3 (0,1,0),
            new Vector3 (0,0,0)
        };
        SetVertices(tempVertices);
    }
}