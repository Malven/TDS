using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed = 5f;
	public LayerMask[] collisionMask;
    public Color trailColor;

	float damage = 1f;

	float lifeTime = 3f;

	float skinWidth = 0.1f;

	void Start() {
		Destroy(gameObject, lifeTime);

		Collider[] initialColliders = Physics.OverlapSphere(transform.position, 0.1f, collisionMask[0]);
		if(initialColliders.Length > 0) {
			OnHitObject(initialColliders[0], transform.position);
		}

        GetComponent<TrailRenderer>().material.SetColor( "_TintColor", trailColor );
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

		if(Physics.Raycast(ray, out hit, _moveDistance + skinWidth, collisionMask[0], QueryTriggerInteraction.Collide)) {
			OnHitObject(hit.collider, hit.point);
		}
        if (Physics.Raycast(ray, out hit, _moveDistance + skinWidth, collisionMask[1], QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }

	void OnHitObject( Collider c, Vector3 hitPoint ){
		IDamageable damageableObject = c.GetComponent<IDamageable>();
		if(damageableObject != null) {
            damageableObject.TakeHit( damage, hitPoint, transform.forward );
		}
        else
        {
            Destroy(gameObject);
        }
		GameObject.Destroy(gameObject);
	}
}
