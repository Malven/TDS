﻿using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;

    float lifeTime = 4f;
    float fadeTime = 2f;

	// Use this for initialization
	void Start () {
        float force = Random.Range( forceMin, forceMax );
        myRigidbody.AddForce( transform.right * force );
        myRigidbody.AddTorque( Random.insideUnitSphere * force );
        StartCoroutine( Fade() );
	}
	
    IEnumerator Fade() {
        yield return new WaitForSeconds( lifeTime );

        float percent = 0;
        float fadeSpeed = 1 / fadeTime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColor = mat.color;

        while (percent < 1) {
            percent = Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp( initialColor, Color.clear, percent );
            yield return null;
        }

        Destroy( gameObject );
    }
}
