using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

    // User sets time to be added
    public float time;

    // Gets skeleton pirate
    public GameObject skeletonPirate;



    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is cart
        if (other.tag == "Cart")
        {
            // If skeletonPirate is not alive
            if(!skeletonPirate.GetComponent<SkeletonPirate>().GetIsAlive())
            {
                // Set skeletonPirate to alive
                skeletonPirate.GetComponent<SkeletonPirate>().SetIsAlive(true);

                // Add the time
              //  GameControl.control.SetTime(time);

                // Turn off collider
                GetComponent<Collider2D>().enabled = false;
            }
        }
        // If other is skeletonPirate
        if(other.tag == "SkeletonPirate")
        {
            // Set skeletonPirate to not alive
            skeletonPirate.GetComponent<SkeletonPirate>().SetIsAlive(false);

            // Set time to 0
            GameControl.control.SetTime(0);

            // Turn off collider
            GetComponent<Collider2D>().enabled = false;

            // Turn off text
            GameControl.control.SetIsText(false);
        }
    }
}
