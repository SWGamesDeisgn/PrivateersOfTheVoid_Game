using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    public GameObject objUI;
    public bool bEnableShopTime = false;
    private Scene cScene;


    private void Awake()
    {
        cScene = SceneManager.GetActiveScene();
        
        if (objUI)
        {
            if (objUI.activeInHierarchy != true)
            {
                objUI.SetActive(true);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (true == bEnableShopTime || "UpgradeShopScene" == cScene.name)
        {
            Time.timeScale = 1;
            Debug.Log("Shop Scene Detected, Setting timescale to 1!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
