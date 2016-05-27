using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour {
    public PowerUp powerupPrefab;
    public float timeBetweenPowerupSpawns = 5f;
    public float timeSinceLastSpawn = 0;
    public bool dontSpawn = false;
    MapGenerator map;
	// Use this for initialization
	void Start () {
        map = FindObjectOfType<MapGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (dontSpawn)
            return;
        timeSinceLastSpawn += Time.deltaTime;
	    if(timeSinceLastSpawn > timeBetweenPowerupSpawns)
        {
            Transform spawnTile = map.GetRandomOpenTile();
            Instantiate(powerupPrefab, spawnTile.position + Vector3.up, Quaternion.Euler(270,0,0));
            timeSinceLastSpawn = 0;
        }
	}
}
