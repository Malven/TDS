﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed = 5f;
	public LayerMask collisionMask;

	float damage = 1f;

	float lifeTime = 3f;

	float skinWidth = 0.1f;

	void Start() {
		Destroy(gameObject, lifeTime);

		Collider[] initialColliders = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
		if(initialColliders.Length > 0) {
			OnHitObject(initialColliders[0]);
		}
	}

	public void SetSpeed(float newSpeed){
		speed = newSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions(moveDistance);
		transform.Translate(Vector3.forward * moveDistance);
	}

	void CheckCollisions(float _moveDistance) {
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, _moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit);
		}
	}

	void OnHitObject(RaycastHit hit) {
		IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
		if(damageableObject != null) {
			damageableObject.TakeHit(damage, hit);
		}
		GameObject.Destroy(gameObject);
	}

	void OnHitObject( Collider c){
		IDamageable damageableObject = c.GetComponent<IDamageable>();
		if(damageableObject != null) {
			damageableObject.TakeDamage(damage);
		}
		GameObject.Destroy(gameObject);
	}
}
