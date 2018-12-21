using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour {

    public List<Intersection> intersectionList; //List of currently active intersections
    public GameObject roadObject;               //Road prefab for instantiating in GenerateRoads method

    /// <summary>
    /// Finds all intersections in the game and adds them to the list of the IntersectionManager. Note
    /// that all intersections must be children of the gameObject containing the intersection manager.
    /// </summary>
    public void GetIntersectionsOnGame()
    {
        Intersection[] intArray = GetComponentsInChildren<Intersection>();
        foreach (Intersection inter in intArray)
        {
            intersectionList.Add(inter);
        }
    }
	
    /// <summary>
    /// Generates a Road with a Path for each declared connection between intersections.
    /// </summary>
    public void GenerateRoads()
    {
        GetIntersectionsOnGame();
        foreach (Intersection inter in intersectionList)
        {
            BuildIntersectionRoads(inter);
        }
    }

    void BuildIntersectionRoads(Intersection startInter)
    {
        //We need the 4 connected corner objects
        GameObject[] startCorners = new GameObject[2];
        GameObject[] endCorners = new GameObject[2];

        //For every exit of the intersection
        for (int i = 0; i < startInter.exitArray.Length; i++)
        {
            //Find if exit is connected to an existing intersection
            Intersection endInter = intersectionList.Find(x => x.id == startInter.exitArray[i].nextIntersectionID);
            if (endInter == null)
            {
                Debug.LogWarning("Intersection " + startInter.exitArray[i].nextIntersectionID + " seems to not exist.");
                continue;
            }

            //Retrieve the corner objects of both exits
            startCorners = startInter.GetExitCorners((Intersection.Direction)i);
            endCorners = endInter.GetExitCorners(startInter.exitArray[i].direction);

            //Create borders
            Border borderA = new Border(startCorners,startInter);
            Border borderB = new Border(endCorners, endInter);

            //Create a Road and build its two connections 
            GameObject road = Instantiate(roadObject, transform);
            road.GetComponent<RoadSegment>().connectionList = new List<Connection>();
            road.GetComponent<RoadSegment>().connectionList.Add(new Connection(borderA, borderB));
            road.GetComponent<RoadSegment>().connectionList.Add(new Connection(borderB, borderA));

            //Add the road to the exit list of the other intersection
            Intersection.Exit endExit;
            endExit.road = road.GetComponent<Road>();
            endExit.nextIntersectionID = startInter.id;
            endExit.direction = (Intersection.Direction) i;
            endInter.exitArray[(int)startInter.exitArray[i].direction] = endExit;

            //Add the road object to the exit and the next segment to the road
            startInter.exitArray[i].road = road.GetComponent<Road>();
        }
    }
}


