using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


/* ~~~~~~~~~~ Class Info ~~~~~~~~~~
 *  - Class for storing saved variables.
 *  - Doesn't delete when you change scenes.
 *  - Also saves user data to a file in all platforms except the web.
 *  - Must put this class in each scene so it is a PREFAB.
 */

public class GameControl : MonoBehaviour {

    public static GameControl control;

    /* Variables for saving */
    public int coins;
    public float time = 0;


    // List for storing the vectors of the edge collider points
    private List<Vector2[]> vectorList;


    // Booleans for drawing which GUI
    private bool isMainMenu = false;
    public bool isGamePlay = false;
    private bool isGameOver = false;
    private bool isEnd = false;

    public bool isText = false;

	// Awake happens before Start()
	void Awake () {

        /*
         * So there can only be 1 control.(singleton)
         */
        // If control doesn't exist
        if(control == null)
        {
            // Doesn't destroy the game object when you change scenes
            DontDestroyOnLoad(gameObject);

            // Set control to this
            control = this;

        }
        // If control does exist but it is not this
        else if(control != this)
        {
            // Destroy gameobject because one already exists
            Destroy(gameObject);
        }
        
	}


    // Dat update
    void Update()
    {
        // Timer inbetween gates
        Timer();

        // Logic for when it is game over
        GameOver();

        print(control.isGameOver);
        
    }


    /*
     * Determine variables based on if game over
     */
    void GameOver()
    {
        // If game over
        if (isGameOver)
        {
            // Stop moving camera
            Camera.main.GetComponent<CameraFollowTarget>().isMoving = false;

            // Set gravity of skeleton pirate
            GameObject.FindGameObjectWithTag("SkeletonPirate").GetComponent<Rigidbody2D>().gravityScale = 1f;

            // Stop scrolling the background
            GameObject.FindGameObjectWithTag("Background").GetComponent<OffsetScroller>().enabled = false;

            // Turn off cart script
            GameObject.FindGameObjectWithTag("Cart").GetComponent<Cart>().enabled = false;

            // Set time to 0
            GameControl.control.SetTime(0);


        }
        else
        {
            // Set isMoving to true
            Camera.main.GetComponent<CameraFollowTarget>().isMoving = true;

            // Set gravity to 0
            GameObject.FindGameObjectWithTag("SkeletonPirate").GetComponent<Rigidbody2D>().gravityScale = 0f;

            // Turn on scrolling background
            GameObject.FindGameObjectWithTag("Background").GetComponent<OffsetScroller>().enabled = true;

            // Turn on cart script
            GameObject.FindGameObjectWithTag("Cart").GetComponent<Cart>().enabled = true;

        }
    }


    public void AddTime(float addTime)
    {
        // Add the time
        control.time = control.time + addTime;
    }


 
    public void SetTime(float setTime)
    {
        control.time = setTime;
    }


    public float GetTime()
    {
        return time;
    }


    public void SetGameOver(bool b)
    {
        control.isGameOver = b;
    }


    public bool GetGameOver()
    {
        return control.isGameOver;
    }


    public void SetIsEnd(bool b)
    {
        isEnd = b;
    }

    public bool GetIsEnd()
    {
        return isEnd;
    }


    public void SetIsText(bool b)
    {
        control.isText = b;
    }


    public bool GetIsText()
    {
        return control.isText;
    }


    /*
     * Timer for when cart inbetween gates. 
     */
    void Timer()
    {
        // Don't let it go below 0
        if(control.time < 0)
        {
            control.time = 0;
        }
        else
        {
            control.time = (control.time - Time.deltaTime);
        }
        
    }


    public void SetVectorList(List<Vector2[]> v)
    {
        control.vectorList = v;
    }


    public List<Vector2[]> GetVectorList()
    {
        return control.vectorList;
    }

	

    /*
     * Saves data out into a file. This works on all platforms except the web.
     * You could save file as playerInfo.anything or just playerInfo
     */
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        // Create the file.
        // Application.persistentDataPath is the folder its going to and
        // "/playerInfo.dat" is the file name.
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        // Set the variables
        PlayerData data = new PlayerData();
        data.coins = coins;
        data.time = time;

        // Save the data to the file
        bf.Serialize(file, data);

        file.Close();
    }


    /*
     * Loads data from a file.  Make sure to check if file exist.
     * This works on all platforms except the web.
     */
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            // Need to cast meaning specify that this is a player data object.
            // So it pulls the PlayerData data out of the file
            PlayerData data = (PlayerData)bf.Deserialize(file);

            file.Close();

            // Set your variables to the loaded data
            coins = data.coins;
            time = data.time;
        }
    }

}

/* 
 * This class needs to be serializable... meaning this data can be written to a file.
 */
[Serializable]
class PlayerData
{
    public int coins;
    public float time;
}
