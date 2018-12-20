using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : RoadSegment {

    //!!PROVISIONAL ONLY FOR VISUALIZATION!!
    private void Update()
    {
        path.UpdateKeypoints();
    }

    private void OnDrawGizmos()
    {
        path.DrawGizmo();
    }

    public override Connection GetConnection(Car car)
    {
        RoadSegment rs = car.previousRoadSegment;

        foreach (Connection connection in connectionList)
        {
            if (connection.entryBorder.rs == rs)
            {
                return connection;
            }
        }

        Debug.LogError("GetConnection cannot find Connection for Car object.");
        return null;
    }
}
