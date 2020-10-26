using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        FindTheComponents();
        if (gameObject.name.Contains("Fighter ")) { EnemyMoveSpeed = 4f; EnemyHealth = 4; } if (gameObject.name.Contains("Corsair ")) { EnemyMoveSpeed = 4f; EnemyHealth = 16; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawner.EnableEnemyMovement == true) { MovementEnabled = true; }
        if (EnemyHealth <= 0)
        {
            //adds 100 value to the scoreCount variable in the UI script
            ScoreSet.scoreCount += 100;
            LootDrop();
            Destroy(gameObject);
        }
        if (MovementEnabled) { Timer(); PathSelector(); }
    }
}
