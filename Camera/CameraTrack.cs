using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTrack : MonoBehaviour {

    // Gameobject of the cart
    public GameObject cartCamera;

    public GameObject cart;

    public float sizeAdjustForCart = 0.3f;

    // Stores all the edge colliders
    public EdgeCollider2D[] storedEdgeColliders;

    // Stores number of edge colliders
    public int numberOfEdgeColliders = 0;

    // List for storing the vectors of the edge collider points
    public List<Vector2[]> vectorList;

    // Stores the difference between 2 points
    public Vector2 vectorDifference;


    // Index for the list
    public int listIndex = 0;

    // Index for points
    public int pointIndex = 0;

    // Used to increment move towards path
    public float step = 0f;

    // Speed of the cart
    public float speed = 1f;

    // Stores the calcualted z rotation
    public float zRotation = 0f;

    // Increment for changing the z rotation
    public float increment = 0.01f;

    // Goes from 0 to 1 for the lerping process
    public float lerpNumber = 0f;

    // Boolean for if rotation should take place
    public bool isRotation = true;

    // Boolean for if following track
    public bool isFollowingTrack = true;



    private Vector2 velocity = Vector2.zero;

    // DEBUG
    public Vector2[] forTesting;


    void Start()
    {
        // Store the speed
        speed = cart.GetComponent<Cart>().GetSpeed();

    }



    void FixedUpdate()
    {
       // step = speed * 0.015f;

        vectorList = GetComponent<Track>().GetVectorList();
        listIndex = GetComponent<Track>().GetListIndex();
        pointIndex = GetComponent<Track>().GetPointIndex();

        if(cart.GetComponent<Transform>().position.x - cartCamera.GetComponent<Transform>().position.x >= 0.3f)
        {
            step = speed * 0.025f;
        }
        else
        {
            step = speed * 0.015f;
        }
            cartCamera.GetComponent<Transform>().position = Vector2.MoveTowards(cartCamera.GetComponent<Transform>().position,
        new Vector2(vectorList[listIndex][pointIndex].x, vectorList[listIndex][pointIndex].y + sizeAdjustForCart), step);


    }


    /*
     * 
     */
    public int GetPointIndex()
    {
        return pointIndex;
    }

    /*
     * 
     */
    public int GetListIndex()
    {
        return listIndex;
    }



}
