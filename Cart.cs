using UnityEngine;
using System.Collections;

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



    // Use this for initialization
    void Start()
    {
        // Store the speed
        storedSpeed = speed;

        // Get the audio sources
        audioSources = GetComponents<AudioSource>();
    }


    // Use fixed update if using rigidbody
    void FixedUpdate()
    {
        Speed();

        Gravity();

        Jump();

        PlaySound();


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

}