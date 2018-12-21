using UnityEngine;

public class Bezier {

    Vector3[] controlPoints;
    Vector3[] hodograph;        // Used for derivative https://pages.mtu.edu/~shene/COURSES/cs3621/NOTES/spline/Bezier/bezier-der.html
    public float length;

    public Bezier(Vector3[] points)
    {
        controlPoints = points;

        // create hodograph
        hodograph = new Vector3[points.Length - 1];
        for (int i = 0; i < hodograph.Length; i++)
            hodograph[i] = points[i + 1] - points[i];

        CalculateLength();
    }

    void CalculateLength()
    {
        length = 0;
        Vector3 pointA = controlPoints[0];
        Vector3 pointB;
        int resolution = 8;
        for (float i = 1; i < resolution; i++)
        {
            pointB = BezierPoint(controlPoints, i / resolution);
            length += Vector3.Distance(pointA, pointB);
            pointA = pointB;
        }
    }

    /// <summary>
    /// Returns the point at position t [0,1]
    /// </summary>
    /// <param name="t">Point on the curve [0,1]</param>
    /// <returns></returns>
    public Vector3 GetPoint(float t)
    {
        return BezierPoint(controlPoints, t);
    }

    public Vector3 GetNormal(float t)
    {
        Vector3 derivative = GetDerivative(t).normalized;
        Vector3 normal = new Vector3(derivative.y, -derivative.x, derivative.z);
        return normal;
    }

    public Vector3 GetDerivative(float t)
    {
        return BezierPoint(hodograph, t);
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
}
