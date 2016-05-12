using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Class for getting all the edge colliders and storing each vector point.
 *  - Each point is used for the main characters movement by lerping to each point.
 *  - This script must be attached to the level containing the edge colliders. 
 */

public class Track : MonoBehaviour {

    // Gameobject of the cart
    public GameObject cart;

    // Stores all the edge colliders
    public EdgeCollider2D[] storedEdgeColliders;

    // Stores number of edge colliders
    public int numberOfEdgeColliders = 0;

    // List for storing the vectors of the edge collider points
    public List<Vector2[]> vectorList;



    void Start()
    {

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


        // Store vector list in game control
        GameControl.control.SetVectorList(vectorList);
        
    }



    public List<Vector2[]> GetVectorList()
    {
        return vectorList;
    }

    public EdgeCollider2D[] GetEdgeColliders()
    {
        return storedEdgeColliders;
    }




    /*
     * Trigger collision for cart.  Collision only occurs for the circle collider 2d
     */
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
                    cart.GetComponent<Cart>().doOnce = true;
                }
                
                // Set gravity to false
                cart.GetComponent<Cart>().SetGravity(false);

                // Set boolean to true
                cart.GetComponent<Cart>().isRotation = true;

                // Set boolean to false
                cart.GetComponent<Cart>().isTransition = false;
            }
        }
         
    }

}
