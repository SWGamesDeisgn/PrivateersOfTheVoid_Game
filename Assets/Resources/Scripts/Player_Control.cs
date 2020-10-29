using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    public int playerHealth;
    public int currentHealth;
    private int Borderleft;
    private int Borderright;
    private int Borderbottom;
    [HideInInspector] public int Bordertop = 0;
    public bool UpgradePlayerLife = false;
    //-------------------------Player Life Integers---------------------------------------//
    // c = total lives remaining
    public int c;
    //-------------------------Player Upgrade Variables-----------------------------------//
    public int MaxPlayerLives;
    public int PlayerLives;
    public int MaxMultiShot = 4;
    [HideInInspector] public int multishot;
    public bool BeamLaser;
    public bool PlayerDamageImmune;
    private bool PlayerBrain;
    //-------------------------Player Object Components-----------------------------------//
    [HideInInspector] public GameObject PlayerT1;
    [HideInInspector] public GameObject PlayerT2;
    [HideInInspector] public GameObject PlayerT2Missles;
    [HideInInspector] public GameObject PlayerT2Body;
    public GameObject PlayerLivesMasterObj;
    public GameObject PlayerLivesSpawnedOj;
    private GameObject playerLife;
    private Vector3 PreviousLifeLost;
    bool InitialSpawn = false;
    private Player_Weapons_V2 weaponsSystems;
    //-------------------------Game Operation Components----------------------------------//
    [HideInInspector] public GameObject UpgradeDetection;
    [HideInInspector] public Upgrades UpgradeDetect;
    [HideInInspector] public JoystickControls JoystickInput;
    public UI_Main UI_Main;
    public GameObject deathScreenObj;
    private bool bInitCompleted = false;

    private void Awake()
    {
        currentHealth = playerHealth;
    }
    void Start()
    {
        multishot = MaxMultiShot;
        weaponsSystems = GameObject.Find("PlayerWeaponSpawner").GetComponent<Player_Weapons_V2>();
        UI_Main = GameObject.Find("UI").GetComponent<UI_Main>();
        PlayerLives = 3;
        PlayerLivesMasterObj = GameObject.Find("PlayerLives");
        StartCoroutine(PlayerLifeGenerator());
        PlayerBrain = false;
        BeamLaser = false;
    }

    // Coroutine for the players "damage flicker" effect, triggers 3 times per trigger then turns immunity off after 0.2 seconds time.
    private IEnumerator PlayerDamaged()
    {
        int i = 0;
        for (i = 0; i < 3; ++i)
        {
            // essentially, does the player have the tier 2 player model.
            if (multishot < MaxMultiShot)
            {
                PlayerT2Missles.SetActive(false);
                PlayerT2Body.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                PlayerT2Missles.SetActive(true);
                PlayerT2Body.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                if (PlayerLives < 1) { if (MaxMultiShot < 2) { PlayerT2Missles.SetActive(false); PlayerT2Body.SetActive(false); } }
                yield return null;
            }
            if (multishot == MaxMultiShot)
            {
                PlayerT1.GetComponent<MeshRenderer>().enabled = false;
                yield return new WaitForSeconds(0.2f);
                PlayerT1.GetComponent<MeshRenderer>().enabled = true;
                yield return new WaitForSeconds(0.2f);
                if (PlayerLives < 1) { if (multishot == 2) { PlayerT1.GetComponent<MeshRenderer>().enabled = false; } }
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.2f);
        PlayerDamageImmune = false;
        StopCoroutine(PlayerDamaged());
    }

    // this manages how quickly the player uypgrades are applied, this is typically only needed for the missles upgrade since there can be multiple on the screen at one time, 
    // meaning they might stack up more than intended in the variables if the wait for seconds was not in place to delay how fast the upgrade can be applied.
    private IEnumerator UpgradeDelay()
    {
        yield return new WaitForSeconds(0.2f);
        multishot -= 1;
        if (multishot >= 2 && weaponsSystems.MissleFiringSpeed < 4.0f) { weaponsSystems.MissleFiringSpeed += 0.25f; }
        print("Upgrade Gained!");
        StopCoroutine(UpgradeDelay());
    }
    // The PlayerLifeGenerator is designed to generate 3 lives at the start of the game by instantiated a set amount of pixels apart from the previous life object.
    private IEnumerator PlayerLifeGenerator()
    {
        Vector3 vectorOffset = new Vector3(-60.0f, 0.0f, 0.0f);
        // If the player hasn't obtained a life upgrade run the statement.
        if (UpgradePlayerLife == false)
        {
            // if the inital starting lives haven't been generated.
            if (InitialSpawn == false)
            {
                for (int i = 0; i < ((MaxPlayerLives / 2) - ((MaxPlayerLives / 4)) / 2); ++i)
                {
                    {
                        if (i == 0)
                        {
                            playerLife = (GameObject)Instantiate(PlayerLivesSpawnedOj, PlayerLivesMasterObj.transform.position, Quaternion.identity);
                            playerLife.transform.SetParent(PlayerLivesMasterObj.transform, false); ++c; playerLife.name = "Player Life " + (c); InitialSpawn = true;

                        }
                        if (i > 0)
                        {
                            playerLife = (GameObject)Instantiate(PlayerLivesSpawnedOj, GameObject.Find("Player Life " + (c)).transform.localPosition + vectorOffset, Quaternion.identity);
                            playerLife.transform.SetParent(PlayerLivesMasterObj.transform, false); ++c; playerLife.name = "Player Life " + (c); InitialSpawn = true;
                        }
                        yield return null;
                    }
                }
            }
        }
        // If the player has collided with a life upgrade add one life X distance from the previous life object until total of 4
        // Then move down Y amount and restart from initial location.
        if (UpgradePlayerLife == true)
        {
            float LifeOffsetY = 0.0f;
            float LifeOffsetX = -60.0f;
            if (c == 4) { LifeOffsetY = -60.0f; LifeOffsetX = 180.0f; }
            else if (c >= 5) { LifeOffsetY = 0.0f; LifeOffsetX = -60.0f; }
            for (int ii = 0; ii < 1; ++ii)
            {
                playerLife = (GameObject)Instantiate(PlayerLivesSpawnedOj, GameObject.Find("Player Life " + (c)).transform.localPosition + new Vector3(LifeOffsetX, LifeOffsetY, 0.0f), Quaternion.identity);
                playerLife.transform.SetParent(PlayerLivesMasterObj.transform, false); ++c; playerLife.name = "Player Life " + (c);
                PlayerLives++; UpgradePlayerLife = false; UpgradePlayerLife = true;
            }
            yield return null;
        }
    }
    void FixedUpdate()
    {
        if (false == bInitCompleted)
        {
            ObjectInit();
        }
        // If the player has died activate the deathScreenObj, set the player to inactive, and print to console ( this will be migrated to the death screen soon.)
        if (c == 0) { deathScreenObj.SetActive(true); gameObject.SetActive(false); print("Game Over! Your Score is: " + UI_Main.scoreCount); }
        if ((currentHealth == 0) && (PlayerLives > 0))
        {
            if (c == 1) { PreviousLifeLost = GameObject.Find("Player Life " + (c)).transform.position; }
            else if (c > 1) { PreviousLifeLost = GameObject.Find("Player Life " + (c - 1)).transform.position; }
            GameObject.Destroy(GameObject.Find("Player Life " + c));
            print("Player Died"); if (c > 1) { currentHealth = playerHealth; }
            --c; PlayerLives -= 1;
        }
        // finds the brain for upgrade management.
        if (PlayerDamageImmune == false) { StopCoroutine(PlayerDamaged()); }
        // if the first level of missle upgrades have been obtained then the tier 2 model of the player obj is set to active and the tier 1 model is deactivated.
        if (multishot <= MaxMultiShot / 2) { PlayerT2.SetActive(true); PlayerT1.SetActive(false); }

        // TOUCHSCREEN CONTROLS 
        // obtained from the joystickControls script on the Joystick UI Object.
        if ((Borderleft < 2) && (JoystickInput.inputVector.x < 0))
        {
            transform.Translate(Vector3.left * Time.deltaTime * (-JoystickInput.inputVector.x * 2) * 2);
        }
        if ((Borderright < 2) && (JoystickInput.inputVector.x > 0))
        {
            transform.Translate(Vector3.right * Time.deltaTime * (JoystickInput.inputVector.x * 2) * 2);
        }
        if ((Borderbottom < 2) && (JoystickInput.inputVector.y < 0))
        {
            transform.Translate(Vector3.back * Time.deltaTime * (-JoystickInput.inputVector.y * 2) * 2);
        }
        if ((Bordertop < 2) && (JoystickInput.inputVector.y > 0))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * (JoystickInput.inputVector.y * 2) * 2);
        }
        // KEYBOARD CONTROLS

        //moving up
        if (Bordertop < 2)
        {
            if (Input.GetKey(KeyCode.W)) { transform.Translate(Vector3.forward * Time.deltaTime * 4); }
        }
        //moving left
        if (Borderleft < 2)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.deltaTime * 4);
                if ((Input.GetKey(KeyCode.W)) && (Bordertop < 2)) { transform.Translate(Vector3.back * Time.deltaTime * 2); }
                if ((Input.GetKey(KeyCode.S)) && (Borderbottom < 2)) { transform.Translate(Vector3.forward * Time.deltaTime * 2); }
            }
        }
        //moving right
        if (Borderright < 2)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.deltaTime * 4);
                if ((Input.GetKey(KeyCode.W)) && (Bordertop < 2)) { transform.Translate(Vector3.back * Time.deltaTime * 2); }
                if ((Input.GetKey(KeyCode.S)) && (Borderbottom < 2)) { transform.Translate(Vector3.forward * Time.deltaTime * 2); }
            }
        }
        //moving down
        if (Borderbottom < 2)
        {
            if (Input.GetKey(KeyCode.S)) { transform.Translate(Vector3.back * Time.deltaTime * 4); }
        }
    }
    // This controls how quickly the "BeamLaser" weapon type is toggled on for before switching it off. change the WaitForSeconds integer to alter the duration.
    private IEnumerator BeamToggle()
    {
        yield return new WaitForSeconds(4);
        UpgradeDetect.BeamLaserActive = false;
        BeamLaser = false;
        StopCoroutine(BeamToggle());
    }

    // various ontrigger detection statements. Mostly to handle the border detection of the game,
    // also detects when the player picks an upgrade up.
    void OnTriggerEnter(Collider collision)
    {
        /*
        if (collision.gameObject.CompareTag("BorderLeft")) { Borderleft = 2; }
        if (collision.gameObject.CompareTag("BorderRight")) { Borderright = 2; }
        if (collision.gameObject.CompareTag("BorderTop")) { Bordertop = 2; }
        if (collision.gameObject.CompareTag("BorderBottom")) { Borderbottom = 2; }
        */
        if (collision.gameObject.name == "BorderLeft") { Borderleft = 2; }
        if (collision.gameObject.name == "BorderRight") { Borderright = 2; }
        if (collision.gameObject.name == "BorderTop") { Bordertop = 2; }
        if (collision.gameObject.name == "BorderBottom") { Borderbottom = 2; }
        // if the player isn't immune to damage, PlayerDamageImmune is true, start the PlayerDamaged coroutine, print player has been hit.
        // Then reduce player's current health by 1.
        if (PlayerDamageImmune == false)
        {
            if (collision.gameObject.CompareTag("Enemies") )
            {
                DamagePlayer(1);
            }
        }
        if (collision.gameObject.tag == "PlayerLifeUpgrade")
        {
            if (c < MaxPlayerLives)
            {
                UpgradeDetect.UpgradeLifeCounter = 0;
                UpgradePlayerLife = true; StartCoroutine(PlayerLifeGenerator());
            }
        }
        // Missles upgrade
        if ((collision.gameObject.tag == "TripleShotUpgrade") && (MaxMultiShot > 0)) { StartCoroutine(UpgradeDelay()); }
        // Beamlaser upgrade
        if (collision.gameObject.tag == "UpgradeBeamLaser" && (BeamLaser == false))
        { BeamLaser = true; StartCoroutine(BeamToggle()); }
    }
    // has the player moved away from the colliding object?
    void OnTriggerExit(Collider collision)
    {
        /*
        if (collision.gameObject.CompareTag("BorderLeft")) { Borderleft = 0; }
        if (collision.gameObject.CompareTag("BorderRight")) { Borderright = 0; }
        if (collision.gameObject.CompareTag("BorderTop")) { Bordertop = 0; }
        if (collision.gameObject.CompareTag("BorderBottom")) { Borderbottom = 0; }
        */
        if (collision.gameObject.name == "BorderLeft") { Borderleft = 0; }
        if (collision.gameObject.name == "BorderRight") { Borderright = 0; }
        if (collision.gameObject.name == "BorderTop") { Bordertop = 0; }
        if (collision.gameObject.name == "BorderBottom") { Borderbottom = 0; }
    }

    private void ObjectInit()
    {
        UpgradeDetection = GameObject.FindWithTag("UpgradeBrain");
        UpgradeDetect = UpgradeDetection.GetComponent<Upgrades>();
        bInitCompleted = true;
        //because knowing if you have your brain is important, specially when using a coroutine.
        if (null != UpgradeDetect)
        {
            print("Player 1 has found their brain!");
        }
        else { print(" ERROR:: Player 1 has NOT found their brain!"); }
        
    }
    public void DamagePlayer(int iDmg)
    {
        // One instance of damage per cycle please.
        // Anything greater than that would be inhumane.
        if (!PlayerDamageImmune)
        {
            StartCoroutine(PlayerDamaged());
            if (currentHealth > iDmg)
            {
                currentHealth -= iDmg;
            }
            else
            {
                currentHealth = 0;
            }

            PlayerDamageImmune = true;
            print("player has been hit");
        }
    }
}