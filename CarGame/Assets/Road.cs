using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : RoadSegment {

    //!!PROVISIONAL ONLY FOR VISUALIZATION!!
    private void Update()
    {
        path.GetMidwaysAndLength();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(path.startCorners[0].transform.position, path.endCorners[1].transform.position);
        Gizmos.DrawLine(path.startCorners[1].transform.position, path.endCorners[0].transform.position);
        Gizmos.DrawLine(path.midways[0], path.midways[1]);
    }
}
