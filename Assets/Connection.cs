using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection {

    public Border entryBorder, exitBorder;
    public Path path;

    public Connection(Border entryBorder, Border exitBorder)
    {
        this.entryBorder = entryBorder;
        this.exitBorder = exitBorder;
        BuildPath();
    }

    public void BuildPath()
    {
        path = new Path();
        path.Create(entryBorder.corners, exitBorder.corners);
    }

    public RoadSegment GetNextSegment()
    {
        return exitBorder.rs;
    }

}
