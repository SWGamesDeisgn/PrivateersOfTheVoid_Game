using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : SpawnPositions
{
    [Tooltip("UpgradeBrain prefab goes here.")]
    public GameObject UpgradeTracking;
    private GameObject UpgradeTracker;
    private GameObject spawnee;
    private GameObject Spawned;
    private Player_Control player;
    [Tooltip("Entirely for testing purposes, saves having to iterate through every wave to test specific variations.")]
    public bool ForceEndlessWave = false;
    public int SetWave;
    private float GameTime;
    // waitTime is the upper limter for the timer var.
    private float SpawnTimer;
    public float InitialSpawnSpeed;
    public float spawnDelay = 2.0f;
    public int EnemiesSpawnedCount;
    [HideInInspector] public bool EnableEnemyMovement;
    private Vector3 SpawnPoint;
    private UI_Main gamePause;
    private bool bPrecachingComplete;
    private GameObject EnemyParent;
    private Vector3 SpawneePosition;

    private int SpawnerSelected = 0;
    private int WaveCount = 0;
    private int TotalSpawned = 0;
    private int SpawnsPerWave = 0;
    private bool bPauseSpawnClock = false;
    private bool bWaveComplete = false;
    void Awake()
    {
        // spawn the upgrade tracker prefab object into the scene at specified location.
        UpgradeTracker = ( GameObject )Instantiate(UpgradeTracking, new Vector3(0, 0.15f, 0), Quaternion.identity); UpgradeTracker.AddComponent<Upgrades>();
    }

    // initialization for the class
    public void Start()
    {
        gamePause = GameObject.Find("UI").GetComponent<UI_Main>();
        SpawnFormations();
        // grab the player control script from the player in the world and assign it to the local player variable.
        player = GameObject.FindWithTag("Player").GetComponent<Player_Control>();
        EnemyParent = GameObject.Find("EnemiesParent");
        if( SetWave > 0 )
        {
            WaveCount = SetWave;
        }
        GetTotalSpawnsFromWave();
    }
    void SpawnVariables()
    {
        // this method will set the modifiers of the spawnee's movement speed based on the spawn formation they belong to,
        // as well as being able to alter the delay time before they start their rotations.
        switch( WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].EnemyPathChoice )
        {
            case 0:
                spawnee.GetComponent<EnemyControl>().PathDelay = 8.0f;
                break;
            case 1:
                spawnee.GetComponent<EnemyControl>().PathDelay = 8.0f;
                break;
            case 2:
                if( SpawnerSelected % 2 == 0 )
                {
                    spawnee.GetComponent<EnemyControl>().PathDelay = 6.0f;
                    spawnee.GetComponent<EnemyControl>().enemySpeedModifier = 0.935f;
                }
                else
                {
                    spawnee.GetComponent<EnemyControl>().PathDelay = 6.0f;
                    spawnee.GetComponent<EnemyControl>().enemySpeedModifier = 1.20f;
                }
                break;
            case 3:
                spawnee.GetComponent<EnemyControl>().PathDelay = 7.0f;
                spawnee.GetComponent<EnemyControl>().enemySpeedModifier = 1.0f;
                break;

        }
    }
    void Update()
    {
        if( false == bPrecachingComplete )
        {
            bPrecachingComplete = GameObject.Find("GameMaster").GetComponent<Pooling>().PreCachingFinished();
        }
        // displays the amount of time since the game was launched.
        GameTime = Mathf.RoundToInt(Time.time);
        if( gamePause.GamePause == false )
        {
            if( ( player.PlayerLives > 0 ) && ( bPrecachingComplete ) )
            {
                // Until we hit the upper count of WaveConfig run the loop.
                if( WaveCount < WaveConfigurations.Count )
                {
                    if( false == bWaveComplete )
                    {
                        SpawnTimer += Time.deltaTime * WaveConfigurations[WaveCount].SpawningSpeed;
                    }
                    else
                    {
                        SpawnTimer += Time.deltaTime * InitialSpawnSpeed;
                    }
                    if( SpawnTimer > spawnDelay )
                    {
                        bWaveComplete = false;
                        SpawnTimer -= SpawnTimer;
                        StartCoroutine(SpawnWave());
                    }
                }
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        if( 0 == TotalSpawned )
        {
            GetTotalSpawnsFromWave();
        }
        // If the wave is designated to be an alternating sequence then the below code will execute, otherwise the else statement will trigger.
        // resulting in every spawner being activated before enemies are allowed to move, which creates a formation.
        if( true == WaveConfigurations[WaveCount].AlternateSpawners )
        {
            if( SpawnsPerWave < TotalSpawned && false == EnableEnemyMovement )
            {
                SpawnEnemies();
                EnableEnemyMovement = true;
                yield return new WaitForSeconds(0.1f);
                EnableEnemyMovement = false;
            }
        }
        else
        {
            if( SpawnsPerWave < TotalSpawned && false == EnableEnemyMovement )
            {
                foreach( EnemySpawnSettings obj in WaveConfigurations[WaveCount].SpawnSettings )
                {
                    SpawnEnemies();
                }
                EnableEnemyMovement = true;
                yield return new WaitForSeconds(0.1f);
                EnableEnemyMovement = false;
            }
        }
        if( SpawnsPerWave >= TotalSpawned )
        {
            SpawnerSelected-= SpawnerSelected;
            bWaveComplete = true;
            if( ( false == ForceEndlessWave && SpawnsPerWave >= TotalSpawned ) )
            {                        
                if( WaveCount <= WaveConfigurations.Count )
                {
                    ++WaveCount;
                }
                if( WaveCount >= WaveConfigurations.Count )
                {
                    WaveCount = Mathf.RoundToInt(Random.Range(0, WaveConfigurations.Count));
                    Debug.Log("WaveCount is: " +WaveCount);
                }            
                             
            }
            TotalSpawned -= TotalSpawned;
            SpawnsPerWave -= SpawnsPerWave;
            SpawnTimer -= SpawnTimer;
            StopCoroutine(SpawnWave());
        }
    }
    // Moved to it's own method from SpawnWave() to save resources, resulting in this being typed once instead of twice.
    private void SpawnEnemies()
    {      
        if( WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].GetSpawnCounterValue() <  WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].AmountToSpawn )
        {          
            Spawned = WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].ObjEnemy;
            SpawnPoint = WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].SpawnerLocator.gameObject.transform.position;
            spawnee = ( GameObject )Instantiate(Spawned, SpawnPoint, Spawned.transform.rotation);
            SpawnVariables();
            ++EnemiesSpawnedCount;
            spawnee.GetComponent<EnemyControl>().PathSelection = WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].EnemyPathChoice;
            spawnee.name = WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].ObjEnemy.name + " " + EnemiesSpawnedCount;
            spawnee.transform.parent = EnemyParent.transform;
            
            WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].IncrementSpawnCounter();
            ++SpawnsPerWave;
        }
        if( SpawnerSelected+1 < WaveConfigurations[WaveCount].SpawnSettings.Count )
        {
            ++SpawnerSelected;
        }
        else
        {
            SpawnerSelected-= SpawnerSelected;
        }
    }
    // Returns the reference for the player to whoever requests it. i.e. enemies being spawned.
    public Player_Control GetPlayerRef()
    {
        return player;
    }
    // Tally up the amount of enemies which are to be spawned per wave setting TotalSpawned to the sum of the total of the amount to spawn for each spawner in a given wave.
    private void GetTotalSpawnsFromWave()
    {
        if( 0 == TotalSpawned )
        {            
            foreach( EnemySpawnSettings obj in WaveConfigurations[WaveCount].SpawnSettings )
            {
                if( 0 != WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].AmountToSpawn )
                {
                    TotalSpawned += WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].AmountToSpawn;                    
                }                    
                WaveConfigurations[WaveCount].SpawnSettings[SpawnerSelected].ResetSpawnCounter();
                ++SpawnerSelected;
            }
            // Resetting the SpawnCounters here because it saves resources, meaning we don't have to create another foreach loop and iterate through the spawners.
            // Spawn counter should be zero at the start of a wave anyway.
            SpawnerSelected -= SpawnerSelected;
        }
    }
}
