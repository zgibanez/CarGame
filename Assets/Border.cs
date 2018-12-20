using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border {

    public GameObject[] corners; //Corners of the borders
    public RoadSegment rs;       //RoadSegment it leads to

    public Border(GameObject[] corners, RoadSegment rs)
    {
        this.corners = corners;
        this.rs = rs;
    }

    public Border()
    {
    }

}
