using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour, IDamageable {

	public float startingHealth;
	protected float health;
	protected bool dead;

	public event System.Action OnDeath;

	protected virtual void Start() {
		health = startingHealth;
	}

	public void TakeHit(float damage, RaycastHit hit){
		//Will do stuff later with raycasthit
		TakeDamage(damage);
	}

	public void TakeDamage(float damage) {
		health -= damage;
		if(health <= 0 && !dead) {
			Die();
		}
	}

    [ContextMenu("Self Destruct")]
	protected void Die() {
		dead = true;
		if(OnDeath != null) {
			OnDeath();
		}
		GameObject.Destroy(gameObject);
	}
}
