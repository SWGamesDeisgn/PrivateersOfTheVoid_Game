using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int i_SpawnedAmount;
    public GameObject GO_ObjectToPool;
    public bool b_Expand;
}
public class Pooling : MonoBehaviour
{
    public int TimeToPreCache;
    public static Pooling c_PoolInstance;
    public List<ObjectPoolItem> L_ObjectsToPool;
    public List<GameObject> L_SpawnedObjects;
    private GameObject ObjectPool;
    private bool bPreCachingFinished = false;
    private UI_Main UIObj;
    private bool bPrecachingFinished = false;
    // Start is called before the first frame update
    void Awake()
    {
        c_PoolInstance = this;
    }

    void Start()
    {
        ObjectPool = GameObject.Find("PooledObjects");
        UIObj = GameObject.Find("UI").GetComponent<UI_Main>();
        // For as long as int i is less than the spawned amount iterate int i and create a GameObject called obj then setting it inactive and adding it to the list of spawned objects.
        L_SpawnedObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in L_ObjectsToPool)
        {
            for (int i = 0; i < item.i_SpawnedAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.GO_ObjectToPool);
                obj.transform.SetParent(GameObject.Find("PooledObjects").transform, false);
                obj.SetActive(false);
                L_SpawnedObjects.Add(obj);
            }
        }
        bPrecachingFinished = true;
        StartCoroutine(PreCaching());
    }
    public List<GameObject> GetList()
    {
        return L_SpawnedObjects;
    }
    // Update is called once per frame
    void Update()
    {
        if (!bPreCachingFinished)
        {
            if (null != UIObj)
            {
                UIObj.GamePause = true;
            }          
           // Time.timeScale = 0;
        }
    }
    private IEnumerator PreCaching()
    {
        foreach (GameObject obj in L_SpawnedObjects)
        {
            obj.SetActive(true);
            obj.transform.Translate(0.0f, 0.0f, 0.0f);
            yield return new WaitForSecondsRealtime(0.1f);
            obj.SetActive(false);
        }
        if (null != UIObj)
        {
            UIObj.GamePause = false;
        }
        bPreCachingFinished = true;
        Debug.Log("Pre Caching Completed");
        yield return null;
    }
    public GameObject GetFromPool(string tag)
    {
        for (int i = 0; i < L_SpawnedObjects.Count; i++)
        {
            if (L_SpawnedObjects[i].activeInHierarchy == false && L_SpawnedObjects[i].tag == tag)
            {
                return L_SpawnedObjects[i];
            }
        }

        foreach (ObjectPoolItem item in L_ObjectsToPool)
        {
            if (item.GO_ObjectToPool.tag == tag)
            {
                if (item.b_Expand)
                {
                    GameObject obj = (GameObject)Instantiate(item.GO_ObjectToPool);
                    obj.transform.SetParent(GameObject.Find("PooledObjects").transform, false);
                    obj.SetActive(false);
                    L_SpawnedObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
    public bool PreCachingFinished()
    {
        return bPrecachingFinished;
    }
}
