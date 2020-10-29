using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : SpawnPositions
{
    public bool PauseSpawning;
    public bool Endless_Mode;
    public GameObject UpgradeTracking;
    private GameObject UpgradeTracker;
    private GameObject spawnee;
    private GameObject Spawned;
    public List<GameObject> EnemyTypes;
    private Player_Control player;
    private int startDelay;
    private int nextWave = 0;
    public bool bForceEndlessWave = false;
    public int SetWave;
    public float GameTime;
    // waitTime is the upper limter for the timer var.
    private float waitTime = 0.02f;
    private float SpawnTimer;
    public float spawnSpeed;
    public float spawnDelay = 2.0f;
    public int enemyType;
    private float spawnOffsetX;
    private float spawnOffsetZ;
    public int EnemiesSpawnedCount;
    [HideInInspector]public bool EnableEnemyMovement;
    private Vector3 SpawnPoint;
    private string enemyName;
    private List<Vector3> spawnSwapper;
    private int pathChoice;
    private int i = 0;
    private UI_Main gamePause;
    private bool bPrecachingComplete;
    private GameObject EnemyParent;
    private Vector3 SpawneePosition;
    public int c = 0;
    void Awake()
    {
        //sets spawn delay to allow for loading
        startDelay = 5;
        // spawn the upgrade tracker prefab object into the scene at specified location.
        UpgradeTracker = (GameObject)Instantiate(UpgradeTracking, new Vector3(0, 0.15f, 0), Quaternion.identity); UpgradeTracker.AddComponent<Upgrades>();
    }
    // set the initial enemy spawn type based on the value of int enemytype.
    // sets the instantiated objects name according to the enemytype that is spawned, and sets int i back to 0 when the co-routine is started.
    private void EnemySelection()
    {
        // Set the name of the spawned object in relation to the public enemytype variable, allowing for more variations to be addedand switched in at any point.
        switch (enemyType)
        {
            case 0:
                Spawned = EnemyTypes[0]; enemyName = "EnemyFighter ";
                break;
            case 1:
                Spawned = EnemyTypes[1]; enemyName = "EnemyCorsair ";
                break;
            case 2:
                Spawned = EnemyTypes[2]; enemyName = "EnemyAttackFighter ";
                break;
            case 3:
                break;
            default:
                break;
        }
    }
    // initialization for the class
    public void Start()
    {
        gamePause = GameObject.Find("UI").GetComponent<UI_Main>();
        SpawnFormations();
        EnemySelection();
        // grab the player control script from the player in the world and assign it to the local player variable.
        player = GameObject.FindWithTag("Player").GetComponent<Player_Control>();
        EnemyParent = GameObject.Find("EnemiesParent");
        if (SetWave > 0)
        {
            nextWave = SetWave;
        }
        // Testing new spawn mechanics here
        // c represents the index of the spawn foramtion array, which is iterated only when a successful spawn occurs, otherwise incorrect spawns will happen.
        // define the index of the spawn formation to use with nextWave, this can be a random value from min to max array length,
        // TODO:: Ensure an array out of bounds can never happen -.- Don't forget this before you implement randomised wave spawning.
        // SpawnLocations index is managed by available spawn positions i.e 0 to 5, then we iterate through the array of locations until we hit a valid spawn configuration.
        // spawn configuration is defined by another array of 0 and 1 where 1 is a defined spawn and 0 denoted an empty place.
        // Doing this results in us being able to skip to the next iteration attempt without incrementing on the spawned entity index, allowing us to create many different formations.
        // All of this was theorised and prototyped to allow a designer to interact with the spawn mechanic variables and define / alter spawn configurations without having open a code editor.
        // Naming of variables might need redefining later to make the logic easier to follow.
        /*
        for (int i = 0; i < L_SpawnOffsets[nextWave].iSpawnConfiguration.Count; ++i)
        {
            if (L_SpawnOffsets[nextWave].iSpawnConfiguration[i] != 0)
            {
                SpawneePosition = GetVectorFromList(SpawnLocations[i], L_SpawnOffsets[nextWave].SpawnFormationOffsets[c]);
                spawnee = (GameObject)Instantiate(Spawned, SpawneePosition, Spawned.transform.rotation);
                SpawnVariables();
                spawnee.GetComponent<EnemyControl>().PathSelection = pathChoice;
                ++EnemiesSpawnedCount;
                spawnee.name = enemyName + EnemiesSpawnedCount;
                spawnee.transform.parent = EnemyParent.transform;
                EnableEnemyMovement = true;
                c++;
            }
        }
        */
    }
    private IEnumerator Spawning()
    {
        if (SetWave > 0)
        {
            nextWave = SetWave;
        }   
        int a;
        int waveSpawns = 1;
        // if the nextwave is 0 then spawnswapper list is set to the spawnformation1 list.
        switch (nextWave)
        {
            case 0:
                spawnSwapper = SpawnFormation1;
                pathChoice = 0;
                break;
            case 1:
                spawnSwapper = SpawnFormation2;
                pathChoice = 0;
                break;
            case 2:
                spawnSwapper = SpawnFormation5;
                pathChoice = 3;
                break;
            case 3:
                spawnSwapper = SpawnFormation3;
                pathChoice = 2;
                waveSpawns = 8;
                break;
            default:
                break;
        }
        for (a = 0; a < waveSpawns; ++a)
        {
            for (i = 0; i < (spawnSwapper.Count); ++i)
            {
                SpawnPoint = spawnSwapper[i];
                spawnee = (GameObject)Instantiate(Spawned, SpawnPoint, Spawned.transform.rotation);
                SpawnVariables();
                spawnee.GetComponent<EnemyControl>().PathSelection = pathChoice;
                ++EnemiesSpawnedCount;
                spawnee.name = enemyName + EnemiesSpawnedCount;
                spawnee.transform.parent = EnemyParent.transform;
                yield return null;
            }
            if (i == spawnSwapper.Count)
            {
                i = 0; yield return new WaitForSeconds(0.1f);
                EnableEnemyMovement = true;
                yield return new WaitForSeconds(0.1f);
                EnableEnemyMovement = false;
            }
            yield return null;
        }
        // if i equals the total length of the swapped list then wait for 0.2seconds and enable the instantiated enemy's movement before waiting for 0.1 seconds to switch it off and then stop the co-routine.
        if (a == (waveSpawns))
        {
            yield return new WaitForSeconds(0.2f);
            EnableEnemyMovement = true;
            yield return new WaitForSeconds(0.1f);
            EnableEnemyMovement = false;
            if (false == bForceEndlessWave)
            {
                nextWave++;
            }
            StopCoroutine(Spawning());
        }
    }
    void SpawnVariables()
    {
        // this method will set the modifiers of the spawnee's movement speed based on the spawn formation they belong to,
        // as well as being able to alter the delay time before they start their rotations.
        if ((spawnSwapper == SpawnFormation3) && (i == 0))
        {
            spawnee.GetComponent<EnemyControl>().PathDelay = 6.0f;
            spawnee.GetComponent<EnemyControl>().enemySpeedModifier = 0.935f;
        }
        if ((spawnSwapper == SpawnFormation3) && (i == 1))
        {
            spawnee.GetComponent<EnemyControl>().PathDelay = 6.0f;
            spawnee.GetComponent<EnemyControl>().enemySpeedModifier = 1.20f;
        }
        if (spawnSwapper == SpawnFormation5)
        {
            spawnee.GetComponent<EnemyControl>().PathDelay = 7.0f;
            spawnee.GetComponent<EnemyControl>().enemySpeedModifier = 1.0f;
        }
        if (spawnSwapper == SpawnFormation1 || spawnSwapper == SpawnFormation2)
        {
            spawnee.GetComponent<EnemyControl>().PathDelay = 8.0f;
        }
    }
    void Update()
    {
        if (false == bPrecachingComplete)
        {
            bPrecachingComplete = GameObject.Find("GameMaster").GetComponent<Pooling>().PreCachingFinished();
        }
        // if pause is false then check if player has more than 0 lives then set the spawntimer then check if the timer is greater than delay, 
        // if true then reset spawn timer and if nextwave is 0 start the spawning co-routine.
        if (gamePause.GamePause == false && false == PauseSpawning)
        {
            if ((player.PlayerLives > 0) && (bPrecachingComplete) && (EnemyParent.transform.childCount == 0))
            {
                SpawnTimer +=  spawnSpeed * Time.deltaTime;
                if (nextWave > 3) { nextWave = Mathf.RoundToInt(Random.Range(0, 3)); }
                if (SpawnTimer > spawnDelay)
                {                  
                    SpawnTimer = SpawnTimer - spawnDelay;
                    StartCoroutine(Spawning());
                }
            }
        }
        // displays the amount of time since the game was launched.
        GameTime = Mathf.RoundToInt(Time.time);
    }
}
