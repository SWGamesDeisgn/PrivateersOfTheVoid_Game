using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimator : MonoBehaviour {


    // This script might be merged with other UV/UI animator scripts and make use of the game object's name to differenciate between the required variables.
	private float animSpeed  = 20f;
	public Renderer rend;
	private float offset; 

	void Start ()
	{
        // Get the renderer component of the game object.
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        // Translate the material of the game object this script is assigned to by offset.
        offset -= (Time.unscaledDeltaTime) / animSpeed; rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
	}
}
