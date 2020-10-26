using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    public GameObject objUI;

    private void Awake()
    {
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
