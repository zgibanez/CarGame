using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour {
    public Path path;
    public RoadSegment nextRoadSegment;

    Path[,] paths;  //Array of possible paths

    public virtual RoadSegment GetNextSegment(Car car)
    {
        nextRoadSegment.UpdatePath(car);  //Path is updated before assigning
        return nextRoadSegment;
    }
     /// <summary>
     /// Generic method for updating the Path of the RoadSegment according to the car state.
     /// </summary>
     /// <param name="car"></param>
    public virtual void UpdatePath(Car car)
    {

    }
}
