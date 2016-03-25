using UnityEngine;
using System.Collections;

public class AutoShotMenu : MonoBehaviour {

	private GunController gunController;
	public float msBetweenShot = 1500f;
	float nextShotTime;

	// Use this for initialization
	void Start () {
		gunController = GetComponent<GunController>();
		gunController.startingGun.fireMode = Gun.FireMode.Single;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Time.time > nextShotTime){
			gunController.OnTriggerHold();
			nextShotTime = Time.time + (msBetweenShot * Random.Range(1,10)) / 1000;
			gunController.OnTriggerRelease();
		}
	}
}
