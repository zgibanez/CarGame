using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Segment from which the car is spawned.
/// </summary>
public class StartSegment : Intersection {

    public int startingEntry = 0;   //Not sure why dont they show in inspector
    public int startingExit = 2; 

    /// <summary>
    /// Simplified, more hardcoded version of GetConnection in function of specified entry and exit point
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    public override Connection GetConnection(Car car)
    {
        Border entryBorder = new Border(GetExitCorners(exitArray[startingEntry].direction), this);
        Border exitBorder = new Border(GetExitCorners(exitArray[startingExit].direction), exitArray[startingExit].road);
        Connection c = new Connection(entryBorder,exitBorder);
        connectionList.Add(c);
        return c;
    }

    private Exit findNonEmptyExit()
    {
        foreach (Exit exit in exitArray)
        {
            if (exit.road != null)
            {
                return exit;
            }
        }

        throw new NullReferenceException("Start segment cannot find any non-empty exit");
    }

}
