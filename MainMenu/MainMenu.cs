using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        // Press any button to load normal scene
        if(Input.anyKey)
        {
            SceneManager.LoadScene("normal");
        }
	}
}
