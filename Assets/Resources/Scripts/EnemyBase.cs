using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [HideInInspector] public GameObject UpgradeMaster;
    private GameObject spawnee;
    private GameObject UpgradeBeamLaser;
    private GameObject UpgradeMulti;
    private GameObject UpgradeLife;
    public float EnemyMoveSpeed;
    public int EnemyHealth;
    private float pathTimer;
    private Upgrades UpgradeDetect;
    protected UI_Main ScoreSet;
    [HideInInspector] public EnemySpawning spawner;
    private bool upgradeDropped;
    [HideInInspector]public bool MovementEnabled;
    [HideInInspector]public int PathSelection = 0;
    [HideInInspector]public float PathDelay = 0.0f;
    [HideInInspector]public float enemySpeedModifier = 0.0f;
    private float speedModifier = 0.0f;
    public Pooling AssetPool;
    private int MultiShotRarity;
    private BoxCollider boxColFoV;
    private GameObject objPlayer;
    private bool bPauseTimer = false;
    private bool bAttacking = false;
    private bool bAttackFinished = false;
    private string strTargetName = "";
    public int MissleSpeed = 6;
    public GameObject Missle;
    public Transform MissleLoc;
    private GameObject missle;
    private Quaternion quatLookRotation;
    bool bLookingAtTarget = false;
    public void FindTheComponents()
    {
        UpgradeMaster = Resources.Load<GameObject>("Upgrades/UpgradesMaster") as GameObject;
        SetTheUpgrades();
        spawner = GameObject.Find("EnemySpawning").GetComponent<EnemySpawning>();
        ScoreSet = GameObject.Find("UI").GetComponent<UI_Main>();
        UpgradeDetect = GameObject.Find("UpgradeBrain(Clone)").GetComponent<Upgrades>();
        AssetPool = GameObject.Find("GameMaster").GetComponent<Pooling>();
        boxColFoV = transform.GetChild(1).gameObject.GetComponent<BoxCollider>();
    }
    public void Timer()
    {
        if (false == bPauseTimer)
        {
            pathTimer += Mathf.RoundToInt(Time.fixedDeltaTime * 100.0f) / 100.0f;
        }
    }
    public void PathA()
    {
        transform.Translate(Vector3.forward * (EnemyMoveSpeed * Time.fixedDeltaTime));
        if (pathTimer >= (8.0f / EnemyMoveSpeed))
        {
            if (gameObject.transform.position.x < 0)
            {
                gameObject.transform.rotation = Quaternion.AngleAxis(225.0f, Vector3.up);
                Destroy(gameObject, 4f);
            }
            if (gameObject.transform.position.x > 0)
            {
                gameObject.transform.rotation = Quaternion.AngleAxis(135.0f, Vector3.up);
                Destroy(gameObject, 4f);
            }
            if (gameObject.transform.position.z <= -6.5f) { Destroy(gameObject); }
        }
    }
    public void PathB()
    {
        gameObject.transform.rotation = Quaternion.AngleAxis(45.0f, Vector3.up);
        transform.Translate(Vector3.forward * (EnemyMoveSpeed * Time.fixedDeltaTime));
        Destroy(gameObject, 8f);
    }
    public void PathC()
    {
        if (false == bPauseTimer)
        {
            speedModifier = EnemyMoveSpeed * enemySpeedModifier;
            float step = EnemyMoveSpeed * (Time.fixedDeltaTime * 20);
            if (pathTimer < (PathDelay / EnemyMoveSpeed)) { transform.Translate(Vector3.forward * (Time.deltaTime * EnemyMoveSpeed)); }
            if (pathTimer >= (PathDelay / EnemyMoveSpeed))
            {
                transform.Translate(Vector3.forward * (Time.deltaTime * speedModifier));
                gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.AngleAxis(270, Vector3.up), step);
            }
            if (gameObject.transform.rotation == Quaternion.Euler(0, 270, 0)) { enemySpeedModifier = 1; }
            if ((gameObject.transform.position.x >= 7.0f) || ((gameObject.transform.position.z <= -6.5f)) || ((gameObject.transform.position.x <= -7.0f))) { Destroy(gameObject); }
        }
    }
    
    public void AttackPathA()
    {
        if (gameObject.transform.position.z <= 0.0f)
        {
            if ((null != objPlayer) && (false == bAttackFinished))
            {
                if (false == bAttackFinished)
                {
                    StartCoroutine(AttackTimer());
                    bPauseTimer = true;
                }
            }
        }
       if (false == bPauseTimer)
        {
            PathA();
        }
    }
    private IEnumerator AttackTimer()
    {
        // https://answers.unity.com/questions/254130/how-do-i-rotate-an-object-towards-a-vector3-point.html
        quatLookRotation = Quaternion.LookRotation((objPlayer.transform.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, quatLookRotation, Time.deltaTime * 4);
        yield return new WaitForSeconds(1.0f);
        bLookingAtTarget = true;
        if (true == bPauseTimer && bLookingAtTarget == true)
        {
            if (false == bAttackFinished)
            {
                Fire();
                print(transform.name + " has launched an attack on " + strTargetName + "!");
                bAttackFinished = true;
            }
            bLookingAtTarget = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(180.0f, Vector3.up), Time.deltaTime * 4);
            yield return new WaitForSeconds(1.0f);
            bPauseTimer = false;                           
            StopCoroutine(AttackTimer());
            yield return null;
        }        
        yield return null;
    }
    public void LootDrop()
    {
        if (AssetPool)
        {
            var multiShotCount = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Control>();
            int upgradeDropper = (Random.Range(0, 200));

            //when an upgrade is spawned the information is sent to an offsite brain to relay the information to the other member of the group.
            if (upgradeDropper < 30)
            {
                Debug.Log(upgradeDropper);
                if (!upgradeDropped)
                {
                    if ((upgradeDropper <=20 && upgradeDropper > 10) && (UpgradeDetect.BeamLaserDropped == 0) && (UpgradeDetect.beamLaserCounter == 0))
                    {
                        UpgradeDetect.BeamLaserDropped += 1;
                        upgradeDropped = true;
                        spawnee = AssetPool.GetFromPool("UpgradeBeamLaser");
                        if (null != spawnee)
                        {
                            spawnee.transform.position = (gameObject.transform.position + new Vector3(0, 0.2f, 0));
                            spawnee.transform.rotation = (gameObject.transform.rotation);
                            spawnee.SetActive(true);
                        }
                    }
                    if ((upgradeDropper <= 10) && (UpgradeDetect.MultiSpawnAmount < multiShotCount.MaxMultiShot))
                    {
                        UpgradeDetect.MultiSpawnAmount += 1;
                        upgradeDropped = true;
                        spawnee = AssetPool.GetFromPool("TripleShotUpgrade");
                        if (null != spawnee)
                        {
                            spawnee.transform.position = (gameObject.transform.position + new Vector3(0, 0.2f, 0));
                            spawnee.transform.rotation = (gameObject.transform.rotation);
                            spawnee.SetActive(true);
                        }
                    }
                    if ((upgradeDropper <= 30 && upgradeDropper > 20) && (UpgradeDetect.UpgradeLifeCounter == 0))
                    {
                        UpgradeDetect.UpgradeLifeCounter += 1;
                        upgradeDropped = true;
                        spawnee = AssetPool.GetFromPool("PlayerLifeUpgrade");
                        if (null != spawnee)
                        {
                            spawnee.transform.position = (gameObject.transform.position + new Vector3(0, 0.2f, 0));
                            spawnee.transform.rotation = (gameObject.transform.rotation);
                            spawnee.SetActive(true);
                        }
                    }
                }
            }
        }
    }
    public void SetTheUpgrades()
    {
        UpgradeBeamLaser = UpgradeMaster.transform.GetChild(0).gameObject;
        UpgradeMulti = UpgradeMaster.transform.GetChild(1).gameObject;
        UpgradeLife = UpgradeMaster.transform.GetChild(2).gameObject;
    }
    public void PathSelector()
    {
        int Pathing = PathSelection;
        switch (Pathing)
        {
            case 0:
                PathA();
                break;
            case 1:
                PathB();
                break;
            case 2:
                PathC();
                break;
            case 3:
                AttackPathA();
                break;
            default:
                break;
        }
    }
    void OnTriggerEnter(Collider boxColFoV)
    {
        if (boxColFoV.gameObject.CompareTag("Player"))
        {
            objPlayer = boxColFoV.gameObject;
            strTargetName = objPlayer.name;
            //print(boxColFoV.gameObject.name + " detected");
        }
    }
    void OnTriggerExit(Collider boxColFoV)
    {
        if (boxColFoV.gameObject.CompareTag("Player"))
        {
            objPlayer = null;
            //print(boxColFoV.gameObject.name + " no longer detected");
        }
    }
    void Fire()
    {
        switch (null != AssetPool)
        {
            case true:
                missle = AssetPool.GetFromPool("Missles");
                if (null != missle)
                {
                    missle.transform.position = (MissleLoc.position + new Vector3(0.0f, 0, 0.5f));
                    missle.transform.rotation = transform.rotation;
                    missle.SetActive(true);
                    missle.GetComponent<Rigidbody>().velocity = missle.transform.forward * MissleSpeed;
                    missle.GetComponent<RaycastEmitter>().SetOwner("Enemy");
                }
                break;
            case false:
                break;
            default:
                break;
        }
    }
}

