using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Intersection : RoadSegment {

    public int id;
    public GameObject SE, SW, NW, NE;                 //Placeholders for corners
    public enum Direction { SOUTH, WEST, NORTH, EAST};

    [System.Serializable]
    public struct Exit
    {
        public int nextIntersectionID; // To which intersection does this exit lead
        public Direction direction;    // To which side of the intersection does the exit lead
    }
    public Exit[] exitArray;  //In the order 0 = South, 1 = West, 2 = North, 3 = East


    /// <summary>
    /// Returns the corner objects of a direction in an intersecion.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public GameObject[] GetExitCorners(Direction dir)
    {
        GameObject[] corners = new GameObject[2];
        switch (dir)
        {
            case Direction.SOUTH:
                corners[0] = SE;
                corners[1] = SW;
                break;
            case Direction.WEST:
                corners[0] = SW;
                corners[1] = NW;
                break;
            case Direction.NORTH:
                corners[0] = NW;
                corners[1] = NE;
                break;
            case Direction.EAST:
                corners[0] = NE;
                corners[1] = SE;
                break;
        }
        return corners;
    }

    private void OnDrawGizmos()
    {
        //Maybe pass a GuiStyle for better visualization?
        Handles.Label(transform.position + new Vector3(0,0.5f,0), id.ToString()); //Draw label
    }
}
