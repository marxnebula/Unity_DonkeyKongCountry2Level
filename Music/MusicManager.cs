using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    // The audio clips for main songs and death
    public AudioClip mainMenu;
    public AudioClip theme;
    public AudioClip death;
    public AudioClip end;

    private bool doOnce = true;

	// Use this for initialization
	void Start () {

        // If game play scene
	    if (SceneManager.GetActiveScene().name == "normal")
        {
            // Set main theme
            GetComponent<AudioSource>().clip = theme;
        }
        // Else if main menu scene
        else if (SceneManager.GetActiveScene().name == "mainMenu")
        {
            // Set main menu theme
            GetComponent<AudioSource>().clip = mainMenu;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
        // I game over
        if(GameControl.control.GetGameOver())
        {
            // Set death clip
            GetComponent<AudioSource>().clip = death;

            // If doOnce
            if (doOnce)
            {
                // Set boolean to false
                doOnce = false;

                // Do not loop
                GetComponent<AudioSource>().loop = false;

                // Play music
                GetComponent<AudioSource>().Play();
            }
        }
        // Else if it is end
        else if(GameControl.control.GetIsEnd())
        {
            // Set end clip
            GetComponent<AudioSource>().clip = end;

            // If doOnce
            if (doOnce)
            {
                // Set boolean to false
                doOnce = false;

                // Play music
                GetComponent<AudioSource>().Play();
            }
        }

        
	}
}
