using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {

    public GameObject[] startCorners; //I changed these for game objects to track better dynamic changes in position
    public GameObject[] endCorners; 
    public Vector2[] midways;

    float startWidth, endWidth;

    Bezier curve;

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

        Recalculate();
    }

    /// <summary>
    /// function that can be used to recalculate the number and configurations of the keypoints of the path center
    /// </summary>
    public void Recalculate()
    {
        Vector3[] controlPoints = new Vector3[4];

        // set center points as end points
        controlPoints[0] = Vector3.Lerp(startCorners[0].transform.position, startCorners[1].transform.position, 0.5f);
        controlPoints[3] = Vector3.Lerp(endCorners[0].transform.position, endCorners[1].transform.position, 0.5f);

        // determine 1/2 of length between end points (arbitrary factor. lower factor results in less sharp curve)
        float keypointDistance = Vector3.Distance(controlPoints[0], controlPoints[3]) / 2f;

        // add intermediate points normal to start and end points at distance calculated above
        Vector3 startForwards = (startCorners[1].transform.position - controlPoints[0]);
        startForwards = Quaternion.AngleAxis(-90, Vector3.back) * startForwards;
        startForwards = controlPoints[0] + startForwards.normalized * keypointDistance;
        controlPoints[1] = startForwards;

        Vector3 endBackwards = (endCorners[0].transform.position - controlPoints[3]);
        endBackwards = Quaternion.AngleAxis(90, Vector3.back) * endBackwards;
        endBackwards = controlPoints[3] + endBackwards.normalized * keypointDistance;
        controlPoints[2] = endBackwards;

        curve = new Bezier(controlPoints);

        startWidth = Vector3.Distance(startCorners[0].transform.position, startCorners[1].transform.position);
        endWidth = Vector3.Distance(endCorners[0].transform.position, endCorners[1].transform.position);
        length = curve.length;
        }

    /// <summary>
    /// Generic function that returns the 2D position of a vehicle on the path.
    /// </summary>
    /// <param name="pathD"> Distance travelled along the road segment.</param>
    /// <param name="horiD"> Horizontal deviation from the midway path.</param>
    /// <returns></returns>
    public virtual Vector3 GetPosition(float pathD, float horiD)
    {
        return curve.GetPoint(pathD);
    }

    public void DrawGizmo()
    {
        // draw curve
        int resolution = 20;
        for (float i = 0; i < resolution; i++)
        {
            float t1 = i / resolution;
            float t2 = (i + 1) / resolution;

            Gizmos.color = Color.red;
            Vector3 point1 = curve.GetPoint(t1);
            Vector3 point2 = curve.GetPoint(t2);
            Gizmos.DrawLine(point1, point2);

            // draw edges
            Gizmos.color = Color.white;
            float length1 = Mathf.Lerp(startWidth, endWidth, t1)/2;
            float length2 = Mathf.Lerp(startWidth, endWidth, t2) / 2;
            Gizmos.DrawLine(point1 + curve.GetNormal(t1) * length1, point2 + curve.GetNormal(t2) * length2);
            Gizmos.DrawLine(point1 + curve.GetNormal(t1) * -length1, point2 + curve.GetNormal(t2) * -length2);
        }
    }
}
