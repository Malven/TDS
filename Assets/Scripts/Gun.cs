﻿using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform muzzle;
	public Projectile projectile;
	public float msBetweenShot = 100f;
	public float muzzleVelocity = 35f;

	float nextShotTime;

	public void Shoot(){

		if(Time.time > nextShotTime){
			nextShotTime = Time.time + msBetweenShot / 1000;
			Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
			newProjectile.SetSpeed(muzzleVelocity);
		}

	}

}
