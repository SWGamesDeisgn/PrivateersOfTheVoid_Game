using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Weapons_V2 : MonoBehaviour {

    public bool Pause;
    public bool FireTheGuns = false;
    public bool FireBeamLaser = false;
    public float MissleFiringSpeed = 1.75f;
	public int MissleDamage = 1;
	public GameObject Missle;
	public Transform MissleLoc;
	public int MissleSpeed = 6;
	private float nextShot = 1.0f;
	private GameObject missle;
	private GameObject missle1;
	public Player_Control WeaponsUnlocked;
	public GameObject WeaponSystems;
	public GameObject BeamLaserObj;
    private bool MissleLock = false;
    // waitTime is the upper limter for the timer var.
    private float timer;
    private float waitTime = 0.02f;
    private UI_Main gamePause;
    public Pooling AmmoPool;
    private bool bReadyToFire = true;


    void Start ()
	{
        // Get the UI_Main script of the UI game object and assign it to the gamePause local variable.
        gamePause = GameObject.Find("UI").GetComponent<UI_Main>();
        MissleLock = false;
        // Get the Player_Control script from the player in world and assign it to the WeaponsUnlocked local variable,
        // This is to track if the player has any specific weapon upgrades, and how many of them.
		WeaponsUnlocked = WeaponSystems.GetComponent<Player_Control>();
        AmmoPool = GameObject.Find("GameMaster").GetComponent<Pooling>();
    }
void FixedUpdate()
    {
        // prevent the weapons from functioning if the game is paused using the UI_Main game pause statement using time.timescale.
        if (gamePause.GamePause == false)
        {
            if (WeaponsUnlocked.PlayerLives < 1)
            {
                Pause = true;
            }
            if (Pause == false)
            {
                if (WeaponsUnlocked.Bordertop < 2)
                {
                    if (timer <= 1.0f)
                    {
                        timer += MissleFiringSpeed * Time.deltaTime;
                    }          
                    //I am a clock.
                    // when timer is greater than nextShot run the code which then checks if the player has specific upgrades unlocked.
                    //timer += Time.fixedDeltaTime * MissleFiringSpeed;
                    if (((Input.GetKey(KeyCode.Space) || (FireTheGuns == true)) && (timer >= 1.0f)) && (MissleLock == false))
                    {
                        timer -= timer;
                        Fire();
                        if (WeaponsUnlocked.multishot < (WeaponsUnlocked.MaxMultiShot))
                        {
                            Fire1();
                        }
                        if (WeaponsUnlocked.multishot < (WeaponsUnlocked.MaxMultiShot - 1))
                        {
                            Fire2();
                        }
                    }
                    // checks if the beamlaser upgrade has been obtained then turns MissleLock to true, disabling the player's Missle firing mechanics
                    // then it will activate the BeamLaserObject which instantly fires the particle emitter along with the two raycasts for the weapon type.

                }
                if (WeaponsUnlocked.BeamLaser == true || FireBeamLaser == true)
                {
                    MissleLock = true;
                    BeamLaserObj.SetActive(true);
                }
                // if the beamlaser upgrade isn't obtained then the BeamLaserObject is set to inactive and MissleLock is set to false.
                // meaning the player's primary weapons will be enabled.
                else
                {
                    BeamLaserObj.SetActive(false);
                    MissleLock = false;
                }
            }
        }
	}
    // instantiates the Missle projectiles at specified locations relative to the origin position of MissleLoc, then provides a quaternion rotation.
    // also sets the velocity of the rigidbodies to be multiplied by the MissleSpeed integer
    // and finally adds a destroy component with a delay of 2 seconds. though that is not strictly required for standard gameplay, 
    // it's more of an insurance that there won't be millions of projectiles if the MissleSpeed int is set below 6.
	void Fire()
	{
        switch (null != AmmoPool)
        {
            case true:
                missle = AmmoPool.GetFromPool("PlayerMissile");
                if (null != missle)
                {
                    missle.transform.position = (MissleLoc.position + new Vector3(-0.075f, 0, 0.5f));
                    missle.transform.rotation = (Quaternion.identity);
                    missle.SetActive(true);
                    missle.GetComponent<Rigidbody>().velocity = missle.transform.forward * MissleSpeed;
                    missle.GetComponent<RaycastEmitter>().SetOwner("Player");
                }

                missle1 = AmmoPool.GetFromPool("PlayerMissile");
                if (null != missle1)
                {
                    missle1.transform.position = (MissleLoc.position + new Vector3(0.075f, 0, 0.5f));
                    missle1.transform.rotation = (Quaternion.identity);
                    missle1.SetActive(true);
                    missle1.GetComponent<Rigidbody>().velocity = missle1.transform.forward * MissleSpeed;
                    missle1.GetComponent<RaycastEmitter>().SetOwner("Player");
                }
                break;
            case false:               
                break;
            default:
                break;
        }

        /*
        projm = (GameObject)Instantiate (Missle, MissleLoc.position + new Vector3(-0.075f, 0, 0.5f), Quaternion.identity);
        projm.transform.SetParent(GameObject.Find("PlayerProjectileKeeper").transform, false); projm.GetComponent<Rigidbody>().velocity = projm.transform.forward * MissleSpeed;
		Destroy (projm, 2f);

		projm = (GameObject)Instantiate (Missle, MissleLoc.position + new Vector3(0.075f, 0, 0.5f), Quaternion.identity);
        projm.transform.SetParent(GameObject.Find("PlayerProjectileKeeper").transform, false); projm.GetComponent<Rigidbody>().velocity = projm.transform.forward * MissleSpeed;
		Destroy (projm, 2f);
        */
	}

	void Fire1()
	{
        switch (null != AmmoPool)
        {
            case true:
                missle = AmmoPool.GetFromPool("PlayerMissile");
                if (null != missle)
                {
                    missle.transform.position = (MissleLoc.position + new Vector3(0.15f, 0, 0.5f));
                    missle.transform.rotation = (MissleLoc.rotation * Quaternion.AngleAxis(3.0f, Vector3.up));
                    missle.SetActive(true);
                    missle.GetComponent<Rigidbody>().velocity = missle.transform.forward * MissleSpeed;
                    missle.GetComponent<RaycastEmitter>().SetOwner("Player");
                }

                missle1 = AmmoPool.GetFromPool("PlayerMissile");
                if (null != missle1)
                {
                    missle1.transform.position = (MissleLoc.position + new Vector3(-0.15f, 0, 0.5f));
                    missle1.transform.rotation = (MissleLoc.rotation * Quaternion.AngleAxis(-3.0f, Vector3.up));
                    missle1.SetActive(true);
                    missle1.GetComponent<Rigidbody>().velocity = missle1.transform.forward * MissleSpeed;
                    missle1.GetComponent<RaycastEmitter>().SetOwner("Player");
                }
                break;
            case false:
                break;
            default:
                break;
        }

        /*
        projm = (GameObject)Instantiate (Missle, MissleLoc.position + new Vector3(0.15f, 0, 0.5f), MissleLoc.rotation * Quaternion.AngleAxis(3.0f, Vector3.up));
        projm.transform.SetParent(GameObject.Find("PlayerProjectileKeeper").transform, false); projm.GetComponent<Rigidbody>().velocity = projm.transform.forward * MissleSpeed;
	  	Destroy (projm, 2f);

		projm = (GameObject)Instantiate (Missle, MissleLoc.position + new Vector3(-0.15f, 0, 0.5f), MissleLoc.rotation * Quaternion.AngleAxis(-3.0f, Vector3.up));
        projm.transform.SetParent(GameObject.Find("PlayerProjectileKeeper").transform, false); projm.GetComponent<Rigidbody>().velocity = projm.transform.forward * MissleSpeed;
		Destroy (projm, 2f);
        */
	}

	void Fire2()
	{
        switch (null != AmmoPool)
        {
            case true:
                missle = AmmoPool.GetFromPool("PlayerMissile");
                if (null != missle)
                {
                    missle.transform.position = (MissleLoc.position + new Vector3(-0.25f, 0, 0.5f));
                    missle.transform.rotation = (MissleLoc.rotation * Quaternion.AngleAxis(-4.0f, Vector3.up));
                    missle.SetActive(true);
                    missle.GetComponent<Rigidbody>().velocity = missle.transform.forward * MissleSpeed;
                    missle.GetComponent<RaycastEmitter>().SetOwner("Player");
                }

                missle1 = AmmoPool.GetFromPool("PlayerMissile");
                if (null != missle1)
                {
                    missle1.transform.position = (MissleLoc.position + new Vector3(0.25f, 0, 0.5f));
                    missle1.transform.rotation = (MissleLoc.rotation * Quaternion.AngleAxis(4.0f, Vector3.up));
                    missle1.SetActive(true);
                    missle1.GetComponent<Rigidbody>().velocity = missle1.transform.forward * MissleSpeed;
                    missle1.GetComponent<RaycastEmitter>().SetOwner("Player");
                }
                break;
            case false:
                break;
            default:
                break;
        }

        /*
        projm = (GameObject)Instantiate (Missle, MissleLoc.position + new Vector3 (-0.25f, 0, 0.5f), MissleLoc.rotation * Quaternion.AngleAxis (-4.0f, Vector3.up));
        projm.transform.SetParent(GameObject.Find("PlayerProjectileKeeper").transform, false); projm.GetComponent<Rigidbody>().velocity = projm.transform.forward * MissleSpeed;
		Destroy (projm, 2f);

		projm = (GameObject)Instantiate (Missle, MissleLoc.position + new Vector3(0.25f, 0, 0.5f), MissleLoc.rotation * Quaternion.AngleAxis (4.0f, Vector3.up));
        projm.transform.SetParent(GameObject.Find("PlayerProjectileKeeper").transform, false); projm.GetComponent<Rigidbody>().velocity = projm.transform.forward * MissleSpeed;
		Destroy (projm, 2f);
        */
	}
}