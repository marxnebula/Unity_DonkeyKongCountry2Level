using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Displays all UI buttons.
 *  - If game over it displays play button.
 *  - Replay button will load the game play screen.
 */

public class UI : MonoBehaviour {

    // Text for UI
    public Text timeText;

    // Images for UI
    public Image playButton;
    public Image endImage;

    // Booleans for drawing UI
    private bool isMainMenu = false;
    private bool isGamePlay = false;
    public bool isGameOver = false;


    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update () {

        // Display users stats
        DisplayUserStats();

        // Check the users input
        CheckUserInput();

        // Display game over
        DisplayGameOver();

        // Display end screen
        DisplayEnd();
	}


    /*
     * Checks users input based on which platform they are on.
     * It then calls a function to check if a certain button was pressed.
     */
    void CheckUserInput()
    {
        // If user is running on android
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                // TouchPhase.Began means a finger touched the screen
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    CheckWhichButtonTouched(Input.GetTouch(0).position);
                }
            }
        }

        // If user is running the editor, windows, or mac
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // If left mouse button is up
            if (Input.GetMouseButtonUp(0))
            {
                CheckWhichButtonTouched(Input.mousePosition);
            }
        }
    }


    /*
     * Checks which button was pressed if any.
     */
    void CheckWhichButtonTouched(Vector3 pos)
    {
        
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = pos;

        // Gets a list of raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);


        if (raycastResults.Count > 0)
        {
            // If it's play button
            if (raycastResults[0].gameObject.name == "PlayButton")
            {
                print("Play button hit");

                // If this is a replay then load new level
                if(GameControl.control.GetGameOver())
                {
                    GameControl.control.SetGameOver(false);

                    // Load the replay level
                    SceneManager.LoadScene("normal");

                }
            }
        }
    }


   


    /*
     * Displays the users stats which are in GameControl.
     */
    void DisplayUserStats()
    {
        // If text is true
        if(GameControl.control.GetIsText())
        {
            // Show the time
            timeText.text = "" + (int)GameControl.control.time;
        }
        else
        {
            // Show nothing
            timeText.text = "";
        }
        
    }


    /*
     * Displays game over screen.
     */
    void DisplayGameOver()
    {
        // If game over
        if(GameControl.control.GetGameOver())
        {
            // Display button for replay
            playButton.enabled = true;

            // Turn off text
            timeText.enabled = false;
        }
        else
        {
            // Turn off play button
            playButton.enabled = false;

            // Turn on time text
            timeText.enabled = true;
            
        }
    }

    /*
     * End screen.
     */
    void DisplayEnd()
    {
        // If at the end
        if(GameControl.control.GetIsEnd())
        {
            print("YOU WIN");

            // Display end image
            endImage.enabled = true;
        }
    }


    /*
     * Sets isGameOver boolean to true.
     */
    public void SetGameOver()
    {
        isGameOver = true;
    }


    /*
     * Sets isGameOver boolean to true.
     */
    public bool GetGamePlay()
    {
        return isGamePlay;
    }
}
