using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Camera follows main character and camera cart.
 *  - It also sets the aspect and camera size based on what platform game is running on.
 */


public class CameraFollowTarget : MonoBehaviour {

    // Transform of what the camera follows
    public Transform player;

    // Transform of the camera cart
    public Transform cameraCart;

    // Offset for the camera
    public Vector3 offset;

    // If the camera should follow the targets
    public bool isMoving = true;

    // Aspect
    public float aspect;
    
    // Current aspect
    public float currentAspect;

    // Step for camera movement
    public float step = 0;

    // Zero vector3
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Set camera aspect
    //    DetermineCameraAspectAndSize();


        // If .exe then set the resolution
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // Set resolution
            Screen.SetResolution(1280, 800, false);

            // Set aspect
            GetComponent<Camera>().aspect = 16f / 10f;

        }

    }
    
	
	// Update is called once per frame
	void Update () {

        followPlayer();

	}


    void followPlayer()
    {
     
        // If moving
        if(isMoving)
        {
            // Used to catch up to the player
            if(player.position.x - cameraCart.GetComponent<Transform>().position.x >= 2f)
            {
                step = 0.001f;
            }
            else
            {
                step = 0.0001f;
            }

            // Follows the players x position and the camera carts y position
            transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y, -10),
            new Vector3(player.position.x + offset.x, cameraCart.GetComponent<Transform>().position.y + offset.y, -10),
            ref velocity, 0.0001f);
        }
        

    }

    /*
     * Determine what the camera aspect is based on current device.
     * Then set the aspect and adjust the size to show entire screen.
     * The camera size was determined by trial and error.
     */
    void DetermineCameraAspectAndSize()
    {
        // If .exe then set the resolution
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // Set resolution
            Screen.SetResolution(640, 960, false);

            // Set aspect
            GetComponent<Camera>().aspect = 2f / 3f;

            // Set size
            Camera.main.orthographicSize = 4.82f;
        }
        else
        {


            // Get the current aspect
            currentAspect = (float)Screen.width / (float)Screen.height;


            // 3:4 = 0.75
            if (currentAspect > 0.70)
            {
                // Set aspect
                GetComponent<Camera>().aspect = 3f / 4f;

                // Set size
                Camera.main.orthographicSize = 4.30f;
            }
            // 2:3 = 0.6666666667
            else if (currentAspect > 0.64)
            {
                // Set Aspect
                GetComponent<Camera>().aspect = 2f / 3f;

                // Set size
                Camera.main.orthographicSize = 4.82f;
            }
            // 10:16 = 0.625
            else if (currentAspect > 0.60)
            {
                // Set aspect
                GetComponent<Camera>().aspect = 10f / 16f;

                // Set size
                Camera.main.orthographicSize = 5.15f;
            }
            // 10:17 = 0.5882
            else if (currentAspect > 0.57)
            {
                // Set aspect
                GetComponent<Camera>().aspect = 10f / 17f;

                // Set size
                Camera.main.orthographicSize = 5.72f;
            }
            // 9:16 = 0.5625
            else if (currentAspect > 0.50)
            {
                // Set aspect
                GetComponent<Camera>().aspect = 9f / 16f;

                // Set size
                Camera.main.orthographicSize = 5.7f;
            }
            // Just incase
            else
            {
                // Set aspect
                GetComponent<Camera>().aspect = 9f / 16f;

                // Set size
                Camera.main.orthographicSize = 5.7f;
            }

        }
    }
}
