using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    private GameObject spawnee;
    public GameObject Spawned1;
	public GameObject Spawned;
	public int EnemyHealth;
	public float enemyMoveSpeed;
	private bool MultiSpawnStop;
	public int MultiHasSpawned = 0;
	public Upgrades UpgradeDetect;
	public UI_Main ScoreSet;
    private EnemySpawning spawner;
    private int upgradeDropper;
	private bool upgradeDropped;

    public float pathTimer;
    private bool MovementEnabled;

    void Start()
	{
        spawner = GameObject.Find("EnemySpawning").GetComponent<EnemySpawning>();
        ScoreSet = GameObject.Find("UI").GetComponent<UI_Main>();
        UpgradeDetect = GameObject.Find("UpgradeBrain(Clone)").GetComponent<Upgrades>();
        if (gameObject.name.Contains("EnemyCorsair")) { enemyMoveSpeed = 1f; EnemyHealth = 16; }
        if (gameObject.name.Contains("EnemyFighter")) { enemyMoveSpeed = 2f; EnemyHealth = 8; }
    }
    void Update()
    {
        if (false == ScoreSet.GamePause)
        {
            pathTimer += Mathf.RoundToInt(Time.fixedDeltaTime * 100.0f) / 100.0f;
            if (spawner.EnableEnemyMovement == true)
            {
                MovementEnabled = true;
            }
            int upgradeDropper = (Random.Range(0, 101));
            //this controls when the enemies of this class will be destroyed, and also if they drop an upgrade pickup based on randon range.
            if (EnemyHealth <= 0)
            {
                //adds 100 value to the scoreCount variable in the UI script
                ScoreSet.scoreCount += 100;
                //when an upgrade is spawned the information is sent to an offsite brain to relay the information to the other member of the group.
                if ((upgradeDropper <= 50) && (upgradeDropped == false) && (UpgradeDetect.MultiSpawnAmount < 2))
                {
                    UpgradeDetect.MultiSpawnAmount += 1;
                    upgradeDropped = true;
                    spawnee = (GameObject)Instantiate(Spawned, gameObject.transform.position + new Vector3(0, 0.2f, 0), gameObject.transform.rotation);
                    spawnee.name = ("UpgradeMultiShot");
                    spawnee.GetComponent<Rigidbody>().useGravity = false;
                    spawnee.AddComponent<UpgradeMovement>();
                }
                if ((upgradeDropper <= 100) && (upgradeDropped == false) && (UpgradeDetect.BeamLaserDropped == 0) && (UpgradeDetect.beamLaserCounter < 1))
                {
                    UpgradeDetect.BeamLaserDropped += 1;
                    upgradeDropped = true;
                    spawnee = (GameObject)Instantiate(Spawned1, gameObject.transform.position + new Vector3(0, 0.2f, 0), gameObject.transform.rotation);
                    spawnee.name = ("UpgradeBeamLaser");
                    spawnee.GetComponent<Rigidbody>().useGravity = false;
                    spawnee.AddComponent<UpgradeMovement>();
                }
                Destroy(gameObject);
            }
            // Moves the birdie.	
            if (MovementEnabled == true)
            {
                PathA();
            }
        }
    }

    private void PathA()
    {
        transform.Translate(Vector3.back * (Time.deltaTime * enemyMoveSpeed));
        if (pathTimer >= (8.0f / enemyMoveSpeed))
       // if (gameObject.transform.position.z <= 0)
        {
            if (gameObject.transform.position.x < 0)
            {
                gameObject.transform.rotation = Quaternion.AngleAxis(45.0f, Vector3.up);
                Destroy(gameObject, 4f);
            }
            if (gameObject.transform.position.x > 0)
            {
                gameObject.transform.rotation = Quaternion.AngleAxis(-45.0f, Vector3.up);
                Destroy(gameObject, 4f);
            }
        }
    }
}
