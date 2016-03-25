using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public enum FireMode {
        Auto,
        Burst,
        Single
    }

    public FireMode fireMode;

	public Transform[] projectileSpawn;
	public Projectile projectile;
	public float msBetweenShot = 100f;
	public float muzzleVelocity = 35f;

    public int burstCount;


    public Transform shell;
    public Transform shellEjection;

    Muzzleflash muzzleFlash;

	float nextShotTime;

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;

    void Start() {
        muzzleFlash = GetComponent<Muzzleflash>();
        shotsRemainingInBurst = burstCount;
    }

	void Shoot(){

		if(Time.time > nextShotTime){

	            if( fireMode == FireMode.Burst ) {
	                if ( shotsRemainingInBurst == 0 ) {
	                    return;
	                }                    
	                shotsRemainingInBurst--;

	            }

	            else if ( fireMode == FireMode.Single ) {
	                if ( !triggerReleasedSinceLastShot ) {
	                    return;
	                }
	                shotsRemainingInBurst--;

	            }

	            for ( int i = 0; i < projectileSpawn.Length; i++ ) {
	                nextShotTime = Time.time + msBetweenShot / 1000;
	                Projectile newProjectile = Instantiate( projectile, projectileSpawn[i].position, projectileSpawn[i].rotation ) as Projectile;
	                newProjectile.SetSpeed( muzzleVelocity );
	            }

	            Instantiate( shell, shellEjection.position, shellEjection.rotation );
	            muzzleFlash.Activate();
	        }

	}

    public void OnTriggerHold() {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelase() {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }

}
