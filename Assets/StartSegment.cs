using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Segment from which the car is spawned.
/// </summary>
public class StartSegment : Intersection {

	// Use this for initialization
	void Start ()
    {
	}

    public override Connection GetConnection(Car car)
    {
        return connectionList[0];
    }
}
