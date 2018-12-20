using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoadSegment : MonoBehaviour {
    public Path path;
    public RoadSegment nextRoadSegment;
    public List<Connection> connectionList = new List<Connection>();

    public Connection QueryConnectionFromRoadSegments(RoadSegment entry, RoadSegment exit)
    {
        foreach (Connection connection in connectionList)
        {
            if (connection.entryBorder.rs == entry && connection.exitBorder.rs == exit)
            {
                return connection;
            }
        }

        return null;
    }

    public virtual RoadSegment GetNextSegment(Car car)
    {
        nextRoadSegment.UpdatePath(car);  //Path is updated before assigning
        return nextRoadSegment;
    }
     /// <summary>
     /// Generic method for updating the Path of the RoadSegment according to the car state.
     /// </summary>
     /// <param name="Car">The car object that queries</param>
    public virtual void UpdatePath(Car car)
    {

    }

    public abstract Connection GetConnection(Car car);
}
