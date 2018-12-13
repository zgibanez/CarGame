using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {

    public GameObject[] startCorners; //I changed these for game objects to track better dynamic changes in position
    public GameObject[] endCorners; 
    public Vector2[] midways;

    Vector3[] keypoints;

    public float length;

    /// <summary>
    /// Custom constructor (if Monobehavior is required, standard constructors are not allowed)
    /// </summary>
    /// <param name="startCorners"></param>
    /// <param name="endCorners"></param>
    public void Create(GameObject[] startCorners, GameObject[] endCorners)
    {
        this.startCorners = startCorners;
        this.endCorners = endCorners;

        keypoints = new Vector3[4]; // initialize once
        UpdateKeypoints();
    }

    /// <summary>
    /// function that can be used to recalculate the number and configurations of the keypoints of the path center
    /// </summary>
    public void UpdateKeypoints()
    {
        keypoints = new Vector3[4];

        // set center points as end points
        keypoints[0] = Vector3.Lerp(startCorners[0].transform.position, startCorners[1].transform.position, 0.5f);
        keypoints[3] = Vector3.Lerp(endCorners[0].transform.position, endCorners[1].transform.position, 0.5f);

        // determine 1/2 of length between end points (arbitrary factor. lower factor results in less sharp curve)
        float keypointDistance = Vector3.Distance(keypoints[0], keypoints[3]) / 2f;

        // add intermediate points normal to start and end points at distance calculated above
        Vector3 startForwards = (startCorners[1].transform.position - keypoints[0]);
        startForwards = Quaternion.AngleAxis(-90, Vector3.back) * startForwards;
        startForwards = keypoints[0] + startForwards.normalized * keypointDistance;
        keypoints[1] = startForwards;

        Vector3 endBackwards = (endCorners[0].transform.position - keypoints[3]);
        endBackwards = Quaternion.AngleAxis(90, Vector3.back) * endBackwards;
        endBackwards = keypoints[3] + endBackwards.normalized * keypointDistance;
        keypoints[2] = endBackwards;

        CalculateLength();
    }

    void CalculateLength()
    {
        length = 0;
        Vector3 pointA = keypoints[0];
        Vector3 pointB;
        int resolution = 10;
        for (float i = 1; i < resolution; i++)
        {
            pointB = BezierPoint(keypoints, i / resolution);
            length += Vector3.Distance(pointA, pointB);
            pointA = pointB;
        }
    }

    /// <summary>
    /// Generic function that returns the 2D position of a vehicle on the path.
    /// </summary>
    /// <param name="pathD"> Distance travelled along the road segment.</param>
    /// <param name="horiD"> Horizontal deviation from the midway path.</param>
    /// <returns></returns>
    public virtual Vector3 GetPosition(float pathD, float horiD)
    {
        return BezierPoint(keypoints, pathD / length);
    }

    Vector3 BezierPoint(Vector3[] points, float t)
    {
        if (points.Length == 2) return Vector3.Lerp(points[0], points[1], t);
        Vector3[] subPoints = new Vector3[points.Length - 1];
        for (int i = 0; i < subPoints.Length; i++)
        {
            subPoints[i] = BezierPoint(new Vector3[] { points[i], points[i + 1] }, t);
        }
        return BezierPoint(subPoints, t);
    }

    public void DrawGizmo()
    {
        Gizmos.color = Color.red;

        // draw keypoints
        for (int i = 0; i < keypoints.Length; i++)
            Gizmos.DrawSphere(keypoints[i], 0.05f);

        // draw curve
        Vector3 pointA = keypoints[0];
        Vector3 pointB;
        int resolution = 20;
        for (float i = 1; i <= resolution; i++)
        {
            pointB = BezierPoint(keypoints, i / resolution);
            Gizmos.DrawLine(pointA, pointB);
            pointA = pointB;
        }
    }
}
