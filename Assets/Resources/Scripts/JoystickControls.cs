using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControls : MonoBehaviour
{
    private Vector2 JoystickStartLoc;
    public RectTransform Background;
    public RectTransform Joystick;
    public Vector2 inputVector;
    [Range(0f,1f)]public float JoystickLimit = 0.5f;
    public int TouchLimiter = 100;
    private float touchXLower;
    private float touchXUpper;
    private float touchYLower;
    private float touchYUpper;
    public bool touchLock;


    void Start()
    {
        JoystickStartLoc = Joystick.transform.position;
        inputVector = Vector2.zero;

        touchXLower = Background.position.x - ((Background.sizeDelta.x / 2) + TouchLimiter);
        touchXUpper = Background.position.x + ((Background.sizeDelta.x / 2) + TouchLimiter);

        touchYLower = Background.position.y - (Background.sizeDelta.y / 2) - TouchLimiter;
        touchYUpper = Background.position.y + ((Background.sizeDelta.y / 2) + TouchLimiter);
    }

    private void Update()

    {
        if (Input.touchCount > 0)
        {
            print("Start Position is " + JoystickStartLoc);
            
            Touch touch = Input.GetTouch(0);
            /*
            if (((touch.position.x > touchXLower) && (touch.position.x < touchXUpper)) && ((touch.position.y > touchYLower) && (touch.position.y < touchYUpper)))
            {
                Vector2 direction = touch.position - JoystickStartLoc;
                inputVector = (direction.magnitude > Background.sizeDelta.x / 2f) ? direction.normalized : direction / (Background.sizeDelta.x / 2f);
                Joystick.anchoredPosition = (inputVector * Background.sizeDelta.x / 2f) * JoystickLimit;
                touchLock = true;
            }
            */

            if (Input.touchCount > 0)
            {
                Vector2 direction = touch.position - JoystickStartLoc;
                inputVector = (direction.magnitude > Background.sizeDelta.x / 2f) ? direction.normalized : direction / (Background.sizeDelta.x / 2f);
                Joystick.anchoredPosition = (inputVector * Background.sizeDelta.x / 2f) * JoystickLimit;
                touchLock = true;
            }
            else
            {
                touchLock = false;
                Joystick.anchoredPosition = Vector2.zero;
                inputVector = Vector2.zero;
            }
            print(touch.position);
        }
        if ((Input.touchCount > 1) && (touchLock == false))
        {
            Touch touch = Input.GetTouch(1);
            if (((touch.position.x > touchXLower) && (touch.position.x < touchXUpper)) && ((touch.position.y > touchYLower) && (touch.position.y < touchYUpper)))
            {
                Vector2 direction = touch.position - JoystickStartLoc;
                inputVector = (direction.magnitude > Background.sizeDelta.x / 2f) ? direction.normalized : direction / (Background.sizeDelta.x / 2f);
                Joystick.anchoredPosition = (inputVector * Background.sizeDelta.x / 2f) * JoystickLimit;
            }
            else
            {
                Joystick.anchoredPosition = Vector2.zero;
                inputVector = Vector2.zero;
            }
        }
        if (Input.touchCount == 0)
        {
            Joystick.anchoredPosition = Vector2.zero;
            inputVector = Vector2.zero;
            touchLock = false;
        }

    }
}
