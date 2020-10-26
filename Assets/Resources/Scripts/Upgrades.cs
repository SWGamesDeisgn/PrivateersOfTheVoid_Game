using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour {

	public int MultiSpawnAmount;
	public int BeamLaserDropped;
	public bool BeamDelay;
	public bool BeamLaserActive;
	public int beamLaserCounter;
    public int UpgradeLifeCounter;

	void start ()
	{
        UpgradeLifeCounter = 0;
        beamLaserCounter = 0;
		BeamLaserActive = false;
		MultiSpawnAmount = 0;
		BeamLaserDropped = 0;
	}
	private void Update ()
	{
		if (BeamLaserActive == false) 
		{
			BeamLaserDropped = 0;
			beamLaserCounter = 0;
		}
	}

	IEnumerator PlayerHasLaser ()
	{
		yield return new WaitForSeconds (4);
		BeamLaserDropped = 0;
		StopCoroutine (PlayerHasLaser ());
	}

	void OnTriggerEnter (Collider UpgradeCol)
	{
		if (UpgradeCol.tag == "UpgradeBeamLaser") {
			BeamLaserActive = true;
			beamLaserCounter += 1;
		}
	}



}
