using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity {

	public float moveSpeed = 5f;
	public Transform crosshairs;
	PlayerController controller;
	GunController gunController;

	Camera viewCamera;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		controller = GetComponent<PlayerController>();
		viewCamera = Camera.main;
		gunController = GetComponent<GunController>();
	}
	
	// Update is called once per frame
	void Update () {
		//Movement INput
		Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move(moveVelocity);

		//Look input
		Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up,Vector3.up * gunController.GunHeight);
		float rayDistance;

		if(groundPlane.Raycast(ray, out rayDistance)) {
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine(ray.origin, point, Color.red);
			controller.LookAt(point);
			crosshairs.position = point;
		}

		//Weapon input
		if(Input.GetMouseButton(0)){
			gunController.OnTriggerHold();
		}

        if ( Input.GetMouseButtonUp( 0 ) ) {
            gunController.OnTriggerRelease();
        }
    }
}
