using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public IntersectionManager intersectionManager;
    public RoadSegment initialRoadSegment;
    public GameObject carObject;

    void Start()
    {
       intersectionManager.GetIntersectionsOnGame();
       intersectionManager.GenerateRoads();
       GameObject car = Instantiate(carObject);
       car.GetComponent<Car>().currentRoadSegment = initialRoadSegment;
    }
}
