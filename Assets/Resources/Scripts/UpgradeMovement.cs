using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMovement : MonoBehaviour {
	
	private float rotationSpeed;
	private int rotationRandom;
	private float upgradeRotation;
	public float RotationSpeed;
	private bool rotationLock;
	private float negativeUpgradeRotation;

	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
            gameObject.SetActive(false);
		}
	}

	void Start () 
	{
		rotationLock = false;
        RotationSpeed = 16;
	}


	void Update () 
	{
		negativeUpgradeRotation = -RotationSpeed * Time.deltaTime;
		upgradeRotation = RotationSpeed * Time.deltaTime;
		if (rotationLock == false) 
		{
			rotationRandom = (Random.Range (0, 2));
			rotationLock = true;
		}

		if (rotationRandom == 0)
		{
			rotationSpeed = negativeUpgradeRotation;
		}
		if (rotationRandom == 1)
		{
			rotationSpeed = upgradeRotation;
		}
	// rotates the gameObject in World Space using rotationSpeed.
		transform.Rotate (0, rotationSpeed, 0, Space.World);
	}

	// If player collides with the upgrade it will destroy the upgrade.



}