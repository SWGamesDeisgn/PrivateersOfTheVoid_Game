using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastEmitter : MonoBehaviour {
	
	public bool Paused;
	public float BeamLength;
	public GameObject targetHit;
	public float DamageTicker;
	public int Damage;
	private float plasmaDPS;
    public string strOwningObject;
    public bool bUseCollision = false;

	// Use this for initialization
	void Start ()
    {
        // sets the variables based on the tags of the objects this script is assigned to.
        if (gameObject.CompareTag("PlayerBeamLaser"))
        {
            plasmaDPS = 0.5f;
            Damage = 2;
        }
        if (gameObject.CompareTag("Missles"))
        {
            Damage = 1;
            BeamLength = 0.25f;
        }
    }
	// Update is called once per frame
	void Update () 
	{
		if (Paused == false)
		{
            // if gameobject's tag is PlayerBeamLaser create a new raycast and draw a line for it.
			if (gameObject.CompareTag( "PlayerBeamLaser"))
				{
                Ray Ray = new Ray(transform.position + new Vector3(0, 0, 0.35f), Vector3.forward * BeamLength);
                Debug.DrawRay (transform.position + new Vector3(0,0,0.35f), Vector3.forward * BeamLength);
                // if raycast hits a gameobject tagged with Enemies then deal Damage everytime the timer is greater than DamageTicker.
					if ((Physics.Raycast (Ray, out RaycastHit Hit, BeamLength) && (Hit.collider.tag == "Enemies")))
					{
						targetHit = Hit.collider.gameObject;
						if ((Time.time > DamageTicker) && (targetHit.GetComponent<EnemyControl> ().EnemyHealth > 0))
						{
							DamageTicker = Time.time + plasmaDPS;
							targetHit.GetComponent<EnemyControl> ().EnemyHealth -= Damage;
						}
					} 
                    // if the raycast doesn't hit anytihng tagged with "Enemies" then DamageTicekrs = 0 and the targethit is null.
					else 
					{
						DamageTicker = 0;
						targetHit = null;
					}
				}
            if (gameObject.CompareTag("Missles"))
            {
                if (false == bUseCollision)
                {
                    // if gameobject's tag is PlayerMissles create a new raycast and draw a line for it.
                    // two raycasts for LHS and RHS of the missle, instead of one in the middle, to enable increased hit accuracy.
                    Ray Ray1 = new Ray(transform.position + new Vector3(0.014f, 0, 0f), gameObject.transform.forward * BeamLength);
                    Ray Ray2 = new Ray(transform.position + new Vector3(-0.014f, 0, 0f), gameObject.transform.forward * BeamLength);
                    Debug.DrawRay(transform.position + new Vector3(0.014f, 0, 0f), gameObject.transform.forward * BeamLength);
                    Debug.DrawRay(transform.position + new Vector3(-0.014f, 0, 0f), gameObject.transform.forward * BeamLength);
                    // if the raycast hits the projectile destroyer objects then the missle is destroyed.
                    if ((Physics.Raycast(Ray1, out RaycastHit Hit, BeamLength) && (Hit.collider.tag == "ProjectileDestroyer")) || ((Physics.Raycast(Ray2, out Hit, BeamLength) && (Hit.collider.tag == "ProjectileDestroyer"))))
                    {
                        gameObject.SetActive(false);
                        //Destroy (gameObject);
                    }
                    switch (strOwningObject)
                    {
                        case "Player":
                            // if raycast hits a gameobject tagged with Enemies and the EnemyHealth is greater than 0 deal Damage and destroy the missle.
                            if (((Physics.Raycast(Ray1, out Hit, BeamLength) && (Hit.collider.tag == "Enemies"))) || ((Physics.Raycast(Ray2, out Hit, BeamLength) && (Hit.collider.tag == "Enemies"))))
                            {
                                targetHit = Hit.collider.gameObject;
                                if (targetHit.GetComponent<EnemyControl>().EnemyHealth > 0)
                                {
                                    targetHit.GetComponent<EnemyControl>().EnemyHealth -= Damage;
                                    gameObject.SetActive(false);
                                    //Destroy (gameObject);
                                }
                            }
                            break;
                        case "Enemy":
                            // if raycast hits a gameobject tagged with Enemies and the EnemyHealth is greater than 0 deal Damage and destroy the missle.
                            if (((Physics.Raycast(Ray1, out Hit, BeamLength) && (Hit.collider.tag == "Player"))) || ((Physics.Raycast(Ray2, out Hit, BeamLength) && (Hit.collider.tag == "Player"))))
                            {
                                targetHit = Hit.collider.gameObject;
                                if (targetHit.GetComponent<Player_Control>().currentHealth > 0)
                                {
                                    targetHit.GetComponent<Player_Control>().DamagePlayer(Damage);
                                    gameObject.SetActive(false);
                                    //Destroy (gameObject);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            // if the raycast doesn't hit anytihng tagged with "Enemies"  the targethit is null.
            else
            {
                targetHit = null;
            }
        }
    }
    public void SetOwner(string strOwner)
    {
        strOwningObject = strOwner;
    }
    public string GetOwner()
    {
        return strOwningObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (bUseCollision)
        {
            if (other.gameObject.tag == "ProjectileDestroyer")
            {
                gameObject.SetActive(false);
                //Destroy (gameObject);
            }
            switch (strOwningObject)
            {
                case "Player":
                    // if raycast hits a gameobject tagged with Enemies and the EnemyHealth is greater than 0 deal Damage and destroy the missle.
                    if (other.gameObject.tag == "Enemies")
                    {
                        targetHit = other.gameObject;
                        if (targetHit.GetComponent<EnemyControl>().EnemyHealth > 0)
                        {
                            targetHit.GetComponent<EnemyControl>().EnemyHealth -= Damage;
                            gameObject.SetActive(false);
                            //Destroy (gameObject);
                        }
                    }
                    break;
                case "Enemy":
                    // if raycast hits a gameobject tagged with Enemies and the EnemyHealth is greater than 0 deal Damage and destroy the missle.
                    if (other.gameObject.tag == "Player")
                    {
                        targetHit = other.gameObject;
                        if (targetHit.GetComponent<Player_Control>().currentHealth > 0)
                        {
                            targetHit.GetComponent<Player_Control>().DamagePlayer(Damage);
                            gameObject.SetActive(false);
                            //Destroy (gameObject);
                        }
                    }
                    break;
                default:
                    targetHit = null;
                    break;
            }
        }
    }
}