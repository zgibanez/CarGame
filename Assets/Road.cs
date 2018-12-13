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
}
