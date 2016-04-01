using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour {


    void Start()
    {
        // Starts from random animation
        GetComponent<Animator>().Play(0, -1, Random.Range(0f, 1f));
    }

    /*
     * 
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is the cart
        if (other.tag == "Cart")
        {
            // If it is a box collider
            if (other == other.GetComponent<BoxCollider2D>())
            {
                // Play the audio
                GetComponent<AudioSource>().Play();

                // Add the banana

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
