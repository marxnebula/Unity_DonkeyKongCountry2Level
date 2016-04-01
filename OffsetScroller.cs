using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour {

    // Get the cart
    public GameObject cart;

    // User sets scroll speed
	public float scrollSpeed;

    // Stored offset
    private Vector2 savedOffset;

    void Start () {
        // Store the offset
        savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
    }

    void Update () {
        // Set the position
        GetComponent<Transform>().position = new Vector3(cart.GetComponent<Transform>().position.x,
            GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);

        float x = Mathf.Repeat (Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(x, savedOffset.y);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    void OnDisable () {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }

}
