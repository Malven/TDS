using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] behavioursToDisable;

	// Use this for initialization
	void Start () {
        if ( !isLocalPlayer ) {
            for ( int i = 0; i < behavioursToDisable.Length; i++ ) {
                behavioursToDisable[ i ].enabled = false;
            }
        } else {
            SmoothFollow sf = Camera.main.GetComponent<SmoothFollow>();
            sf.target = gameObject.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
