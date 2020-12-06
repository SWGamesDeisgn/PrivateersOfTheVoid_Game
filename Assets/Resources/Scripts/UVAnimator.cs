using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimator : MonoBehaviour {


    // This script might be merged with other UV/UI animator scripts and make use of the game object's name to differenciate between the required variables.
	private float animSpeed  = 20f;
	public Renderer rend;
	private float offset;
    private Material AnimMat;
    public int MaterialIndex = 0;
	void Start ()
	{
        // Get the renderer component of the game object.
        rend = GetComponent<Renderer>();
        if (MaterialIndex >= 0)
        {
            AnimMat = rend.materials[MaterialIndex];
        }
        else
        {
            Debug.Log(gameObject.name + " UVAnimator.CS Animated Material Index Null");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Translate the material of the game object this script is assigned to by offset.
        if (MaterialIndex >= 0)
        {
            offset -= (Time.unscaledDeltaTime) / animSpeed; AnimMat.SetTextureOffset("_MainTex", new Vector2(0, offset));
        }
	}
}
