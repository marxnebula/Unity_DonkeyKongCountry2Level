using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Track : MonoBehaviour {

    // Gameobject of the cart
    public GameObject cart;

    // Adjustment for the cart size
    public float sizeAdjustForCart = 0.3f;

    // Stores all the edge colliders
    public EdgeCollider2D[] storedEdgeColliders;

    // Stores number of edge colliders
    public int numberOfEdgeColliders = 0;

    // List for storing the vectors of the edge collider points
    public List<Vector2[]> vectorList;

    // Stores the difference between 2 points
    public Vector2 vectorDifference;

    // Used to store the new rotation (This needs to be set to a hidden object on the scene to work)
    public Transform newRotation;

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

    // If transitioning to other track
    public bool isTransition = false;

    // Boolean for doing once
    public bool doOnce = true;

    // Store zRotation
    public float storeZRotation = 0f;

    // Zero Vector2
    private Vector2 velocity = Vector2.zero;

    // DEBUG
    public Vector2[] forTesting;
    public Vector2 tt;



    void Start()
    {
        // Store the speed
        speed = cart.GetComponent<Cart>().GetSpeed();

        // Gets all the edge colliders
        storedEdgeColliders = GetComponents<EdgeCollider2D>();

        // Gets the number of edge colliders
        numberOfEdgeColliders = storedEdgeColliders.Length;

        // Create the list which will store the vector of points for each edge collider
        vectorList = new List<Vector2[]>();
        
        // Loop for storing the points into the list
        for(int i = 0; i < numberOfEdgeColliders; i++)
        {
            // Add the vector2[] to the list
            vectorList.Add(storedEdgeColliders[i].points);
        }

        // DEBUG
        forTesting = vectorList[1];

        // Store vector list in game control
        GameControl.control.SetVectorList(vectorList);
        
    }


    void Update()
    {
        
        FollowTrack();

        DetermineRotation();

        DetermineIfFallen();

        DetermineEndOfLevel();
    }



    void FixedUpdate()
    {
      

        // Get the carts speed
        speed = cart.GetComponent<Cart>().GetSpeed();

        // Set the step
        step = speed * 0.015f;
        //  step = speed * Time.deltaTime;



        // If cart has not jumped and is not in transition
        if (!cart.GetComponent<Cart>().GetJump() && !isTransition)
        {
            // Cart follows the track
            cart.GetComponent<Transform>().position = Vector2.MoveTowards(cart.GetComponent<Transform>().position,
        new Vector2(vectorList[listIndex][pointIndex].x, vectorList[listIndex][pointIndex].y + sizeAdjustForCart), step);

        }
        // Else if in transition
        else if(isTransition)
        {
            // Cart follows track x position only
            cart.GetComponent<Transform>().position = Vector2.MoveTowards(cart.GetComponent<Transform>().position,
        new Vector2(vectorList[listIndex][pointIndex].x, cart.GetComponent<Transform>().position.y), step);
        }
        else
        {

    //         if ((storeZRotaion > 45))
    //        {
                    
    //              cart.GetComponent<Transform>().position = Vector2.MoveTowards(cart.GetComponent<Transform>().position,
    //   new Vector2(cart.GetComponent<Transform>().position.x, cart.GetComponent<Transform>().position.y), step);
    //           }
    //           else
    //           {
            // Cart follows x position only
                cart.GetComponent<Transform>().position = Vector2.MoveTowards(cart.GetComponent<Transform>().position,
        new Vector2(vectorList[listIndex][pointIndex].x, cart.GetComponent<Transform>().position.y), step);
    //         }

                
        }


        
    }


    /*
     * Cart follows each point in the edge colliders.  It switches to the other edge collider once
     * it has gotten close to the last point of the previous edge collider.
     */
    void FollowTrack()
    {
        // If the carts position is close to the edge collider point
        if (cart.GetComponent<Transform>().position.x >= (vectorList[listIndex][pointIndex].x - 0.1f))
        {
            // If it is not the last point
            if (pointIndex != (vectorList[listIndex].Length - 1))
            {
                // Increment point
                pointIndex++;

                // If has not jumped
                if (!cart.GetComponent<Cart>().GetJump())
                {
                    // Set lerp to 0
                    lerpNumber = 0f;
                }

            }
            // If it is last point
            else
            {
                // Turn off edge collider
                storedEdgeColliders[listIndex].enabled = false;

                // Set boolean to true
                isTransition = true;

                // If not last edge collider
                if (listIndex != (storedEdgeColliders.Length - 1))
                {
                    // Increment list
                    listIndex++;

                    // Set point to 0
                    pointIndex = 0;

                    // Set gravity to true
                    cart.GetComponent<Cart>().SetGravity(true);
                }

                     
             }     
            
        }

      
    }


    /*
     * Determines if main character beat the level.
     */
    void DetermineEndOfLevel()
    {
        
        // If cart has touched the last vector point in the last edge collider
        if (cart.GetComponent<Transform>().position.x >=
            (vectorList[storedEdgeColliders.Length - 1][vectorList[storedEdgeColliders.Length - 1].Length - 1].x))
        {
            print("You win");

            // Set the end
            GameControl.control.SetIsEnd(true);
        }
    }


    /*
     * Determines the rotation of the cart based on two points.  Subtract the two vectors and find the angle
     * between them.
     */
    void DetermineRotation()
    {
        // If is rotation
        if (isRotation)
        {
            // If point index is greater than 0
            if (pointIndex > 0)
            {
                
                if (pointIndex + 1 < storedEdgeColliders[listIndex].points.Length)
                {
                    //  vectorDifference = vectorList[listIndex][pointIndex] - vectorList[listIndex][pointIndex + 1];

                    // Get the vector difference
                    vectorDifference = vectorList[listIndex][pointIndex - 1] - vectorList[listIndex][pointIndex];
                
                    // Calculate zRotation
                    zRotation = Mathf.Atan((vectorDifference.y) / (vectorDifference.x));
                    zRotation = zRotation * (180 / Mathf.PI);

                    // Set the zRotation for cart
                    cart.GetComponent<Cart>().SetZRotation(zRotation);

                    // If cart jumped and doOnce is true
                    if(cart.GetComponent<Cart>().GetJump() && doOnce)
                    {
                        // Store zRotation
                        storeZRotation = zRotation;

                        // Set boolean to false
                        doOnce = false;
                    }

                    // If cart has not jumped
                    if (!cart.GetComponent<Cart>().GetJump())
                    {
                        // Set rotation to the zRotation
                        newRotation.eulerAngles = new Vector3(cart.GetComponent<Transform>().rotation.x,
                            cart.GetComponent<Transform>().rotation.y, zRotation);
                    }
                    else
                    {
                        // If zRotation greater than 25
                        if(storeZRotation > 25)
                        {
                            // Set to the stored rotation
                            newRotation.eulerAngles = new Vector3(cart.GetComponent<Transform>().rotation.x,
                            cart.GetComponent<Transform>().rotation.y, storeZRotation);
                        }
                        else
                        {
                            // Set rotation to 25
                            newRotation.eulerAngles = new Vector3(cart.GetComponent<Transform>().rotation.x,
                            cart.GetComponent<Transform>().rotation.y, 25);
                        }


                    
                    }
            }
            }

            // Lerp number for rotation speed
            lerpNumber = lerpNumber + increment;

            // Lerp the rotation
            cart.GetComponent<Transform>().rotation = Quaternion.Lerp(cart.GetComponent<Transform>().rotation,
                newRotation.rotation, lerpNumber); 
        }
    }


    /*
     * Determines if cart has fallen off the track.
     */
    void DetermineIfFallen()
    {

        // If cart is 4 unitys below the current vector point y position
        if(cart.GetComponent<Transform>().position.y - vectorList[listIndex][pointIndex].y <= -4)
        {
            // Set game over to true
            GameControl.control.SetGameOver(true);
        }

    }


    public void SetIsRotation(bool b)
    {
        isRotation = b;
    }


    public void SetIsFollowingTrack(bool b)
    {
        isFollowingTrack = b;
    }



    public List<Vector2[]> GetVectorList()
    {
        return vectorList;
    }



    public int GetPointIndex()
    {
        return pointIndex;
    }


    public int GetListIndex()
    {
        return listIndex;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is cart
        if(other.tag == "Cart")
        {
            // If other is circle collider
            if (other == other.GetComponent<CircleCollider2D>())
            {
                // If cart has jumped
                if(cart.GetComponent<Cart>().GetJump())
                {
                    // Call end jump function
                    cart.GetComponent<Cart>().EndJump();

                    // Set doOnce to true
                    doOnce = true;
                }
                
                // Set gravity to false
                cart.GetComponent<Cart>().SetGravity(false);

                // Set boolean to true
                isRotation = true;

                // Set boolean to false
                isTransition = false;
            }
        }
         
    }

}
