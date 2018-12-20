using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public IntersectionManager intersectionManager;
    public StartSegment startSegment;
    public GameObject carObject;

    void Start()
    {
       intersectionManager.GenerateRoads();
       GameObject car = Instantiate(carObject);
       car.GetComponent<Car>().currentRoadSegment = startSegment;
       car.GetComponent<Car>().currentConnection = startSegment.GetConnection(car.GetComponent<Car>());
    }
}
