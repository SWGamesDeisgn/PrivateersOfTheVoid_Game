using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnSettings
{
    public GameObject SpawnerLocator;
    public GameObject ObjEnemy;
    public int AmountToSpawn;
    public int EnemyPathChoice;
    private int EnemiesSpawned = 0;
    public void IncrementSpawnCounter() { if( AmountToSpawn  > 0 ) { ++EnemiesSpawned; } }
    public int GetSpawnCounterValue() { return EnemiesSpawned; }
    public void ResetSpawnCounter() { EnemiesSpawned-= EnemiesSpawned; }
}

[System.Serializable]
public class WaveConfig
{
    [Tooltip("Give the wave a name it make it easier to talk about.")]
    public string WaveName;
    [Tooltip("How fast the spawners will cycle, i.e. value of 10 will result in spawns being very close together, like a snake.")]
    public float SpawningSpeed;
    [Tooltip("Alternate Spawning between all spawners in the wave if false each spawner will spawn once per cycle.")] public bool AlternateSpawners;
    public List<EnemySpawnSettings> SpawnSettings;
}

// Everything below this is now obsolete :D I achieved my goal of making wave configurations via editor view only! Woo!!!
public class SpawnPositions : MonoBehaviour
{
    public List<WaveConfig> WaveConfigurations;
    [HideInInspector] public List<Vector3> SpawnFormation1;
    [HideInInspector] public List<Vector3> SpawnFormation2;
    [HideInInspector] public List<Vector3> SpawnFormation3;
    [HideInInspector] public List<Vector3> SpawnFormation4;
    [HideInInspector] public List<Vector3> SpawnFormation5;
    private Vector3 StartingVector = new Vector3(2.25f, 0.0f, 0.0f);
    private Vector3 AdjustmentVector;
    // TODO:: Make these spawn locations configurable in editor rather than in code maybe?
    // Would require an array of game objects then get the vector locations for those objects
    // Maybe make the formations be creatable in editor?


    public Vector3 GetVectorFromList(GameObject InputLocation, Vector3 InputOffset)
    {
        Vector3 OutputVector = new Vector3();
        OutputVector = InputLocation.transform.position + InputOffset;
        return OutputVector;
    }
    public void SpawnFormations()
    {
        /*    Old SpawnPoints List --- Might be removed --- or kept as a Master list of the possible spawn points.
            SpawnPoints = new List<Vector3>
            {
                (gameObject.transform.position - StartingVector + new Vector3(0.75f,0, 0.0f)),
                (gameObject.transform.position - StartingVector + new Vector3(1.5f,0, 0.0f)),
                (gameObject.transform.position - StartingVector + new Vector3(2.25f,0, 0.0f)),
                (gameObject.transform.position - StartingVector + new Vector3(3.0f,0, 0.0f)),
                (gameObject.transform.position - StartingVector + new Vector3(3.75f,0, 0.0f)),
                (gameObject.transform.position - StartingVector + new Vector3(4.5f,0, 0.0f)),
                (gameObject.transform.position - StartingVector + new Vector3(5.25f,0, 0.0f))
          };
        */
        // Spawn Formation 1 list holds the vector information for the instantiated objects, This formation is that of a "V". 
        SpawnFormation1 = new List<Vector3>
        {
            (gameObject.transform.position - StartingVector + new Vector3(0.75f,0, 0.0f)),
            (gameObject.transform.position - StartingVector + new Vector3(1.5f,0, -0.75f)),
            (gameObject.transform.position - StartingVector + new Vector3(2.25f,0, -1.5f)),
            (gameObject.transform.position - StartingVector + new Vector3(3.0f,0, -0.75f)),
            (gameObject.transform.position - StartingVector + new Vector3(3.75f,0, 0.0f))
        };
        // enemies spawnm at the center with a gap in the middle. This formation is that of a "V" minus the center. 
        SpawnFormation2 = new List<Vector3>
        {
            (gameObject.transform.position - StartingVector + new Vector3(0.75f,0, 0.0f)),
            (gameObject.transform.position - StartingVector + new Vector3(1.5f,0, -0.75f)),
            (gameObject.transform.position - StartingVector + new Vector3(3.0f,0, -0.75f)),
            (gameObject.transform.position - StartingVector + new Vector3(3.75f,0, 0.0f))
        };
        // enemies spawn at far top right of the screen. This formation is that of a "//". 
        SpawnFormation3 = new List<Vector3>
        {
            (gameObject.transform.position - StartingVector + new Vector3(3.75f,0, 0.0f)),
            (gameObject.transform.position - StartingVector + new Vector3(4.5f,0, 0.0f))
        };
        // Inverse location of formation 3.
        SpawnFormation4 = new List<Vector3>
        {
            (gameObject.transform.position - StartingVector + new Vector3(-3.75f,0, 0.0f)),
            (gameObject.transform.position - StartingVector + new Vector3(-4.5f,0, 0.0f))
        };
        //  A---A Formation. --- are blank spaces.
        SpawnFormation5 = new List<Vector3>
        {
            (gameObject.transform.position - StartingVector + new Vector3(0.75f,0, 0.0f)),
            (gameObject.transform.position - StartingVector + new Vector3(1.5f,0, -0.75f)),
            (gameObject.transform.position - StartingVector + new Vector3(3.0f,0, -0.75f)),
            (gameObject.transform.position - StartingVector + new Vector3(3.75f,0, 0.0f))
        };
    }
}
