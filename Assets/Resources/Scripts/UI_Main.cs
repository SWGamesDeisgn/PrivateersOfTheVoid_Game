using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class UI_Main: MonoBehaviour {
    public bool GamePause;
	public int scoreCount;
	public Text Score;
    public Canvas UICanvas;
    private InputField resWidth;
    private InputField resHeight;
    private GameObject ResolutionsObject;
    private Dropdown CustomResolutions;
    public GameObject UIPauseMenu;
    public GameObject GameInfo;
    private Resolution[] resolutions;
    public bool bHideResolutions;
    private void Awake()
    {
        resolutions = Screen.resolutions;
        SetResolutions();
        UIPauseMenu = GameObject.Find("UIPauseMenu");
        if( UIPauseMenu )
        {
            ResolutionsObject = GameObject.Find("UIResolutionsDropdown");
            if( ResolutionsObject )
            {
                CustomResolutions = GameObject.Find("UIResolutionsDropdown").GetComponent<Dropdown>();
                if( bHideResolutions )
                {
                    ResolutionsObject.SetActive(false);
                }
            }
        }    
    }
 
    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 0;
		scoreCount = 0;
        Score.rectTransform.anchoredPosition = new Vector2(100, -44.0f);
    }

    // Update is called once per frame
    void Update() 
{
        ScoreCounter();
        // pauses the game using timescale when escape is pressed, and unpauses when escape is pressed again.
        if (Input.GetKeyDown(KeyCode.Escape)) { if (GamePause != true) { GamePause = true; Pause(); } else if (GamePause == true) { GamePause = false; Resume(); } }

    }

	void ScoreCounter()
	{
		Score.text = "Score: " + scoreCount.ToString ();
	}
    public void Restart() { SceneManager.LoadScene("Space"); }
    public void Quit() { Application.Quit(); }
    public void Resume() { UIPauseMenu.SetActive(false); Time.timeScale = 1; GamePause = false; }
    public void Pause() { UIPauseMenu.SetActive(true); Time.timeScale = 0; GamePause = true; }
    void SetResolutions()
    {       
        if ((resolutions[0].width == 1366) && (resolutions[0].height == 768)) { Debug.Log("Resolution is " + resolutions[0].width + " by " + resolutions[0].height); Screen.SetResolution(388, 690, false); }
        if ((resolutions[0].width == 1920) && (resolutions[0].height == 1080)) { Debug.Log("Resolution is " + resolutions[0].width + " by " + resolutions[0].height); Screen.SetResolution(560, 1000, false); }
    }
    public void CustomResolution()
    {
        if (ResolutionsObject.activeInHierarchy)
        {
            if (CustomResolutions.value == 1) { Debug.Log("1920 by 1080"); Screen.SetResolution(560, 1000, false); }
            if (CustomResolutions.value == 2) { Debug.Log("1366 by 768"); Screen.SetResolution(388, 690, false); }
        }        
    }
    public void GameInfoMethod() { GameInfo.SetActive(true); UIPauseMenu.SetActive(false); }
    public void GameInfoBackButtonMethod() { GameInfo.SetActive(false); UIPauseMenu.SetActive(true); }
}
