using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public bool Pause;
    private bool TrackerTrue;
    public bool Endless_Mode;
    private bool spawnPause;
    public GameObject UpgradeTracking;
    private GameObject UpgradeTracker;
    private GameObject spawnee;
	public GameObject Spawned;
    public GameObject EnemyType1;
    public GameObject EnemyType2;
    public Player_Control player;
    public int startDelay;
	private int nextWave = 0;
	public float GameTime;
    // waitTime is the upper limter for the timer var.
    public float waitTime = 0.02f;
    private float SpawnTimer;
    public float spawnSpeed;
    public float spawnDelay;
    public int enemyType;

    void Awake ()
	{
        //sets spawn delay to allow for loading
		startDelay = 10;
        spawnSpeed = 1.0f;
	}
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player_Control>();
    }
    void Update () 
	{
        //I am a clock, and I set the visual time of the timer to two decimal places 
        //and removes the deltatime from the visual time because it sometimes adds a 0.02 as a constant value to the result.
        if (Pause == false)
        {
            if (player.PlayerLives > 0)
            {
                SpawnTimer += Time.fixedDeltaTime * spawnSpeed;
                if (enemyType == 0) { Spawned = EnemyType1; }
                if (enemyType == 1) { Spawned = EnemyType2; }
                // timer += Time.fixedDeltaTime;
                if ((startDelay > 0) && (SpawnTimer > 1))
                {
                    if (SpawnTimer > 1) { startDelay -= 1; SpawnTimer = SpawnTimer - 1; }
                }

                if (SpawnTimer > spawnDelay)
                {
                    // VisualTime = Mathf.RoundToInt(timer *100.0f) /100.0f - Time.fixedDeltaTime;
                    // timer = timer - waitTime;
                    SpawnTimer = SpawnTimer - spawnDelay;
                    if (startDelay == 0)
                    {
                        spawnSpeed = 0.5f;
                        SpawnTimer = SpawnTimer - waitTime;
                        // Standard Mode
                        if (Endless_Mode == false)
                        {
                            if ((nextWave == 0) && (spawnPause == false))
                            {
                                Wave1();
                            }
                            if ((nextWave == 1) && (spawnPause == false))
                            {
                                Wave2();
                            }
                            spawnPause = false;
                        }
                        // Endless Mode
                        if (Endless_Mode == true)
                        {
                            Wave1();
                        }

                    }
                }
            }
        }
        // spawn the upgrade tracker
        GameTime = Mathf.RoundToInt(Time.time);
		if (TrackerTrue == false) 
		{
			UpgradeTrack ();
		}
	}
    // spawn wave methods which also state which wave will spawn next after the current wave has spawned, e.g. Wave1 will spawn Wave2 in sequence controlled by the "NextWave" integer.
	public void Wave1 ()
	{
		StartCoroutine (EnemySpawnVar1 ());
		nextWave = 1;
        spawnPause = true;
	}

	public void Wave2 ()
	{
		StartCoroutine (EnemySpawnVar2 ());
		nextWave = 2;
		spawnPause = true;

	}
    // the EnemySpawnVar# Ienums are simply the possible spawn patterns for enemies, the actual locations are controlled in the "Spawn" methods below.
	IEnumerator EnemySpawnVar1 ()
	{
		Spawn ();
		yield return new WaitForSeconds (0.5f);
		Spawn1 ();
		Spawn2 ();
		yield return new WaitForSeconds (0.5f);
		Spawn3 ();
		Spawn4 ();
	}

	IEnumerator EnemySpawnVar2 ()
	{
		Spawn7 ();
		Spawn8 ();
		yield return new WaitForSeconds (5.0f);
		Spawn ();
		yield return new WaitForSeconds (5.0f);
		spawnPause = false;
	}

	void UpgradeTrack()
	{
		UpgradeTracker = (GameObject)Instantiate (UpgradeTracking, new Vector3(0,0.15f,0), Quaternion.identity);
		UpgradeTracker.AddComponent <Upgrades> ();
		TrackerTrue = true;
	}	
    // "Spawn#" set the spawn positions for the various spawn possibilities using the enemySpawner.position + new Vector3s, as well as their rotations using Quaternions.
    // also makes sure the spawnee's rigidbodies don't use gravity otherwise things will break.
	void Spawn()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(0,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn1()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(0.5f,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn2()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(-0.5f,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn3()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(1,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn4()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(-1,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn5()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(-1.5f,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn6()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(1.5f,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn7()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3 (2, 0, -0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn8()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3 (-2, 0, -0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn9()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(2.5f,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}
	void Spawn10()
	{
		spawnee = (GameObject)Instantiate (Spawned, gameObject.transform.position + new Vector3(-2.5f,0,-0.5f), Quaternion.identity);
		spawnee.GetComponent<Rigidbody> ().useGravity = false;
	}

}