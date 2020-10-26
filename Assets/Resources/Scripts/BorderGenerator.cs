using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGenerator : MonoBehaviour
{
    public GameObject Border;
    private GameObject spawnee;
    private float x = 0.0f;
    public int Count;
    public int i;
    public int z;
    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
                spawnee = (GameObject)Instantiate(Border, gameObject.transform.position + new Vector3 (x, 0.0f , 0.0f), Quaternion.identity);
                x += 21; spawnee.transform.SetParent(GameObject.Find("UI_Button_Prefab").transform, false); Count++;
        }
        */
        if (i != 42)
        {
            for (i = 0; i < 42; ++i)
            {
                spawnee = (GameObject)Instantiate(Border, gameObject.transform.position + new Vector3(x, 0.0f, 0.0f), Quaternion.identity);
                x += 21; spawnee.transform.SetParent(GameObject.Find("UI_Button_Prefab").transform, false); Count++;
            }
        }
        if ((i == 42) && (z < 22))
        {
            float y = 21.0f;
            for (z = 0; z < 22; z++)
            {
                if (z == 0)
                {
                    spawnee = (GameObject)Instantiate(Border, gameObject.transform.position + new Vector3(x - 16, -5.0f, 0.0f), Quaternion.AngleAxis(90, Vector3.forward));
                    y += 5; spawnee.transform.SetParent(GameObject.Find("UI_Button_Prefab").transform, false); Count++; z++;
                }
                if (z > 0)
                {
                    spawnee = (GameObject)Instantiate(Border, gameObject.transform.position + new Vector3(x - 16, -y, 0.0f), Quaternion.AngleAxis(90, Vector3.forward));
                    y += 21.0f; spawnee.transform.SetParent(GameObject.Find("UI_Button_Prefab").transform, false); Count++;
                }
            }
            if ((z == 22) && (i == 42))
            {
                Vector3 DuplicatePos = new Vector3(222.5f, 425.0f, 0.0f);
                spawnee = (GameObject)Instantiate(gameObject, DuplicatePos, Quaternion.AngleAxis(270,Vector3.forward));
                spawnee.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }
        }
    }
}
