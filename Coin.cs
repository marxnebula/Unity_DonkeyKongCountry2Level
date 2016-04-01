using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is the cart
        if (other.tag == "Cart")
        {
            // If other is a box collider
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
