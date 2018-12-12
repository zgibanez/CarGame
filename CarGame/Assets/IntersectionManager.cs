using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour {

    public List<Intersection> intersectionList; //List of currently active intersections
    public List<Path> pathList;
    public GameObject roadObject;           //Road prefab for instantiating in GenerateRoads method

	void Start ()
    {
        GetIntersectionsOnGame();
        GenerateRoads();
	}

    /// <summary>
    /// Finds all intersections in the game and adds them to the list of the IntersectionManager. Note
    /// that all intersections must be children of the gameObject containing the intersection manager.
    /// </summary>
    private void GetIntersectionsOnGame()
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
    private void GenerateRoads()
    {
        pathList = new List<Path>();

        //For each intersection in the game
        foreach (Intersection startInter in intersectionList)
        {
            //We just need the 4 connected corner objects
            GameObject[] startCorners = new GameObject[2];
            GameObject[] endCorners = new GameObject[2];

            //For every exit of the intersection
            for (int i=0; i<startInter.exitArray.Length; i++)
            {
                //Find if exit is connected to an existing intersection
                Intersection endInter = intersectionList.Find(x => x.id == startInter.exitArray[i].nextIntersectionID);
                if (endInter == null)
                {
                    Debug.LogWarning("Intersection " + startInter.exitArray[i].nextIntersectionID + " seems to not exist.");
                    continue;
                }
                
                //Retrieve the corner positions of both exits
                startCorners = startInter.GetExitCorners((Intersection.Direction) i);
                endCorners = endInter.GetExitCorners( startInter.exitArray[i].direction);

                //Create a Road and add to it a path connecting corners and midways
                GameObject road = Instantiate(roadObject, transform);
                Path path = new Path();
                path.Create(startCorners, endCorners);
                road.GetComponent<Road>().path = path;
            }
        }
    }


}
