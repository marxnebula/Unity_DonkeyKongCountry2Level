using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    // Main player
    public GameObject cart;

    /*
     * If cart hits an enemy then kill cart
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is cart
        if (other.tag == "Cart")
        {
            // If other is box collider
            if (other == other.GetComponent<BoxCollider2D>())
            {
                // Play the audio
                GetComponent<AudioSource>().Play();

                // Kill main character
                GameControl.control.SetGameOver(true);
            }

        }
    }
}
