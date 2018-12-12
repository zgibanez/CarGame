using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {

    public GameObject[] startCorners; //I changed these for game objects to track better dynamic changes in position
    public GameObject[] endCorners; 
    public Vector2[] midways;

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
        GetMidwaysAndLength();
    }

    public void GetMidwaysAndLength()
    {
        midways = new Vector2[2];
        midways[0] = (startCorners[0].transform.position + startCorners[1].transform.position) / 2;
        midways[1] = (endCorners[0].transform.position + endCorners[1].transform.position) / 2;
        length = Vector2.Distance(midways[0], midways[1]);
    }

    /// <summary>
    /// Generic function that returns the 2D position of a vehicle on the path.
    /// </summary>
    /// <param name="pathD"> Distance travelled along the road segment.</param>
    /// <param name="horiD"> Horizontal deviation from the midway path.</param>
    /// <returns></returns>
    public virtual Vector2 GetPosition(float pathD, float horiD)
    {
        float lambda = length / pathD; //Percentage of path traveled
        return Vector2.Lerp(midways[0], midways[1], lambda);
    }

}
