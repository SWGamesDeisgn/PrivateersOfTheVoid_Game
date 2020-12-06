using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    private float animSpeed = 2f;
    private GameObject Target1;
    private GameObject Target2;
    private float offset;
    private float timer;
    private float waittime;
    public bool StopGates = false;
    private UI_Main UIObj;
    private GameObject GameMaster;
    public bool IgnorePooling = false;
    private bool bPoolingComplete;
    
// Start is called before the first frame update
void Start()
    {
        // if the UIGiantHanger is the object this script is assigned to then, find the the "sides" of the giant hanger doors and assign them to the "Target" variables.
        if (gameObject.name.Contains ("UIGiantHanger")) { Target1 = GameObject.Find("UIGiantHangerDoorLeftObj"); Target2 = GameObject.Find("UIGiantHangerDoorRightObj"); waittime = 2.0f; }
        UIObj = GameObject.Find("UI").GetComponent<UI_Main>();
        GameMaster = GameObject.Find("GameMaster");
    }
    // Update is called once per frame
    void Update()
    {
        if (null != UIObj)
        {
            if (null != GameMaster && UIObj.GamePause == false)
            {
                bPoolingComplete = GameMaster.GetComponent<Pooling>().PreCachingFinished() == true;
            }
            if (UIObj.GamePause == false && ( true == bPoolingComplete || true == IgnorePooling ) )
            {
                // if the UIGiantHanger is the object this script is assigned to then, proceed to move it's children in the X axis for 4.0 Seconds.
                if (gameObject.name.Contains("UIGiantHanger"))
                {
                    if (StopGates == false)
                    {
                        timer += Time.deltaTime;
                        if (timer > waittime)
                        {
                            Target1.transform.Translate(-animSpeed * Time.deltaTime, 0.0f, 0.0f); Target2.transform.Translate(animSpeed * Time.deltaTime, 0.0f, 0.0f);
                        }
                        if (timer >= 4.0f) { print("Gates have stopped"); gameObject.SetActive(false); StopGates = true; }
                    }
                }
            }
        }
    }
}
