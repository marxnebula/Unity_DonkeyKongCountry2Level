using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SkeletonPirate : MonoBehaviour {

    // The cart
    public GameObject cart;

    // The camera cart
    public GameObject cameraCart;

    // Boolean for if alive
    public bool isAlive = false;

    // User enters the added time
    public float time = 0;

    // Used to increment move towards path
    public float step = 0f;

    // Speed of the enemy
    public float speed = 1f;

    // Index for the list
    public int listIndex = 0;

    // Index for points
    public int pointIndex = 0;
    
    // Boolean for if following the cart
    public bool isFollowing = false;

    // Vector skeleton follows
    public Vector2 followVector;

    // Timer
    public float timer = 0f;



	// Use this for initialization
	void Start () {

        

        
	}
	
	// Update is called once per frame
	void Update () {

        // Get the speed
        speed = cart.GetComponent<Cart>().speed;

        // If cart is ahead of skeleton
        if (((cart.GetComponent<Transform>().position.x - 0.5f) >= GetComponent<Transform>().position.x) &&
            !isFollowing)
        {
            // Set booleans to true
            isFollowing= true;
            isAlive = true;

            // Add the time
            GameControl.control.SetTime(time);
        }


            
        // Kill cart
        if (isFollowing && isAlive && GameControl.control.GetTime() <= 0)
        {
            print("DEAD");

            // Set isAlive to false
            isAlive = false;
        //    isFollowing = false;

            // Add a baby jump
            GetComponent<Rigidbody2D>().AddForce(new Vector2(120f, 0));

            // Turn on gravity
            GetComponent<Rigidbody2D>().gravityScale = 1f;

            // Set game over
            GameControl.control.SetGameOver(true);
                
        }

        
        

	}


    void FixedUpdate()
    {
        
        // If following and alive
        if (isFollowing && isAlive)
        {
            // If cart gets way ahead of skeleton
            if(cart.GetComponent<Transform>().position.x - GetComponent<Transform>().position.x >= 1)
            {
                // Increase the step
                step = speed * 0.019f;
            }
            else
            {
                // The normal step
                step = speed * 0.015f;
            }

            // Set the vector to follow
            followVector = new Vector2(cameraCart.GetComponent<Transform>().position.x - 0.3f,
                cameraCart.GetComponent<Transform>().position.y + 0.4f);

            // Set the position
            GetComponent<Transform>().position = Vector2.MoveTowards(GetComponent<Transform>().position,
               followVector, step);

            // Set text to be true
            GameControl.control.SetIsText(true);
        }
    }



    public void SetIsAlive(bool b)
    {
        isAlive = b;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }
}
