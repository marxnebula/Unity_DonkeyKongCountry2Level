using UnityEngine;
using System.Collections;

public class TimeBarrel : MonoBehaviour {

    // User sets time
    public float time = 0f;

    // Splinter prefab
    public GameObject splinterPrefab;

    // The two splinteres to be created
    private GameObject splinter1;
    private GameObject splinter2;

    // The audiosources
    private AudioSource[] audioSources;


    void Start()
    {
        // Get the audio sources
        audioSources = GetComponents<AudioSource>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // If other is cart
        if (other.tag == "Cart")
        {
            // If other is box collider
            if (other == other.GetComponent<BoxCollider2D>())
            {
                // Play the audios
                for (int i = 0; i < audioSources.Length; i++)
                {
                    audioSources[i].Play();
                }

                // Add the time
                GameControl.control.AddTime(time);

                // Destroy the collider
                Destroy(GetComponent<BoxCollider2D>());

                // Trigger animation
                GetComponent<Animator>().SetTrigger("Explosion");

                // Create splinter
                splinter1 = (GameObject)Instantiate(splinterPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    Quaternion.Euler(new Vector3(0, 0, 55)));

                // Create a second splinter
                splinter2 = (GameObject)Instantiate(splinterPrefab, new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z),
                    Quaternion.Euler(new Vector3(0, 0, 55)));

                // Set one of the splinters speed to be different
                splinter2.GetComponent<Animator>().speed = 0.5f;

                // Add a force to the splinters
                splinter1.GetComponent<Rigidbody2D>().AddForce(new Vector2(30f, 120f));
                splinter2.GetComponent<Rigidbody2D>().AddForce(new Vector2(-30f, 115f));

                // Destroy the splinters after couple seconds
                Destroy(splinter1, 5f);
                Destroy(splinter2, 5f);

                // Destroy the barrel after explosion animation was played
                Destroy(gameObject, GetComponent<Animator>().runtimeAnimatorController.animationClips.Length);
            }
        }
    }
}
