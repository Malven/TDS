using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {
		Idle,
		Chasing,
		Attacking
	}

	State currentState;
	NavMeshAgent pathfinder;
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;
	Color originalColor;
	float attackDistanceThreshold = .5f;
	float timeBetweenAttacks = 1f;
	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;
	bool hasTarget;
	float damage = 1f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		pathfinder = GetComponent<NavMeshAgent>();
		skinMaterial = GetComponent<Renderer>().material;
		originalColor = skinMaterial.color;

		if(GameObject.FindGameObjectWithTag("Player") != null) {
			target = GameObject.FindGameObjectWithTag("Player").transform;
			hasTarget = true;
			currentState = State.Chasing;
			myCollisionRadius = GetComponent<CapsuleCollider>().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
			targetEntity = target.GetComponent<LivingEntity>();
			targetEntity.OnDeath += OnTargetDeath;
			
			StartCoroutine(UpdatePath());
		}
	}

	void OnTargetDeath(){
		hasTarget = false;
		currentState = State.Idle;
	}
	
	// Update is called once per frame
	void Update () {
		if(hasTarget){
			if(Time.time > nextAttackTime){
				float squareDistanceToTarget = (target.position - transform.position).sqrMagnitude;	
				if(squareDistanceToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)){
					nextAttackTime = Time.time + timeBetweenAttacks;
					StartCoroutine(Attack());
				}
			}
		}
	}

	IEnumerator Attack() {
		currentState = State.Attacking;
		pathfinder.enabled = false;

		skinMaterial.color = Color.red;

		Vector3 originalPosition = transform.position;
		Vector3 directionToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - directionToTarget * (myCollisionRadius);

		float attackSpeed = 3f;
		float percent = 0f;

		bool hasAppliedDamage = false;

		while (percent <= 1) {
			if(percent >= .5f && !hasAppliedDamage){
				hasAppliedDamage = true;
				targetEntity.TakeDamage(damage);
			}
			percent += Time.deltaTime * attackSpeed;
			float interpolationValue = (-Mathf.Pow(percent, 2)+ percent) * 4;
			transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolationValue);

			yield return null;
		}
		skinMaterial.color = originalColor;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}


	IEnumerator UpdatePath() {
		float refreshRate = 0.25f;

		while(hasTarget) {
			if(currentState == State.Chasing){
				Vector3 directionToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - directionToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
				if(!dead){
					pathfinder.SetDestination(targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}
