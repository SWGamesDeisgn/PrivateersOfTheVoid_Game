using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnLocationOffsets
{
    public List<Vector3> SpawnFormationOffsets;
    public List<int> iSpawnConfiguration;
}

public class SpawnPositions : MonoBehaviour
{
    public List<GameObject> SpawnLocations;
    public List<SpawnLocationOffsets> L_SpawnOffsets;
    public List<Vector3> SpawnFormation1;
    public List<Vector3> SpawnFormation2;
    public List<Vector3> SpawnFormation3;
    public List<Vector3> SpawnFormation4;
    public List<Vector3> SpawnFormation5;
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
