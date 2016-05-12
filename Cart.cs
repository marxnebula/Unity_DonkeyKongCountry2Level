using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Class for the main character which is a cart.
 *  - The cart gets all the vector points of the edge collider and the cart lerps
      to each point.  
 *  - If the cart jumps or falls off an edge then you only lerp to
      the x position of the vector point.  
 *  - The rotation is determined by taking two
      vector points and calculating the angle between them.  
 *  - The cart slows down and speeds up according to certain angles of the track.  
 *  - All cart sounds are in this script.  
 *  - Gravity is on only when jumping or falling off the edge of a track. 
 */

public class Cart : MonoBehaviour
{

    // Speed of the cart
    public float speed = 1f;

    // Store speed of cart
    private float storedSpeed = 0f;

    // Stored speed at start of jump
    private float storedSpeedAtJump = 0f;

    // Boolean for if jumping
    public bool isJumping = false;

    // Boolean for if gravity is on
    public bool isGravity = false;

    // Zero vector3
    private Vector3 velocity = Vector3.zero;

    // Carts z Rotation
    public float zRotation = 0f;

    // Boolean for music
    private bool isMusic = false;

    // Stores the audio sources
    private AudioSource[] audioSources;

    // Cart clips
    public AudioClip cartRiding;
    public AudioClip cartJump;
    public AudioClip cartLand;

    // The Track
    public Track track;

    // Store the vectors of the current track
    public List<Vector2[]> vectorList;

    // Stores all the edge colliders
    public EdgeCollider2D[] storedEdgeColliders;

    // Index for the list
    public int listIndex = 0;

    // Index for points
    public int pointIndex = 0;

    // Goes from 0 to 1 for the lerping process
    public float lerpNumber = 0f;

    // If transitioning to other track
    public bool isTransition = false;

    // Boolean for if rotation should take place
    public bool isRotation = true;

    // Stores the difference between 2 points
    public Vector2 vectorDifference;

    // Boolean for doing once
    public bool doOnce = true;

    // Store zRotation
    public float storeZRotation = 0f;

    // Used to store the new rotation (This needs to be set to a hidden object on the scene to work)
    public Transform newRotation;

    // Increment for changing the z rotation
    public float increment = 0.01f;

     // Used to increment move towards path
    public float step = 0f;

    // Adjustment for the cart size
    public float sizeAdjustForCart = 0.3f;




    // Use this for initialization
    void Start()
    {
        // Store the speed
        storedSpeed = speed;

        // Get the audio sources
        audioSources = GetComponents<AudioSource>();

        // Set the vectors of the track
        vectorList = track.GetVectorList();

        // Get the edge colliders
        storedEdgeColliders = track.GetEdgeColliders();
    }

    // Update
    void Update()
    {

        FollowTrack();

        DetermineRotation();

        DetermineIfFallen();

        DetermineEndOfLevel();
    }


    // Use fixed update if using rigidbody
    void FixedUpdate()
    {
        Speed();

        Gravity();

        Jump();

        PlaySound();




        // Set the step
        step = speed * 0.015f;
        //  step = speed * Time.deltaTime;



        // If cart has not jumped and is not in transition
        if (!GetComponent<Cart>().GetJump() && !isTransition)
        {
            // Cart follows the track
            GetComponent<Transform>().position = Vector2.MoveTowards(GetComponent<Transform>().position,
        new Vector2(vectorList[listIndex][pointIndex].x, vectorList[listIndex][pointIndex].y + sizeAdjustForCart), step);

        }
        // Else if in transition
        else if (isTransition)
        {
            // Cart follows track x position only
            GetComponent<Transform>().position = Vector2.MoveTowards(GetComponent<Transform>().position,
        new Vector2(vectorList[listIndex][pointIndex].x, GetComponent<Transform>().position.y), step);
        }
        // This else is here because I might change it in the future
        else
        {
            // Cart follows x position only
            GetComponent<Transform>().position = Vector2.MoveTowards(GetComponent<Transform>().position,
    new Vector2(vectorList[listIndex][pointIndex].x, GetComponent<Transform>().position.y), step);


        }
    }


    /*
     * Determines speed of the cart which is based off of its zRotation.  Going downhill
     * will accelerate and uphill the opposite.  Limits the lowest and highest speeds.
     */
    void Speed()
    {
        // If not jumping
        if(!isJumping)
        {
            // If zRotation is 31-74
            if ((zRotation > 30) && (zRotation < 75))
            {
                // Decrease speed
                speed = speed - 0.02f;

                // Don't let speed go below 2.4
                if (speed < 2.4f)
                {
                    speed = 2.4f;
                }
            }
            // Else if zRotation is from -34 to -11
            else if ((zRotation > -35) && (zRotation < -10))
            {
                // Increase speed
                speed = speed + 0.009f;
            }
            // Else if zRotation is from -99 to -36
            else if ((zRotation > -100) && (zRotation < -35))
            {
                // Increase speed
                speed = speed + 0.02f;
            }
            else
            {
                // Increase speed
                if(speed < storedSpeed)
                {
                    speed = speed + 0.02f;
                }

            }
            

        }

        

        // Don't allow the speed to be greater than 3.8
        if(speed >= 3.8f)
        {
            speed = 3.8f;
        }

       


    }


    /*
     * Determines which sound is playing.  All sounds associated with the cart.
     */
    void PlaySound()
    {

        // If jumping
        if (isJumping)
        {
            audioSources[0].volume = 0f;
        }
        else
        {
            audioSources[0].volume = 1.0f;
        }

        // If the audio source clip is cartJump
        if (audioSources[1].clip == cartJump)
        {
            // If music
            if (isMusic)
            {
                // Play audio and set boolean
                audioSources[1].Play();
                isMusic = false;
            }

        }
        // Else if the audio source clip is cartLand
        else if (audioSources[1].clip == cartLand)
        {
            // If music
            if (isMusic)
            {
                // Play audio and set boolean
                audioSources[1].Play();
                isMusic = false;
            }
        }

        // If game over
        if (GameControl.control.GetGameOver())
        {
            // Set volumes to 0
            audioSources[0].volume = 0f;
            audioSources[0].volume = 0f;

            // Disable audio sources
            audioSources[0].enabled = false;
            audioSources[1].enabled = false;

            print("GameOver");
        }
        else
        {
            // Set volume to 1
            audioSources[0].volume = 1f;
            audioSources[0].volume = 1f;

            // Enable audio sources
            audioSources[0].enabled = true;
            audioSources[1].enabled = true;
        }
        
    }


    /*
     * Determines the jump height and speed of cart based on the zRotation.
     */
    void Jump()
    {
        // If pressed space bar and no gravity and isn't currently jumping
        if (Input.GetKey(KeyCode.Space) && !isJumping && !isGravity)
        {

            // Set booleans
            isJumping = true;
            isGravity = true;

            
            /* Based on zRotation it sets speed and jump force */
            if(zRotation > 56)
            {
                speed = 0.7f;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 255f));
            }
            else if(zRotation > 40)
            {
                speed = 2f;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 230f));
            }
            else if (zRotation > 30)
            {
                speed = 2.3f;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 225f));
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 220f));
            }

            // Store speed at jump
            storedSpeedAtJump = speed;

            // Set the clip
            audioSources[1].clip = cartJump;

            // Set boolean to true
            isMusic = true;

            
        }
    }



    /*
     * Function called when cart collides with track after jumping.
     */
    public void EndJump()
    {
        // Set booleans to false
        isJumping = false;
        isGravity = false;

        // Set audio clip
        audioSources[1].clip = cartLand;
        isMusic = true;

        // Set velocity to 0 because gravity was just on
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

        // Set speed to go back to what it was
        speed = storedSpeedAtJump;
    }


    /*
     * Turn gravity on when you jump or fall off track.
     */
    void Gravity()
    {
        if(isGravity)
        {
            // Set the gravity scale
            GetComponent<Rigidbody2D>().gravityScale = 1.6f;
        }
        else
        {
            // Turn off gravity
            GetComponent<Rigidbody2D>().gravityScale = 0f;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }

    }

    /*
    * Cart follows each point in the edge colliders.  It switches to the other edge collider once
    * it has gotten close to the last point of the previous edge collider.
    */
    void FollowTrack()
    {
        // If the carts position is close to the edge collider point
        if (GetComponent<Transform>().position.x >= (vectorList[listIndex][pointIndex].x - 0.1f))
        {
            // If it is not the last point
            if (pointIndex != (vectorList[listIndex].Length - 1))
            {
                // Increment point
                pointIndex++;

                // If has not jumped
                if (!GetComponent<Cart>().GetJump())
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
                    GetComponent<Cart>().SetGravity(true);
                }


            }

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
                    GetComponent<Cart>().SetZRotation(zRotation);

                    // If cart jumped and doOnce is true
                    if (GetComponent<Cart>().GetJump() && doOnce)
                    {
                        // Store zRotation
                        storeZRotation = zRotation;

                        // Set boolean to false
                        doOnce = false;
                    }

                    // If cart has not jumped
                    if (!GetComponent<Cart>().GetJump())
                    {
                        // Set rotation to the zRotation
                        newRotation.eulerAngles = new Vector3(GetComponent<Transform>().rotation.x,
                            GetComponent<Transform>().rotation.y, zRotation);
                    }
                    else
                    {
                        // If zRotation greater than 25
                        if (storeZRotation > 25)
                        {
                            // Set to the stored rotation
                            newRotation.eulerAngles = new Vector3(GetComponent<Transform>().rotation.x,
                            GetComponent<Transform>().rotation.y, storeZRotation);
                        }
                        else
                        {
                            // Set rotation to 25
                            newRotation.eulerAngles = new Vector3(GetComponent<Transform>().rotation.x,
                            GetComponent<Transform>().rotation.y, 25);
                        }



                    }
                }
            }

            // Lerp number for rotation speed
            lerpNumber = lerpNumber + increment;

            // Lerp the rotation
            GetComponent<Transform>().rotation = Quaternion.Lerp(GetComponent<Transform>().rotation,
                newRotation.rotation, lerpNumber);
        }
    }

    /*
    * Determines if cart has fallen off the track.
    */
    void DetermineIfFallen()
    {

        // If cart is 4 unitys below the current vector point y position
        if (GetComponent<Transform>().position.y - vectorList[listIndex][pointIndex].y <= -4)
        {
            // Set game over to true
            GameControl.control.SetGameOver(true);
        }

    }

    /*
    * Determines if main character beat the level.
    */
    void DetermineEndOfLevel()
    {

        // If cart has touched the last vector point in the last edge collider
        if (GetComponent<Transform>().position.x >=
            (vectorList[storedEdgeColliders.Length - 1][vectorList[storedEdgeColliders.Length - 1].Length - 1].x))
        {
            print("You win");

            // Set the end
            GameControl.control.SetIsEnd(true);
        }
    }

    public bool GetJump()
    {
        return isJumping;
    }

    public void SetJump(bool b)
    {
        isJumping = b;
    }


    public float GetSpeed()
    {
        return speed;
    }

    public void SetGravity(bool b)
    {
        isGravity = b;
    }

    public void SetZRotation(float z)
    {
        zRotation = z;
    }

    public int GetPointIndex()
    {
        return pointIndex;
    }


    public int GetListIndex()
    {
        return listIndex;
    }

}