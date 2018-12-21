using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Intersection : RoadSegment {

    public int id;
    public GameObject SE, SW, NW, NE;                 //Placeholders for corners
    public enum Direction { SOUTH, WEST, NORTH, EAST, NONE};

    [System.Serializable]
    public struct Exit
    {
        public int nextIntersectionID; // To which intersection does this exit lead
        public Direction direction;    // To which side of the intersection does the exit lead
        public Road road;              // The road object that makes this connection
    }
    public Exit[] exitArray;  //In the order 0 = South, 1 = West, 2 = North, 3 = East

    private int GetExitIndex(Exit exit)
    {
        for (int i = 0; i < exitArray.Length; i++)
        {
            if (exit.Equals(exitArray[i]))
                return i;
        }
        Debug.LogWarning("GetExitIndex could not find exit in exitArray");
        return -1;
    }

    /// <summary>
    /// Returns the corner objects of a direction in an intersecion.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public GameObject[] GetExitCorners(Direction dir)
    {
        return Direction2Corner(dir);
    }

    public GameObject[] GetExitCorners(Exit exit)
    {
        int idx = GetExitIndex(exit);
        return Direction2Corner((Direction)idx);
    }

    private GameObject[] Direction2Corner(Direction dir)
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


    public override Connection GetConnection(Car car)
    {
        Exit startExit = new Exit();
        Exit endExit;
        //Get exit where the car comes from
        for (int i = 0; i < exitArray.Length; i++)
        {
            if (car.previousRoadSegment == exitArray[i].road)
            {
                startExit = exitArray[i];
            }
        }
        //Get exit where is it going according to lights
        endExit = GetCarExit((Direction)GetExitIndex(startExit), car.direcLight);
        //Check if this connection exists
        Connection c = QueryConnectionFromRoadSegments(startExit.road, endExit.road);
        //If it does not exist create a new connection
        if (c == null)
        {
            //Get corners of both exits and construct borders
            Border startBorder = new Border(GetExitCorners(startExit), startExit.road);
            Border endBorder = new Border(GetExitCorners(endExit), endExit.road);
            //Construct a connection
            c = new Connection(startBorder, endBorder);
            connectionList.Add(c);
        }

        return c;
    }

    /// <summary>
    /// Returns the exit where the car is heading according to the car starting direction and directional lights.
    /// </summary>
    /// <param name="startDirection"></param>
    /// <param name="direcLight"></param>
    /// <returns></returns>
    private Exit GetCarExit(Direction startDirection, Car.DirecLight direcLight)
    {
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
        return exitArray[exitNumber];
    }

    private void OnDrawGizmos()
    {
        //Maybe pass a GuiStyle for better visualization?
        Handles.Label(transform.position + new Vector3(0,0.5f,0), id.ToString()); //Draw label
    }
}
