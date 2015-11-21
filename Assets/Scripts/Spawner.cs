using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public Enemy enemy;
    public bool devMode;

	Wave currentWave;
	int currentWaveNumber;
	int enemiesRemainingToSpawn;
	int enemiesRemainingAlive;
	float nextSpawnTime;
    MapGenerator map;

    LivingEntity playerEntity;
    Transform playerT;

    float timeBetweenCampingChecks = 2f;
    float campThresholdDistance = 2f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;
    bool isDisabled;

    public event System.Action<int> OnNewWave;
    
	void Start() {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;
        playerEntity.OnDeath += OnPlayerDeath;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        map = GameObject.FindObjectOfType<MapGenerator>();
		NextWave();
	}

	void Update() {
        if ( !isDisabled ) {
            if ( Time.time > nextCampCheckTime ) {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;
                isCamping = ( Vector3.Distance( playerT.position, campPositionOld ) < campThresholdDistance );
                campPositionOld = playerT.position;
            }

            if (( enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime ) {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine( "SpawnEnemy" );
            }
        }
        if ( devMode ) {
            if ( Input.GetKeyDown( KeyCode.Return ) ) {
                StopCoroutine( "SpawnEnemy" );
                foreach ( Enemy enemy in FindObjectsOfType<Enemy>() ) {
                    GameObject.Destroy( enemy.gameObject );
                }
                NextWave();
            }
        }
	}

    IEnumerator SpawnEnemy() {
        float spawnDelay = 1f;
        float tileFlashSpeed = 4f;

        Transform spawnTile = map.GetRandomOpenTile();
        if ( isCamping ) {
            spawnTile = map.GetTileFromPosition( playerT.position );
        }
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color originalColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;
        while ( spawnTimer < spawnDelay ) {
            tileMat.color = Color.Lerp( originalColor, flashColor, Mathf.PingPong( spawnTimer * tileFlashSpeed, 1 ) );

            spawnTimer += Time.deltaTime;
            yield return null;
        }
        Enemy spawnedEnemy = Instantiate( enemy, spawnTile.position + Vector3.up, Quaternion.identity ) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
        spawnedEnemy.SetCharacteristics( currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColor );
    }

    void OnPlayerDeath() {
        isDisabled = true;
    }

	void OnEnemyDeath() {
		enemiesRemainingAlive--;

		if(enemiesRemainingAlive == 0) {
			NextWave();
		}
	}

    void ResetPlayerPosition() {
        playerT.position = map.GetTileFromPosition( Vector3.zero ).position + Vector3.up * 3;
    }

	void NextWave(){
		currentWaveNumber++;
		if(currentWaveNumber -1 < waves.Length){
			currentWave = waves[currentWaveNumber-1];
		}

		enemiesRemainingToSpawn = currentWave.enemyCount;
		enemiesRemainingAlive = enemiesRemainingToSpawn;

        if(OnNewWave != null) {
            OnNewWave( currentWaveNumber );
        }
        ResetPlayerPosition();
    }

	[System.Serializable]
	public class Wave {
        public bool infinite;
		public int enemyCount;
		public float timeBetweenSpawns;
        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
        public Color skinColor;
	}
}