using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour
{
    public int TouchLimiter = 100;
    private float touchXLower;
    private float touchXUpper;
    private float touchYLower;
    private float touchYUpper;
    public RectTransform Background;
    public Player_Weapons_V2 PlayerWeaponsScript;
    public GameObject PlayerWeapons;
    private Touch touch;
    public bool touchLock;

    // Start is called before the first frame update
    void Start()
    {
//        if (gameObject.name == "FireButton")
        PlayerWeapons = GameObject.FindWithTag("PlayerWeaponSpawner");
        PlayerWeaponsScript = PlayerWeapons.gameObject.GetComponent<Player_Weapons_V2>();
        touchXLower = Background.position.x - ((Background.sizeDelta.x / 2) + TouchLimiter);
        touchXUpper = Background.position.x + ((Background.sizeDelta.x / 2) + TouchLimiter);

        touchYLower = Background.position.y - (Background.sizeDelta.y / 2) - TouchLimiter;
        touchYUpper = Background.position.y + ((Background.sizeDelta.y / 2) + TouchLimiter);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (((touch.position.x > touchXLower) && (touch.position.x < touchXUpper)) && ((touch.position.y > touchYLower) && (touch.position.y < touchYUpper)))
            {
                PlayerWeaponsScript.FireTheGuns = true;
                touchLock = true;
            }
            else
            {
                PlayerWeaponsScript.FireTheGuns = false;
                touchLock = false;
            }
        }
        if ((Input.touchCount > 1) && (touchLock == false))
        {
            Touch touch = Input.GetTouch(1);
            if (((touch.position.x > touchXLower) && (touch.position.x < touchXUpper)) && ((touch.position.y > touchYLower) && (touch.position.y < touchYUpper)))
            {
                PlayerWeaponsScript.FireTheGuns = true;
            }
            else
            {
                PlayerWeaponsScript.FireTheGuns = false;
            }
        }

        if (Input.touchCount == 0)
        {
            PlayerWeaponsScript.FireTheGuns = false;
            touchLock = false;
        }
    }        
}
