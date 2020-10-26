using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIUVAnimator : MonoBehaviour
{
    // I am a script that can animate the matierals assigned to a UI image/raw image.
    private float animSpeed = 20f;
    public CanvasRenderer rend;
    private float offset;

// Start is called before the first frame update
void Start()
    {
        // gets the canvas renderer of the gameobject that this script is attached to.
        rend = transform.GetComponent<CanvasRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (rend.materialCount != 0)
        {
            MoveObject();
        }
    }
    void MoveObject()
    {
        // offset is the amount by which the material will be translated per frame.
        // and then set the material offset of the main tex variable of the material assigned to the game object.
        // GetMaterial(n) where n is the interger index of the materials array assigned to the image script on the UI game object.
        offset -= (Time.deltaTime) / animSpeed;
        rend.GetMaterial().SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }
}
