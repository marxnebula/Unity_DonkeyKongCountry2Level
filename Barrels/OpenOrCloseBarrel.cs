using UnityEngine;
using System.Collections;

public class OpenOrCloseBarrel : MonoBehaviour {

    // Boolean for if barrel opens or closes the gate
    public bool isOpen = true;


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

                // Destroy the collider
                Destroy(GetComponent<BoxCollider2D>());

                // Unrender the coin
                GetComponent<SpriteRenderer>().enabled = false;

                // Destroy the coin after the sound has played
                Destroy(gameObject, GetComponent<AudioSource>().clip.length);
            }
        }
    }
}
