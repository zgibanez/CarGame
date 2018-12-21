using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoadSegment : MonoBehaviour {
    public Path path;
    public List<Connection> connectionList = new List<Connection>();

    //Connections are updated each frame (need to replace for passive wait)
    private void Update()
    {
        foreach (Connection c in connectionList)
        {
            c.path.Recalculate();
        }
    }

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

    public abstract Connection GetConnection(Car car);
}
