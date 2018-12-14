using System;
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
        public Road road;              // The road object that makes this connection
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

    public override void UpdatePath(Car car)
    {
        Direction nextDirection;
        GenerateLocalPath(car.nextIntersectionDirection, car.direcLight, out nextDirection);
        car.nextIntersectionDirection = nextDirection;
    }

    /// <summary>
    /// Generates a path inside the intersection that connects to sides accoding to the entry point and the Car blinking lights.
    /// </summary>
    /// <param name="startDirection"></param>
    /// <param name="direcLight">State of the directional light of the car</param>
    public void GenerateLocalPath(Direction startDirection, Car.DirecLight direcLight, out Direction nextDirection)
    {

        // Find the starting side
        //Direction startDirection = Array.Find(exitArray, e => e.road == road).direction;

        int exitNumber = (int)startDirection;
        //Exits are treated as a circular array. The blinking light state adds to the 
        //array index and the actual exit is obtained with the remainder.
        switch (direcLight)
        {
            case Car.DirecLight.NONE:
                exitNumber += 2;
                break;
            case Car.DirecLight.LEFT:
                exitNumber += 1;
                break;
            case Car.DirecLight.RIGHT:
                exitNumber += 3;
                break;
        }
        exitNumber %= exitArray.Length;
        Direction endDirection = (Direction) exitNumber;
        nextRoadSegment = exitArray[(int)endDirection].road;
        nextDirection = exitArray[(int)endDirection].direction;

        path = new Path();
        path.Create(GetExitCorners(startDirection), GetExitCorners(endDirection));
    }

    private void OnDrawGizmos()
    {
        //Maybe pass a GuiStyle for better visualization?
        Handles.Label(transform.position + new Vector3(0,0.5f,0), id.ToString()); //Draw label
    }
}
