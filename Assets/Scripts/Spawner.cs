﻿using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public Enemy enemy;


	Wave currentWave;
	int currentWaveNumber;
	int enemiesRemainingToSpawn;
	int enemiesRemainingAlive;
	float nextSpawnTime;
    MapGenerator map;

    LivingEntity playerEntity;
    Transform playerT;

    float timeBetweenCampChecks = 2f;

    
	void Start() {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;
        map = GameObject.FindObjectOfType<MapGenerator>();
		NextWave();
	}

	void Update() {
		if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
			enemiesRemainingToSpawn--;
			nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            StartCoroutine( SpawnEnemy() );

		}
	}

    IEnumerator SpawnEnemy() {
        float spawnDelay = 1f;
        float tileFlashSpeed = 4f;

        Transform randomTile = map.GetRandomOpenTile();
        Material tileMat = randomTile.GetComponent<Renderer>().material;
        Color originalColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;
        while ( spawnTimer < spawnDelay ) {
            tileMat.color = Color.Lerp( originalColor, flashColor, Mathf.PingPong( spawnTimer * tileFlashSpeed, 1 ) );

            spawnTimer += Time.deltaTime;
            yield return null;
        }
        Enemy spawnedEnemy = Instantiate( enemy, randomTile.position + Vector3.up, Quaternion.identity ) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

	void OnEnemyDeath() {
		enemiesRemainingAlive--;

		if(enemiesRemainingAlive == 0) {
			NextWave();
		}
	}

	void NextWave(){
		currentWaveNumber++;
		print ("Wave: "+currentWaveNumber);
		if(currentWaveNumber -1 < waves.Length){
			currentWave = waves[currentWaveNumber-1];
		}

		enemiesRemainingToSpawn = currentWave.enemyCount;
		enemiesRemainingAlive = enemiesRemainingToSpawn;
	}

	[System.Serializable]
	public class Wave {
		public int enemyCount;
		public float timeBetweenSpawns;

	}
}
