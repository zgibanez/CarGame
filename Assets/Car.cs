using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    // Variables for calculating path position
    [Header("Path")]
    public float speed = 1f;
    public float pDist = 0f;   //Path distance traversed
    public RoadSegment currentRoadSegment;
    public RoadSegment previousRoadSegment;
    public Connection currentConnection;
    public Intersection.Direction nextIntersectionDirection;

    public enum DirecLight { NONE, LEFT, RIGHT };
    [Header("Directional Lights")]
    public DirecLight direcLight;

    //Key bindings for changing directional lights
    [Header("Keys")]
    public KeyCode leftLight;
    public KeyCode rightLight;
    public KeyCode cancelLight;

    SpriteRenderer spriteRenderer; 
    [Header("Sprites")]
    public Sprite[] carSprites; //Sprites for visualizing changes in direction

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentRoadSegment.GetConnection(this);
    }

    void Update ()
    {
        GetLightDirection(); //Get User Input
        UpdatePosition();
	}

    /// <summary>
    /// Updates the car position by asking the RoadSegment path.
    /// </summary>
    void UpdatePosition()
    {
        pDist += Time.deltaTime * speed; //Update the path distance traversed
        if (pDist < currentConnection.path.length)
        {
            transform.position = currentConnection.path.GetPosition(pDist, 0); //Ask the current road segment for position 
            return;
        }

        //If we have traversed more distance than the current path, it means we are in a new RoadSegment
        previousRoadSegment = currentRoadSegment;
        currentRoadSegment = currentConnection.exitBorder.rs;
        currentConnection = currentRoadSegment.GetConnection(this);
        pDist = 0; //reset path distance
    }

    /// <summary>
    /// Registers user input to change the DirecLight state.
    /// </summary>
    void GetLightDirection()
    {
        DirecLight oldDirecLight = direcLight;

        if (Input.GetKeyDown(leftLight))
        {
            if (direcLight == DirecLight.LEFT)
                direcLight = DirecLight.NONE;
            else
                direcLight = DirecLight.LEFT;
        }

        if (Input.GetKeyDown(rightLight))
        {
            if (direcLight == DirecLight.RIGHT)
                direcLight = DirecLight.NONE;
            else
                direcLight = DirecLight.RIGHT;
        }

        if (Input.GetKeyDown(cancelLight))
        {
            direcLight = DirecLight.NONE;
        }

        if (oldDirecLight != direcLight)
        {
            ChangeSprite();
        }
    }

    void ChangeSprite()
    {
        spriteRenderer.sprite = carSprites[(int)direcLight];
    }


}
